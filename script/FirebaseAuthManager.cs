using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase;
using UnityEngine.SceneManagement; // scene change ke liye

public class FirebaseAuthManager : MonoBehaviour
{
    FirebaseAuth auth;

    [Header("PANELS")]
    public GameObject signUpPanel;
    public GameObject signInPanel;
    public GameObject forgotPanel;

    [Header("SIGN UP")]
    public TMP_InputField signUpName;
    public TMP_InputField signUpEmail;
    public TMP_InputField signUpPassword;
    public TMP_InputField signUpConfirmPassword;
    public TextMeshProUGUI signUpMessage;

    [Header("SIGN IN")]
    public TMP_InputField signInEmail;
    public TMP_InputField signInPassword;
    public TextMeshProUGUI signInMessage;

    [Header("FORGOT PASSWORD")]
    public TMP_InputField forgotEmail;
    public TextMeshProUGUI forgotMessage;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        SetPasswordHidden(signUpPassword);
        SetPasswordHidden(signUpConfirmPassword);
        SetPasswordHidden(signInPassword);

        FixMobileInput(signUpName);
        FixMobileInput(signUpEmail);
        FixMobileInput(signUpPassword);
        FixMobileInput(signUpConfirmPassword);
        FixMobileInput(signInEmail);
        FixMobileInput(signInPassword);
        FixMobileInput(forgotEmail);
    }

    // ================= PANEL SWITCH =================
    public void OpenSignUpPanel()
    {
        ClearAllMessages();
        ClearAllInputFields();
        signInPanel.SetActive(false);
        forgotPanel.SetActive(false);
        signUpPanel.SetActive(true);
    }

    public void OpenSignInPanel()
    {
        ClearAllMessages();
        ClearAllInputFields();
        signUpPanel.SetActive(false);
        forgotPanel.SetActive(false);
        signInPanel.SetActive(true);
    }

    public void OpenForgotPanel()
    {
        ClearAllMessages();
        ClearAllInputFields();
        signUpPanel.SetActive(false);
        signInPanel.SetActive(false);
        forgotPanel.SetActive(true);
    }

    // ================= SIGN UP =================
    public void SignUpUser()
    {
        signUpMessage.text = "";

        if (string.IsNullOrEmpty(signUpEmail.text) || string.IsNullOrEmpty(signUpPassword.text))
        {
            signUpMessage.text = " Please fill all fields!";
            return;
        }

        if (signUpPassword.text != signUpConfirmPassword.text)
        {
            signUpMessage.text = " Passwords do not match!";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(signUpEmail.text, signUpPassword.text)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                ShowSignUpError(task.Exception);
                return;
            }

            signUpMessage.text = " Account created successfully!";
            ClearSignUpFields();

            // Scene change after signup
            SceneManager.LoadScene("ProfileSetupScene");
        });
    }

    void ShowSignUpError(System.AggregateException ex)
    {
        FirebaseException firebaseEx = ex.GetBaseException() as FirebaseException;
        if (firebaseEx == null) { signUpMessage.text = " Signup Failed!"; return; }

        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        if (errorCode == AuthError.EmailAlreadyInUse) signUpMessage.text = " Email already in use!";
        else if (errorCode == AuthError.InvalidEmail) signUpMessage.text = " Invalid email address!";
        else if (errorCode == AuthError.WeakPassword) signUpMessage.text = " Weak password (min 6 characters)!";
        else signUpMessage.text = " Signup Failed!";
    }

    // ================= SIGN IN =================
    public void SignInUser()
    {
        signInMessage.text = "";

        if (string.IsNullOrEmpty(signInEmail.text) || string.IsNullOrEmpty(signInPassword.text))
        {
            signInMessage.text = " Enter email & password!";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(signInEmail.text, signInPassword.text)
        .ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                ShowSignInError(task.Exception);
                return;
            }

            signInMessage.text = " Login Successful!";
            ClearSignInFields();

            // Scene change after login
            SceneManager.LoadScene("ProfileSetupScene");
        });
    }

    void ShowSignInError(System.AggregateException ex)
    {
        FirebaseException firebaseEx = ex.GetBaseException() as FirebaseException;
        if (firebaseEx == null) { signInMessage.text = " Login Failed!"; return; }

        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

        if (errorCode == AuthError.UserNotFound) signInMessage.text = " Account not found!";
        else if (errorCode == AuthError.WrongPassword) signInMessage.text = " Wrong password!";
        else if (errorCode == AuthError.InvalidEmail) signInMessage.text = " Invalid email!";
        else signInMessage.text = " Login Failed!";
    }

    // ================= FORGOT PASSWORD =================
    public void SendPasswordReset()
    {
        forgotMessage.text = "";

        if (string.IsNullOrEmpty(forgotEmail.text))
        {
            forgotMessage.text = " Enter your email!";
            return;
        }

        auth.SendPasswordResetEmailAsync(forgotEmail.text)
        .ContinueWithOnMainThread(task =>
        {
            forgotMessage.text = task.IsFaulted ? " Error sending reset email." : " Reset link sent to email!";
        });
    }

    // ================= CLEAR =================
    public void ClearAllMessages()
    {
        signUpMessage.text = "";
        signInMessage.text = "";
        forgotMessage.text = "";
    }

    public void ClearAllInputFields()
    {
        signUpName.text = "";
        signUpEmail.text = "";
        signUpPassword.text = "";
        signUpConfirmPassword.text = "";
        signInEmail.text = "";
        signInPassword.text = "";
        forgotEmail.text = "";
    }

    public void ClearSignUpFields()
    {
        signUpName.text = "";
        signUpEmail.text = "";
        signUpPassword.text = "";
        signUpConfirmPassword.text = "";
    }

    public void ClearSignInFields()
    {
        signInEmail.text = "";
        signInPassword.text = "";
    }

    // ================= PASSWORD TOGGLE =================
    public void ToggleSignUpPasswordVisibility() => TogglePassword(signUpPassword);
    public void ToggleConfirmPasswordVisibility() => TogglePassword(signUpConfirmPassword);
    public void ToggleSignInPasswordVisibility() => TogglePassword(signInPassword);

    void TogglePassword(TMP_InputField field)
    {
        field.inputType = field.inputType == TMP_InputField.InputType.Password
            ? TMP_InputField.InputType.Standard
            : TMP_InputField.InputType.Password;
        field.ForceLabelUpdate();
    }

    void SetPasswordHidden(TMP_InputField field)
    {
        field.inputType = TMP_InputField.InputType.Password;
        field.ForceLabelUpdate();
    }

    // ================= MOBILE INPUT FIX =================
    void FixMobileInput(TMP_InputField field)
    {
        field.onDeselect.AddListener(delegate { field.text = field.text; });
    }
}
