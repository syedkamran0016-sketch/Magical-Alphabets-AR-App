using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class PrivacyManager : MonoBehaviour
{
    [Header("PANELS")]
    public GameObject privacyPanel;
    public GameObject profilePanel;

    [Header("TEXT UI")]
    public TMP_Text nameText;
    public TMP_Text emailText;
    public TMP_Text passwordText;
    public TMP_Text statusText;

    FirebaseAuth auth;
    DatabaseReference dbRef;

    void OnEnable()
    {
        auth = FirebaseAuth.DefaultInstance;

        dbRef = FirebaseDatabase.GetInstance(
            "https://magicalalphabets-default-rtdb.firebaseio.com/"
        ).RootReference;

        LoadPrivacyData();
    }

    void LoadPrivacyData()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        // Show Email
        emailText.text = auth.CurrentUser.Email;

        // Hide Password
        passwordText.text = "********";

        // Load Name from database
        dbRef.Child("users").Child(userId).Child("name")
        .GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result.Exists)
            {
                nameText.text = task.Result.Value.ToString();
            }
        });

        statusText.text = "";
    }

    // ===============================
    // 🔹 CHANGE NAME (Go To Profile)
    // ===============================
    public void ChangeName()
    {
        privacyPanel.SetActive(false);
        profilePanel.SetActive(true);
    }

    // ===============================
    // 🔹 CHANGE EMAIL
    // ===============================
    public void ChangeEmail()
    {
        if (auth == null || auth.CurrentUser == null)
            return;

        auth.SendPasswordResetEmailAsync(auth.CurrentUser.Email)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                statusText.text = "Email sent. Check your inbox.";
            }
            else
            {
                statusText.text = "Failed to send email.";
                Debug.LogError(task.Exception);
            }
        });
    }

    // ===============================
    // 🔹 CHANGE PASSWORD
    // ===============================
    public void ChangePassword()
    {
        if (auth == null || auth.CurrentUser == null)
            return;

        auth.SendPasswordResetEmailAsync(auth.CurrentUser.Email)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                statusText.text = "Password reset link sent.";
            }
            else
            {
                statusText.text = "Failed to send reset link.";
                Debug.LogError(task.Exception);
            }
        });
    }
}
