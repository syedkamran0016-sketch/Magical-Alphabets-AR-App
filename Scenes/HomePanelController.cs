using UnityEngine;

public class HomePanelController : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject settingsPanel;
    public GameObject profilePanel;
    public GameObject privacyPanel;
    public GameObject notificationPanel;

    void Awake()
    {
        ShowHome(); // Default screen
    }

    // 🔹 HOME SCREEN
    public void ShowHome()
    {
        homePanel.SetActive(true);
        settingsPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    // 🔹 SETTINGS SCREEN
    public void ShowSettings()
    {
        homePanel.SetActive(false);
        settingsPanel.SetActive(true);
        profilePanel.SetActive(false);
        privacyPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    // 🔹 PROFILE SCREEN
    public void ShowProfile()
    {
        homePanel.SetActive(false);
        settingsPanel.SetActive(false);
        profilePanel.SetActive(true);
        privacyPanel.SetActive(false);
        notificationPanel.SetActive(false);
    }

    // 🔹 PRIVACY SCREEN
    public void ShowPrivacy()
    {
        homePanel.SetActive(false);
        settingsPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPanel.SetActive(true);
        notificationPanel.SetActive(false);
    }

    // 🔹 NOTIFICATION SCREEN
    public void ShowNotification()
    {
        homePanel.SetActive(false);
        settingsPanel.SetActive(false);
        profilePanel.SetActive(false);
        privacyPanel.SetActive(false);
        notificationPanel.SetActive(true);
    }

    // 🔹 BACK BUTTONS
    public void BackFromProfile()
    {
        ShowSettings();
    }

    public void BackFromPrivacy()
    {
        ShowSettings();
    }

    public void BackFromNotification()
    {
        ShowSettings();
    }

    public void BackFromSettings()
    {
        ShowHome();
    }
}
