using UnityEngine;
using UnityEngine.UI;

public class MatchingGameController : MonoBehaviour
{
    public Image[] progressBars;
    public GameObject completePanel;

    int progress = 0;
    int levelIndex = 0;

    void Start()
    {
        levelIndex = PlayerPrefs.GetInt("Level", 0);
        completePanel.SetActive(false);
    }

    public void CorrectMatch()
    {
        progress++;

        if (progress <= progressBars.Length)
        {
            progressBars[progress - 1].color = Color.green;
        }

        if (progress >= progressBars.Length)
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        completePanel.SetActive(true);
        levelIndex++;
        PlayerPrefs.SetInt("Level", levelIndex);
    }
}
