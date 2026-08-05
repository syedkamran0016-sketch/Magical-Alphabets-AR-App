using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject normalScreen;
    public GameObject selectScreen;
    public GameObject resultRightScreen;
    public GameObject resultWrongScreen;
    public GameObject successScreen;

    [Header("Images")]
    public Image questionImageNormal;
    public Image questionImageSelect;
    public Image questionImageRight;
    public Image questionImageWrong;

    [Header("Buttons")]
    public Button[] normalButtons;
    public Button[] selectButtons;
    public Button[] rightButtons;
    public Button[] wrongButtons;

    [Header("Texts")]
    public TMP_Text[] normalTexts;
    public TMP_Text[] selectTexts;
    public TMP_Text[] rightTexts;
    public TMP_Text[] wrongTexts;

    [Header("Success UI")]
    public TMP_Text successTimeText;
    public TMP_Text successMistakeText;
    public TMP_Text levelCompleteText;

    [Header("Level Settings")]
    public int currentLevel = 1;
    public int questionsPerBatch = 10;

    [Header("Audio System")]
    public AudioSource audioSource;
    public AudioClip rightSound;
    public AudioClip wrongSound;

    [Header("Question Audio Button")]
    public Button playAudioButton;

    [Header("Result Buttons")]
    public Button nextButton;
    public Button tryAgainButton;

    [System.Serializable]
    public class Question
    {
        public Sprite image;
        public string correctAnswer;
        public string[] options = new string[4];
        public AudioClip questionAudio;
    }

    [System.Serializable]
    public class Level
    {
        public List<Question> questions = new List<Question>();
    }

    [Header("All Levels")]
    public List<Level> levels = new List<Level>();

    private List<Question> currentQuestions;

    int currentIndex = 0;
    int batchCorrect = 0;
    int mistakeCount = 0;

    string selectedAnswer = "";
    float startTime;
    bool answered = false;

    // ✅ AUTO AUDIO FLAG (NEW)
    bool autoAudioPlayed = false;

    // ===================== SAVE & CONTINUE SYSTEM =====================
    void SaveProgress()
    {
        PlayerPrefs.SetInt("SavedLevel", currentLevel);
        PlayerPrefs.SetInt("SavedIndex", currentIndex);
        PlayerPrefs.SetInt("SavedBatchCorrect", batchCorrect);
        PlayerPrefs.SetInt("SavedMistakes", mistakeCount);
        PlayerPrefs.SetFloat("SavedStartTime", startTime);
        PlayerPrefs.Save();
    }

    // ==================================================================

    void Start()
    {
        // ✅ Load saved progress if exists
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            currentLevel = PlayerPrefs.GetInt("SavedLevel");
            currentIndex = PlayerPrefs.GetInt("SavedIndex");
            batchCorrect = PlayerPrefs.GetInt("SavedBatchCorrect");
            mistakeCount = PlayerPrefs.GetInt("SavedMistakes");
            startTime = PlayerPrefs.GetFloat("SavedStartTime");
        }
        else
        {
            startTime = Time.time; // first start
        }

        LoadLevel(currentLevel);
        LoadQuestion();
        ShowNormalScreen();
    }

    void LoadLevel(int levelNumber)
    {
        if (levelNumber - 1 < levels.Count)
        {
            currentQuestions = levels[levelNumber - 1].questions;
            ShuffleQuestions();
        }
        else
        {
            Debug.Log("No More Levels!");
        }
    }

    void LoadQuestion()
    {
        if (currentQuestions == null || currentQuestions.Count == 0) return;

        if (currentIndex >= currentQuestions.Count)
            currentIndex = 0;

        answered = false;
        selectedAnswer = "";
        autoAudioPlayed = false; // reset every question

        var q = currentQuestions[currentIndex];

        questionImageNormal.sprite = q.image;
        questionImageSelect.sprite = q.image;

        if (questionImageRight) questionImageRight.sprite = q.image;
        if (questionImageWrong) questionImageWrong.sprite = q.image;

        for (int i = 0; i < 4; i++)
        {
            normalTexts[i].text = q.options[i];
            selectTexts[i].text = q.options[i];
            rightTexts[i].text = q.options[i];
            wrongTexts[i].text = q.options[i];

            normalButtons[i].interactable = true;
            normalButtons[i].image.color = Color.white;

            int id = i;
            normalButtons[i].onClick.RemoveAllListeners();
            normalButtons[i].onClick.AddListener(() => OnNormalClick(id));
        }

        if (playAudioButton != null)
        {
            playAudioButton.onClick.RemoveAllListeners();

            if (q.questionAudio != null)
            {
                playAudioButton.gameObject.SetActive(true);
                playAudioButton.onClick.AddListener(() =>
                {
                    audioSource.PlayOneShot(q.questionAudio);
                });
            }
            else
            {
                playAudioButton.gameObject.SetActive(false);
            }
        }

        // ✅ AUTO PLAY ON NORMAL SCREEN (ONLY ONCE)
        if (q.questionAudio != null && !autoAudioPlayed)
        {
            audioSource.PlayOneShot(q.questionAudio);
            autoAudioPlayed = true;
        }
    }

    void OnNormalClick(int id)
    {
        selectedAnswer = normalTexts[id].text;
        ShowSelectScreen();

        for (int i = 0; i < selectButtons.Length; i++)
        {
            var outline = selectButtons[i].GetComponent<Outline>();
            if (outline) outline.enabled = (i == id);
        }
    }

    public void OnCheckPressed()
    {
        if (answered) return;
        answered = true;

        var q = currentQuestions[currentIndex];

        if (selectedAnswer == q.correctAnswer)
        {
            batchCorrect++;
            ShowRightScreen();
            Highlight(rightButtons, rightTexts, q.correctAnswer, Color.green);

            if (rightSound != null)
                audioSource.PlayOneShot(rightSound);

            StartCoroutine(ShakeButton(nextButton));
            Handheld.Vibrate();
        }
        else
        {
            mistakeCount++;
            ShowWrongScreen();
            Highlight(wrongButtons, wrongTexts, selectedAnswer, Color.red);

            if (wrongSound != null)
                audioSource.PlayOneShot(wrongSound);

            StartCoroutine(ShakeButton(tryAgainButton));
            Handheld.Vibrate();
        }
    }

    public void OnNextQuestion()
    {
        currentIndex++;

        SaveProgress(); // ✅ Save progress here

        if (batchCorrect >= questionsPerBatch)
        {
            ShowSuccess();
            return;
        }

        LoadQuestion();
        ShowNormalScreen();
    }

    public void OnTryAgain()
    {
        answered = false;
        selectedAnswer = "";
        ShowNormalScreen();
        LoadQuestion();
    }

    void ShowSuccess()
    {
        float t = Time.time - startTime;
        int m = Mathf.FloorToInt(t / 60);
        int s = Mathf.FloorToInt(t % 60);

        successTimeText.text = "Time: " + m + ":" + s.ToString("00");
        successMistakeText.text = "Mistakes: " + mistakeCount;
        levelCompleteText.text = "Level " + currentLevel + " Completed!";

        successScreen.SetActive(true);
        normalScreen.SetActive(false);
        selectScreen.SetActive(false);
        resultRightScreen.SetActive(false);
        resultWrongScreen.SetActive(false);
    }

    public void OnSuccessContinue()
    {
        currentLevel++;
        
        // ✅ LOOP BACK TO LEVEL 1 IF ALL LEVELS COMPLETED
        if (currentLevel > levels.Count)
            currentLevel = 1;

        batchCorrect = 0;
        currentIndex = 0;
        mistakeCount = 0;
        startTime = Time.time;

        SaveProgress(); // ✅ Save new level

        LoadLevel(currentLevel);
        LoadQuestion();
        ShowNormalScreen();
    }

    // ================== ADDITIONAL FUNCTIONS ==================
    void ShowNormalScreen() { normalScreen.SetActive(true); selectScreen.SetActive(false); resultRightScreen.SetActive(false); resultWrongScreen.SetActive(false); successScreen.SetActive(false); }
    void ShowSelectScreen() { normalScreen.SetActive(false); selectScreen.SetActive(true); resultRightScreen.SetActive(false); resultWrongScreen.SetActive(false); successScreen.SetActive(false); }
    void ShowRightScreen() { normalScreen.SetActive(false); selectScreen.SetActive(false); resultRightScreen.SetActive(true); resultWrongScreen.SetActive(false); successScreen.SetActive(false); }
    void ShowWrongScreen() { normalScreen.SetActive(false); selectScreen.SetActive(false); resultRightScreen.SetActive(false); resultWrongScreen.SetActive(true); successScreen.SetActive(false); }

    void Highlight(Button[] buttons, TMP_Text[] texts, string answer, Color color)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (texts[i].text == answer)
            {
                buttons[i].image.color = color;
            }
        }
    }

    IEnumerator ShakeButton(Button btn)
    {
        Vector3 originalPos = btn.transform.localPosition;
        float elapsed = 0f;
        float duration = 0.2f;
        while (elapsed < duration)
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(-5f, 5f);
            btn.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
        btn.transform.localPosition = originalPos;
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < currentQuestions.Count; i++)
        {
            Question temp = currentQuestions[i];
            int randomIndex = Random.Range(i, currentQuestions.Count);
            currentQuestions[i] = currentQuestions[randomIndex];
            currentQuestions[randomIndex] = temp;
        }
    }
}