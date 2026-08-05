using UnityEngine;
using Firebase;
using Firebase.Extensions;

public class FirebaseInit : MonoBehaviour
{
    public static bool IsReady = false;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;

                app.Options.DatabaseUrl = new System.Uri(
                    "https://magicalalphabets-default-rtdb.firebaseio.com/"
                );

                IsReady = true;
                Debug.Log("Firebase Initialized Successfully ✅");
            }
            else
            {
                Debug.LogError("Firebase Dependency Error ❌");
            }
        });
    }
}