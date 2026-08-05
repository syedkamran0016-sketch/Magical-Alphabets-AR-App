using UnityEngine;
using Firebase.Auth;

public class AuthScreenManager : MonoBehaviour
{
    public GameObject Panel_SignInForm;
    public GameObject Panel_SignUpForm;
    public GameObject profilePanel;

    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Check if user already logged in
        if (auth.CurrentUser != null)
        {
            ShowProfile();
        }
    }

    public void OnLoginSuccess()
    {
        ShowProfile();
    }

    public void OnSignupSuccess()
    {
        ShowProfile();
    }

    void ShowProfile()
    {
       Panel_SignInForm.SetActive(false);
     Panel_SignUpForm.SetActive(false);
        profilePanel.SetActive(true);
    }

    public void Logout()
    {
        auth.SignOut();
        profilePanel.SetActive(false);
   Panel_SignInForm.SetActive(true);
    }
}
