using UnityEngine;
using UnityEngine.SceneManagement;

public class IconSceneController : MonoBehaviour
{
    public GameObject profilePanel;
    public GameObject settingsPanel;
    public GameObject notificationPanel;

    void Start()
    {
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        notificationPanel.SetActive(false);

        string panel = PlayerPrefs.GetString("OPEN_PANEL", "");

        if (panel == "PROFILE")
            profilePanel.SetActive(true);
        else if (panel == "SETTINGS")
            settingsPanel.SetActive(true);
        else if (panel == "NOTIFICATION")
            notificationPanel.SetActive(true);
    }

    public void BackToHome()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
