using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Extensions;

public class HomeProfileLoader : MonoBehaviour
{
    public GameObject homeUI; // parent panel of all UI

    public TextMeshProUGUI userNameText;
    public Image userAvatarImage;

    public Sprite[] avatarSprites; // same avatars

    FirebaseAuth auth;
    DatabaseReference dbRef;

    void Start()
    {
        // UI ko pehle hide karo
        if (homeUI != null)
            homeUI.SetActive(false);

        auth = FirebaseAuth.DefaultInstance;

        dbRef = FirebaseDatabase.GetInstance(
            "https://magicalalphabets-default-rtdb.firebaseio.com/"
        ).RootReference;

        LoadProfile();
    }

    void LoadProfile()
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
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string name = snapshot.Child("name").Value.ToString();
                    int avatarIndex = int.Parse(snapshot.Child("avatarIndex").Value.ToString());

                    userNameText.text = "Hello, " + name;

                    if (avatarIndex >= 0 && avatarIndex < avatarSprites.Length)
                    {
                        userAvatarImage.sprite = avatarSprites[avatarIndex];
                    }

                    // UI ko show karo after data load
                    if (homeUI != null)
                        homeUI.SetActive(true);
                }
                else
                {
                    Debug.LogError("Profile data not found in database");
                }
            }
            else
            {
                Debug.LogError("Failed to load profile");
            }
        });
    }
}
