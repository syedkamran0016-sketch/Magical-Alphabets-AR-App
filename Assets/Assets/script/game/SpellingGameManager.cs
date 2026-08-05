using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpellingGameManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string word;
        public string missingLetter;
        public Sprite image;
    }

    [System.Serializable]
    public class Level
    {
        public List<Question> questions = new List<Question>();
    }

    [Header("Panels")]
    public GameObject bluePanel;
    public GameObject greenPanel;
    public GameObject redPanel;
    public GameObject completedPanel;

    [Header("UI")]
    public Image questionImage;
    public TextMeshProUGUI answerText;
    public Button[] buttons;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI finalMistakeText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelCompletedText;
    public Button backHomeButton;
    public Button continueButton;

    [Header("Levels (Add manually)")]
    public List<Level> levels = new List<Level>();

    private List<Question> currentLevelQuestions;
    private Question currentQuestion;

    private int questionIndex = 0;
    private int mistakes = 0;
    private float timer = 0f;
    private bool isPlaying = false;

    private int level;
    private int questionsPerLevel = 10;

    private int maxLevels
    {
        get { return levels.Count; }
    }

    void Start()
    {
        level = PlayerPrefs.GetInt("SavedLevel", 1);

        if (level > maxLevels)
        {
            level = 1;
            PlayerPrefs.SetInt("SavedLevel", level);
            PlayerPrefs.Save();
        }

        if (backHomeButton != null)
        {
            backHomeButton.onClick.RemoveAllListeners();
            backHomeButton.onClick.AddListener(() => SceneManager.LoadScene("HomeScene"));
        }

        if (continueButton != null)
        {
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(ContinueGame);
        }

        StartLevel();
    }

    void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;

            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);

            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    void StartLevel()
    {
        questionIndex = 0;
        mistakes = 0;
        timer = 0f;

        timerText.text = "00:00";

        isPlaying = true;

        levelText.text = "Level: " + level;

        currentLevelQuestions = levels[level - 1].questions;

        if (currentLevelQuestions == null || currentLevelQuestions.Count == 0)
        {
            Debug.LogError("No questions in Level " + level);
            return;
        }

        ShowBlue();
        LoadQuestion();
    }

    void ContinueGame()
    {
        completedPanel.SetActive(false);

        level++;

        if (level > maxLevels)
        {
            level = 1;
        }

        PlayerPrefs.SetInt("SavedLevel", level);
        PlayerPrefs.Save();

        StartLevel();
    }

    void LoadQuestion()
    {
        if (questionIndex >= questionsPerLevel)
        {
            LevelCompleted();
            return;
        }

        currentQuestion = currentLevelQuestions[questionIndex];

        questionImage.sprite = currentQuestion.image;

        answerText.text = currentQuestion.word.Replace(currentQuestion.missingLetter, "_");

        SetupButtons();
    }

    void SetupButtons()
    {
        string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        for (int i = 0; i < buttons.Length; i++)
        {
            Button btn = buttons[i];

            string randomLetter = letters[Random.Range(0, letters.Length)].ToString();

            btn.GetComponentInChildren<TextMeshProUGUI>().text = randomLetter;

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => CheckAnswer(btn));
        }

        int correctIndex = Random.Range(0, buttons.Length);

        buttons[correctIndex].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.missingLetter;
    }

    void CheckAnswer(Button btn)
    {
        string selected = btn.GetComponentInChildren<TextMeshProUGUI>().text;

        if (selected == currentQuestion.missingLetter)
        {
            answerText.text = currentQuestion.word;
            StartCoroutine(Correct());
        }
        else
        {
            mistakes++;
            StartCoroutine(Wrong());
        }
    }

    IEnumerator Correct()
    {
        ShowGreen();

        yield return new WaitForSeconds(1f);

        questionIndex++;

        ShowBlue();

        LoadQuestion();
    }

    IEnumerator Wrong()
    {
        ShowRed();

        yield return new WaitForSeconds(1f);

        ShowBlue();
    }

    void LevelCompleted()
    {
        isPlaying = false;

        levelCompletedText.text = "Level " + level + " Completed!";

        finalMistakeText.text = "Time: " + timerText.text + "\nMistakes: " + mistakes;

        completedPanel.SetActive(true);
    }

    void ShowBlue()
    {
        bluePanel.SetActive(true);
        greenPanel.SetActive(false);
        redPanel.SetActive(false);
    }

    void ShowGreen()
    {
        bluePanel.SetActive(false);
        greenPanel.SetActive(true);
        redPanel.SetActive(false);
    }

    void ShowRed()
    {
        bluePanel.SetActive(false);
        greenPanel.SetActive(false);
        redPanel.SetActive(true);
    }
}