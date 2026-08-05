using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

public class ProfileEditManager : MonoBehaviour
{
    [Header("TOP HEADER")]
    public TextMeshProUGUI topNameText;
    public Image topAvatarImage;

    [Header("EDIT SECTION")]
    public TMP_InputField nameInput;
    public TMP_InputField ageInput;

    public Image[] avatarImages;
    public Sprite[] avatarSprites;

    private int selectedAvatarIndex = -1;

    FirebaseAuth auth;
    DatabaseReference dbRef;

    void OnEnable()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.GetInstance(
            "https://magicalalphabets-default-rtdb.firebaseio.com/"
        ).RootReference;

        LoadCurrentProfile();
    }

    void LoadCurrentProfile()
    {
        if (auth == null || auth.CurrentUser == null)
            return;

        string userId = auth.CurrentUser.UserId;

        dbRef.Child("users").Child(userId).GetValueAsync()
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                var snapshot = task.Result;

                if (snapshot.Exists)
                {
                    string name = snapshot.Child("name").Value.ToString();
                    int avatarIndex = int.Parse(snapshot.Child("avatarIndex").Value.ToString());

                    nameInput.text = name;
                    topNameText.text = name;

                    selectedAvatarIndex = avatarIndex;

                    HighlightAvatar(avatarIndex);
                    topAvatarImage.sprite = avatarSprites[avatarIndex];

                    if (snapshot.Child("age").Exists)
                        ageInput.text = snapshot.Child("age").Value.ToString();
                }
            }
        });
    }

    public void SelectAvatar(int index)
    {
        selectedAvatarIndex = index;
        HighlightAvatar(index);
    }

    void HighlightAvatar(int index)
    {
        for (int i = 0; i < avatarImages.Length; i++)
        {
            avatarImages[i].color = (i == index) ? Color.green : Color.white;
        }
    }

    public void SaveChanges()
    {
        if (string.IsNullOrEmpty(nameInput.text) || selectedAvatarIndex == -1)
        {
            Debug.Log("Name or avatar missing");
            return;
        }

        string userId = auth.CurrentUser.UserId;

        dbRef.Child("users").Child(userId).Child("name")
            .SetValueAsync(nameInput.text);

        dbRef.Child("users").Child(userId).Child("avatarIndex")
            .SetValueAsync(selectedAvatarIndex);

        dbRef.Child("users").Child(userId).Child("age")
            .SetValueAsync(ageInput.text);

        // 🔥 INSTANT UI UPDATE
        topNameText.text = nameInput.text;
        topAvatarImage.sprite = avatarSprites[selectedAvatarIndex];

        Debug.Log("Profile Updated Successfully");
    }
}
