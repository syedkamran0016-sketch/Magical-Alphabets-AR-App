using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth;

public class StartupManager : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Startup scene running");

        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            Debug.Log("User already logged in");
            SceneManager.LoadScene("HomeScene");
        }
        else
        {
            Debug.Log("User not logged in");
            SceneManager.LoadScene("AuthScene");
        }
    }
}
