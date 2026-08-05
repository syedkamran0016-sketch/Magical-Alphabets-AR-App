using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using TMPro;
using System;

public class UserNotificationLoader : MonoBehaviour
{
    public Transform contentParent;
    public GameObject notificationItemPrefab;

    private FirebaseAuth auth;
    private DatabaseReference db;
    private FirebaseApp app;

    private string databaseURL = "https://magicalalphabets-default-rtdb.firebaseio.com/";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialize FirebaseApp
                var options = new AppOptions()
                {
                    DatabaseUrl = new Uri(databaseURL)
                };

                if (FirebaseApp.DefaultInstance == null)
                    app = FirebaseApp.Create(options, "MainApp");
                else
                    app = FirebaseApp.DefaultInstance;

                auth = FirebaseAuth.GetAuth(app);
                db = FirebaseDatabase.GetInstance(app).GetReference("Notifications");

                LoadNotifications();
                Debug.Log("Firebase Loader Ready ✅");
            }
            else
            {
                Debug.LogError("Firebase dependency error ❌");
            }
        });
    }

    void LoadNotifications()
    {
        if (auth == null || auth.CurrentUser == null || db == null)
        {
            Debug.LogError("Firebase not ready or user not logged in ❌");
            return;
        }

        string uid = auth.CurrentUser.UserId;

        db.Child(uid).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (!task.IsCompleted || !task.Result.Exists)
            {
                Debug.Log("No Notifications Found");
                return;
            }

            foreach (DataSnapshot snapshot in task.Result.Children)
            {
                string message = snapshot.Child("message").Value.ToString();

                GameObject obj = Instantiate(notificationItemPrefab, contentParent);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = message;
            }

            Debug.Log("Notifications Loaded ✅");
        });
    }
}