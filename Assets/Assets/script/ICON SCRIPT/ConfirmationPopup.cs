using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using TMPro;

public class ConfirmationPopup : MonoBehaviour
{
    [Header("UI References")]
    public GameObject confirmationPanel;
    public TMP_Text messageText;

    private string actionType = "";

    void Start()
    {
        // Make sure popup is hidden at start
        confirmationPanel.SetActive(false);
    }

    // 🔹 SWITCH BUTTON CLICK
    public void ShowSwitchConfirmation()
    {
        confirmationPanel.SetActive(true);
        messageText.text = "Do you want to switch account?";
        actionType = "switch";
    }

    // 🔹 LOGOUT BUTTON CLICK
    public void ShowLogoutConfirmation()
    {
        confirmationPanel.SetActive(true);
        messageText.text = "Are you sure you want to logout?";
        actionType = "logout";
    }

    // 🔹 YES BUTTON
    public void OnYesClicked()
    {
        confirmationPanel.SetActive(false);

        if (actionType == "switch")
        {
            // DO NOT logout
            SceneManager.LoadScene("AuthScene");
        }
        else if (actionType == "logout")
        {
            FirebaseAuth.DefaultInstance.SignOut();
            SceneManager.LoadScene("AuthScene");
        }

        actionType = "";
    }

    // 🔹 NO BUTTON
    public void OnNoClicked()
    {
        confirmationPanel.SetActive(false);
        actionType = "";
    }
}
