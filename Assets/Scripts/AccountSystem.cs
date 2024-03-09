using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountSystem : MonoBehaviour
{
    public FirebaseAuth auth;
    public TMP_InputField emailInputField, EmailLogin;
    public TMP_InputField passwordInputField,PasswordLogin,UsernameField;
    public Image[] LoginImages,RegisterImages;
    public TMP_Text[] LoginTexts, RegisterTexts;
    public Slider LoginSlider, RegisterSlider;
    private Color ImageColor, TextColor;
    public GameObject AvatarPage;
    
    void Start()
    {
        ImageColor = LoginImages[0].color;
        TextColor = LoginTexts[0].color;
        InitializeFirebase();
    }


    private void Update()
    {

        int emailLoginB = !string.IsNullOrEmpty(EmailLogin.text) ? 1 : 0;
        int passwordLoginB = !string.IsNullOrEmpty(PasswordLogin.text) ? 1 : 0;

        float valueLogin = (emailLoginB * 0.5f) + (passwordLoginB * 0.5f);
        LoginSlider.value = valueLogin;

        int emailRegisterB = !string.IsNullOrEmpty(emailInputField.text) ? 1 : 0;
        int passwordRegisterB = !string.IsNullOrEmpty(passwordInputField.text) ? 1 : 0;
        int usernameRegisterB = !string.IsNullOrEmpty(UsernameField.text) ? 1 : 0;
        
        float K = 1f / 3f;
        float valueRegister = (emailRegisterB * K) + (passwordRegisterB * K)
            + (usernameRegisterB * K); 

        RegisterSlider.value = valueRegister;
    }
    public void RedLogin()
    {
        for (int i = 0; i < LoginImages.Length; i++)
        {
            LoginImages[i].color = Color.red;
        }

        for (int i = 0; i < LoginTexts.Length; i++)
        {
            LoginTexts[i].color = Color.red;
        }
    }

    public void RedRegister()
    {
        for (int i = 0; i < RegisterImages.Length; i++)
        {
            RegisterImages[i].color = Color.red;
        }

        for (int i = 0; i < RegisterTexts.Length; i++)
        {
            RegisterTexts[i].color = Color.red;
        }
    }

    void InitializeFirebase()
    {
       
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                
                auth = FirebaseAuth.DefaultInstance;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public void RegisterUser(string email, string password)
    {

        bool failed = false;

        if (string.IsNullOrEmpty(emailInputField.text))
        {
            RegisterImages[0].color = Color.red;
            RegisterTexts[0].color = Color.red;
            failed = true;
        }
        else
        {
            RegisterImages[0].color = ImageColor;
            RegisterTexts[0].color = TextColor;
        }

        if (string.IsNullOrEmpty(UsernameField.text))
        {
            RegisterImages[1].color = Color.red;
            RegisterTexts[1].color = Color.red;
            failed = true;
        }
        else
        {
            RegisterImages[1].color = ImageColor;
            RegisterTexts[1].color = TextColor;
        }

        if (string.IsNullOrEmpty(passwordInputField.text))
        {
            RegisterImages[2].color = Color.red;
            RegisterTexts[2].color = Color.red;
            failed = true;
        }
        else
        {
            RegisterImages[2].color = ImageColor;
            RegisterTexts[2].color = TextColor;
        }

        if (failed)
            return;

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                RedRegister();
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                RedRegister();
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception.Message);
                return;
            }

           
            FirebaseUser newUser = task.Result.User;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            AvatarPage.SetActive(true);
        });
    }

    public void SignInUser(string email, string password)
    {

        bool failed = false;
        if (string.IsNullOrEmpty(EmailLogin.text))
        {
            LoginImages[0].color = Color.red;
            LoginTexts[0].color = Color.red;
            failed = true;
        }
        else
        {
            LoginImages[0].color = ImageColor;
            LoginTexts[0].color = TextColor;
        }

        if (string.IsNullOrEmpty(PasswordLogin.text))
        {
            LoginImages[1].color = Color.red;
            LoginTexts[1].color = Color.red;
            failed = true;
        }
        else
        {
            LoginImages[1].color = ImageColor;
            LoginTexts[1].color = TextColor;
        }
        if (failed)
            return;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                RedLogin();
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                RedLogin();
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            
            FirebaseUser user = task.Result.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
            AvatarPage.SetActive(true);
        });
    }


    public void OnRegisterButtonClick()
    {
        string email = emailInputField.text;
        string password = passwordInputField.text;
        RegisterUser(email, password);
    }

    public void OnSignInButtonClick()
    {
        string email = EmailLogin.text;
        string password = PasswordLogin.text;
        SignInUser(email, password);
    }

}
