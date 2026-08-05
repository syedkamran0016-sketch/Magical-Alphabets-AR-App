using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public Sprite image;
        public string[] options;
        public int correctIndex;
    }

    public List<Question> allQuestions;

    [Header("UI")]
    public Image questionImage;
    public Button[] optionButtons;
    public TMP_Text[] optionTexts;

    public GameObject selectPanel;
    public GameObject resultPanel;
    public GameObject endPanel;

    public TMP_Text resultText;
    public TMP_Text endStatsText;

    public GameObject selectNextButton;
    public GameObject resultNextButton;
    public GameObject tryAgainButton;

    [Header("Sprites")]
    public Sprite normalSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;

    int currentIndex;
    int selectedIndex = -1;
    bool locked = false;

    int correctCount = 0;
    int wrongCount = 0;

    float startTime;

    List<int> remaining = new List<int>();

    // -----------------

    void Start()
    {
        LoadProgress();
        startTime = Time.time;
        LoadQuestion();
    }

    // -----------------
    void LoadProgress()
    {
        remaining.Clear();

        if (PlayerPrefs.HasKey("Remain"))
        {
            foreach (var s in PlayerPrefs.GetString("Remain").Split(','))
                if (s != "") remaining.Add(int.Parse(s));
        }
        else
        {
            for (int i = 0; i < allQuestions.Count; i++)
                remaining.Add(i);
        }

        correctCount = PlayerPrefs.GetInt("Correct", 0);
        wrongCount = PlayerPrefs.GetInt("Wrong", 0);
    }

    void SaveProgress()
    {
        PlayerPrefs.SetString("Remain", string.Join(",", remaining));
        PlayerPrefs.SetInt("Correct", correctCount);
        PlayerPrefs.SetInt("Wrong", wrongCount);
        PlayerPrefs.Save();
    }

    // -----------------
    void LoadQuestion()
    {
        if (correctCount >= 3)
        {
            ShowEndPanel();
            return;
        }

        locked = false;
        selectedIndex = -1;

        int r = Random.Range(0, remaining.Count);
        currentIndex = remaining[r];

        var q = allQuestions[currentIndex];
        questionImage.sprite = q.image;

        for (int i = 0; i < 4; i++)
        {
            optionTexts[i].text = q.options[i];
            optionButtons[i].image.sprite = normalSprite;
            optionButtons[i].interactable = true;
            optionButtons[i].transition = Selectable.Transition.None;

            int id = i;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnOptionClick(id));
        }

        selectPanel.SetActive(false);
        resultPanel.SetActive(false);
        selectNextButton.SetActive(false);
        resultNextButton.SetActive(false);
        tryAgainButton.SetActive(false);
    }

    // -----------------
    public void OnOptionClick(int index)
    {
        if (locked) return;

        locked = true;
        selectedIndex = index;
        StartCoroutine(OpenSelect());
    }

    IEnumerator OpenSelect()
    {
        yield return new WaitForEndOfFrame();
        selectPanel.SetActive(true);
        selectNextButton.SetActive(true);
    }

    // -----------------
    public void OnSelectNext()
    {
        selectPanel.SetActive(false);
        ShowResult();
    }

    void ShowResult()
    {
        var q = allQuestions[currentIndex];
        resultPanel.SetActive(true);

        foreach (var b in optionButtons)
            b.interactable = false;

        optionButtons[q.correctIndex].image.sprite = correctSprite;

        if (selectedIndex == q.correctIndex)
        {
            resultText.text = "RIGHT";
            correctCount++;
            remaining.Remove(currentIndex);
            resultNextButton.SetActive(true);
        }
        else
        {
            resultText.text = "WRONG";
            wrongCount++;
            optionButtons[selectedIndex].image.sprite = wrongSprite;
            tryAgainButton.SetActive(true);
        }

        SaveProgress();
    }

    // -----------------
    public void OnResultNext()
    {
        LoadQuestion();
    }

    public void OnTryAgain()
    {
        resultPanel.SetActive(false);
        LoadQuestion();
    }

    // -----------------
    void ShowEndPanel()
    {
        endPanel.SetActive(true);

        float t = Time.time - startTime;
        int min = (int)t / 60;
        int sec = (int)t % 60;

        endStatsText.text =
            "Correct: " + correctCount +
            "\nWrong: " + wrongCount +
            "\nTime: " + min + ":" + sec.ToString("00");
    }

    // -----------------
    public void ContinueNextBatch()
    {
        correctCount = 0;
        wrongCount = 0;
        startTime = Time.time;
        endPanel.SetActive(false);
        LoadQuestion();
    }

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
    }
}
