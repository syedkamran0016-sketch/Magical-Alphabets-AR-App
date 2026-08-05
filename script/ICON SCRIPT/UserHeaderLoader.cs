using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class UserHeaderLoader : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Image avatarImage;
    public Sprite[] avatarSprites;

    FirebaseAuth auth;
    DatabaseReference dbRef;

    void OnEnable()
    {
        auth = FirebaseAuth.DefaultInstance;

        dbRef = FirebaseDatabase.GetInstance(
            "https://magicalalphabets-default-rtdb.firebaseio.com/"
        ).RootReference;

        LoadUserData();
    }

    void LoadUserData()
    {
        if (auth == null || auth.CurrentUser == null)
        {
            Debug.LogError("User not logged in!");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        dbRef.Child("users").Child(userId).GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted || task.IsFaulted)
                return;

            DataSnapshot snapshot = task.Result;

            if (!snapshot.Exists)
                return;

            // ✅ Safe Name Read
            string name = snapshot.Child("name").Value != null
                ? snapshot.Child("name").Value.ToString()
                : "User";

            // ✅ Safe Avatar Read
            int avatarIndex = 0;
            if (snapshot.Child("avatarIndex").Value != null)
                int.TryParse(snapshot.Child("avatarIndex").Value.ToString(), out avatarIndex);

            // ✅ Set UI
            nameText.text = name;

            if (avatarSprites != null &&
                avatarIndex >= 0 &&
                avatarIndex < avatarSprites.Length)
            {
                avatarImage.sprite = avatarSprites[avatarIndex];
            }
        });
    }
}