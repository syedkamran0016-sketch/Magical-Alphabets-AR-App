using UnityEngine;

public class AuthUIManager : MonoBehaviour
{
    public GameObject panelChoice;
    public GameObject panelSignUpForm;
    public GameObject panelSignInForm;
    public GameObject panelForgotPassword; // NEW

    public void OpenSignUpForm()
    {
        panelChoice.SetActive(false);
        panelSignUpForm.SetActive(true);
        panelSignInForm.SetActive(false);
        panelForgotPassword.SetActive(false);
    }

    public void OpenSignInForm()
    {
        panelChoice.SetActive(false);
        panelSignInForm.SetActive(true);
        panelSignUpForm.SetActive(false);
        panelForgotPassword.SetActive(false);
    }

    public void OpenForgotPassword()
    {
        panelChoice.SetActive(false);
        panelSignInForm.SetActive(false);
        panelSignUpForm.SetActive(false);
        panelForgotPassword.SetActive(true);
    }

    public void BackToChoice()
    {
        panelChoice.SetActive(true);
        panelSignUpForm.SetActive(false);
        panelSignInForm.SetActive(false);
        panelForgotPassword.SetActive(false);
    }

    public void BackToSignIn()
    {
        panelChoice.SetActive(false);
        panelSignUpForm.SetActive(false);
        panelSignInForm.SetActive(true);
        panelForgotPassword.SetActive(false);
    }
}
