using UnityEngine;
using Firebase.Auth;

public class UserEventNotifier : MonoBehaviour
{
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Example: Show login notification if user is already logged in
        if (auth.CurrentUser != null)
        {
            NotificationManagerUI.Instance.ShowNotification(
                "Hello " + auth.CurrentUser.DisplayName + " 👋 Successfully Logged In ✅"
            );
        }
    }

    // Call this method when user changes name
    public void OnUserNameChanged(string newName)
    {
        NotificationManagerUI.Instance.ShowNotification(
            "Your name has been changed to " + newName + " ✅"
        );
    }

    // Call this method when user changes avatar
    public void OnUserAvatarChanged(string avatarName)
    {
        NotificationManagerUI.Instance.ShowNotification(
            "Your avatar has been updated to " + avatarName + " ✅"
        );
    }
}