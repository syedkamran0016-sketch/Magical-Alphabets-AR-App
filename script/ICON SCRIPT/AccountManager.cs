using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{
    FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    // SWITCH ACCOUNT
    public void SwitchAccount()
    {
        // Just go to Auth screen
        SceneManager.LoadScene("AuthScene");
    }

    // LOGOUT ACCOUNT
    public void LogoutAccount()
    {
        auth.SignOut();
        SceneManager.LoadScene("AuthScene");
    }
}
