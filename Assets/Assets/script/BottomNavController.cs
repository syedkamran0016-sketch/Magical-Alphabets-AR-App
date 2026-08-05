using UnityEngine;
using UnityEngine.SceneManagement;

public class BottomNavController : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject profilePanel;
    public GameObject settingsPanel;
    public GameObject notificationPanel;

    void Start()
    {
        ShowHome(); // app open hote hi home
    }

    public void ShowHome()
    {
        homePanel.SetActive(true);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void ShowProfile()
    {
        homePanel.SetActive(false);
        profilePanel.SetActive(true);
        settingsPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        homePanel.SetActive(false);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(true);
        notificationPanel.SetActive(false);
    }

    public void ShowNotification()
    {
        homePanel.SetActive(false);
        profilePanel.SetActive(false);
        settingsPanel.SetActive(false);
        notificationPanel.SetActive(true);
    }

    // 🔥 SCAN ICON (ALAG SCENE)
    public void GoToScan()
    {
        SceneManager.LoadScene("IconScen");
    }
}
