using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class ProfileManager : MonoBehaviour
{
    [Header("PANELS")]
    public GameObject introPanel;
    public GameObject namePanel;
    public GameObject avatarPanel;

    [Header("NAME")]
    public TMP_InputField nameInput;

    [Header("AVATAR")]
    public Image[] avatarImages;
    int selectedAvatar = -1;

    FirebaseAuth auth;
    DatabaseReference dbRef;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // 🔴 IMPORTANT: direct database URL
        dbRef = FirebaseDatabase.GetInstance(
            "https://magicalalphabets-default-rtdb.firebaseio.com/"
        ).RootReference;

        // Debug checks
        if (nameInput == null)
            Debug.LogError("NameInput not assigned in Inspector!");

        if (introPanel == null || namePanel == null || avatarPanel == null)
            Debug.LogError("Panels not assigned in Inspector!");

        // Login check
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("No user logged in!");
        }
        else
        {
            Debug.Log("User logged in: " + auth.CurrentUser.Email);
        }

        ShowIntro();
    }

    // INTRO
    public void ShowIntro()
    {
        introPanel.SetActive(true);
        namePanel.SetActive(false);
        avatarPanel.SetActive(false);
    }

    // INTRO → NAME
    public void GoToName()
    {
        introPanel.SetActive(false);
        namePanel.SetActive(true);
        avatarPanel.SetActive(false);
    }

    // NAME → AVATAR
    public void GoToAvatar()
    {
        if (nameInput == null)
        {
            Debug.LogError("NameInput missing!");
            return;
        }

        if (string.IsNullOrEmpty(nameInput.text))
        {
            Debug.Log("Enter name first");
            return;
        }

        introPanel.SetActive(false);
        namePanel.SetActive(false);
        avatarPanel.SetActive(true);
    }

    // BACK BUTTONS
    public void BackToIntro()
    {
        ShowIntro();
    }

    public void BackToName()
    {
        introPanel.SetActive(false);
        namePanel.SetActive(true);
        avatarPanel.SetActive(false);
    }

    // AVATAR SELECT
    public void SelectAvatar(int index)
    {
        selectedAvatar = index;
        Debug.Log("Selected avatar: " + index);
    }

    // SAVE PROFILE
    public void SaveProfile()
    {
        if (nameInput == null)
        {
            Debug.LogError("NameInput is NULL!");
            return;
        }

        if (string.IsNullOrEmpty(nameInput.text))
        {
            Debug.LogError("Name is empty!");
            return;
        }

        if (selectedAvatar == -1)
        {
            Debug.LogError("Select avatar first");
            return;
        }

        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in!");
            return;
        }

        if (dbRef == null)
        {
            Debug.LogError("Database reference is NULL!");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        ProfileData data = new ProfileData
        {
            name = nameInput.text,
            avatarIndex = selectedAvatar
        };

        string json = JsonUtility.ToJson(data);

        dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Profile Saved!");
                SceneManager.LoadScene("HomeScene");
            }
            else
            {
                Debug.LogError("Profile save failed: " + task.Exception);
            }
        });
    }
}

[System.Serializable]
public class ProfileData
{
    public string name;
    public int avatarIndex;
}
