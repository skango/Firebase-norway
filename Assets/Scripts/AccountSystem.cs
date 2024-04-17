using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;


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
    private DatabaseReference databaseReference;
    FirebaseUser CurrentUser;
    public Avatar[] avatars;
    public static AccountSystem instance;
    public Sprite avatarSprite;
    public TMP_InputField usernameField;
    public TMP_Text ScoreText;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(instance.gameObject);
            }
           
        }
        DontDestroyOnLoad(transform.root.gameObject);
        instance = this;
    }

    void Start()
    {
        
        ImageColor = LoginImages[0].color;
        TextColor = LoginTexts[0].color;
        InitializeFirebase();
        StartCoroutine(GetAvatar());
        
    }


    public Task<List<PlayerData>> FetchTopPlayersAsync()
    {
        var tcs = new TaskCompletionSource<List<PlayerData>>();
        List<PlayerData> topPlayers = new List<PlayerData>();

        databaseReference.Child("Users").OrderByChild("score").LimitToLast(6).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Error retrieving data: " + task.Exception);
                tcs.SetException(task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (!snapshot.Exists)
                {
                    Debug.Log("No data available.");
                    tcs.SetResult(new List<PlayerData>()); // Return an empty list
                    return;
                }

                foreach (DataSnapshot childSnapshot in snapshot.Children)
                {
                    string userID = childSnapshot.Key;
                    long score = childSnapshot.Child("score").Value as long? ?? 0;
                    int avatar = (int)(childSnapshot.Child("avatar").Value as long? ?? 0);
                    string username = childSnapshot.Child("username").Value as string ?? userID; // Retrieve the username
                    topPlayers.Insert(0, new PlayerData(userID, score, avatar, username)); // Include username in PlayerData
                }

                tcs.SetResult(topPlayers);
            }
        });

        return tcs.Task;
    }


    public void SetRating1(int rate)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Ratings/{UID}/question1";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(rate).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }

    public void SetRating2(int rate)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Ratings/{UID}/question2";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(rate).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }

    public void SetRatingStar(int rate)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Ratings/{UID}/stars";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(rate).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }


    public void SetComment1(string body)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Ratings/{UID}/comment1";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(body).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }


    public void SetComment2(string body)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Ratings/{UID}/comment2";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(body).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }
    public void SetUserScore(int score)
    {
        string UID = CurrentUser.UserId;
        // Construct the path
        string path = $"Users/{UID}/score";

        // Set the score value at the specified path
        databaseReference.Child(path).SetValueAsync(score).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });

        databaseReference.Child($"Users/{UID}/username").SetValueAsync(CurrentUser.Email).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set score in the database");
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Score updated successfully");
            }
        });
    }

     public IEnumerator GetAvatar()
    {
        yield return new WaitUntil(() => CurrentUser != null);
        ReadAvatarValue(CurrentUser.UserId);
        LoadScore();
    }

    async void LoadScore()
    {
        ScoreText.text = (await ReadUserScore()).ToString();
    }

    public string GetUsername()
    {
        if (!string.IsNullOrEmpty(CurrentUser.DisplayName))
        {
            return CurrentUser.DisplayName;
        }
        return CurrentUser?.Email;
    }

    public void UpdateUsernameFromProfile()
    {
        
        UpdateDisplayName(usernameField.text);
        FindObjectOfType<ProfilePage>().UpdateUsernames(usernameField.text);
    }

    public void UpdateDisplayName(string newDisplayName)
    {
        FirebaseUser user = CurrentUser;
        if (user != null)
        {
            UserProfile profile = new UserProfile { DisplayName = newDisplayName };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
            });
        }
    }

    public async Task<int> ReadUserScore()
    {
        string UID = CurrentUser.UserId;
        // Construct the path to the user's avatar value
        string path = $"Users/{UID}/score";
        int value = 0;
        // Read from the specified path
        await databaseReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Error accessing the database");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Check if the avatar value exists
                if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int scoreValue))
                {
                    // Use the avatarValue as needed
                    Debug.Log($"Score value: {scoreValue}");
                    value = scoreValue;
                    PlayerPrefs.SetInt("Score", value);
                }
                else
                {

                    // The path does not exist or is not an int
                    Debug.Log("score value does not exist or is not an int");
                }
            }
        });

        return value;
    }

    // Adds a method to write or set the avatar value
    public void SetAvatarValue(int avatarValue)
    {
        string UID = CurrentUser.UserId;
        string path = $"Users/{UID}/avatar";
        databaseReference.Child(path).SetValueAsync(avatarValue).ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Failed to set avatar value in the database. " + 
                    task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Successfully set avatar value in the database.");
            }
        });
    }

    private void Update()
    {
        if (CurrentUser == null && UserHolder.USER != null)
        {
            CurrentUser = UserHolder.USER;
        }

        int emailLoginB = !string.IsNullOrEmpty(EmailLogin.text) ? 1 : 0;
        int passwordLoginB = !string.IsNullOrEmpty(PasswordLogin.text) ? 1 : 0;

        float valueLogin = (emailLoginB * 0.5f) + (passwordLoginB * 0.5f);
        //LoginSlider.value = valueLogin;

        int emailRegisterB = !string.IsNullOrEmpty(emailInputField.text) ? 1 : 0;
        int passwordRegisterB = !string.IsNullOrEmpty(passwordInputField.text) ? 1 : 0;
        int usernameRegisterB = !string.IsNullOrEmpty(UsernameField.text) ? 1 : 0;
        
        float K = 0.8f / 3f;
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
                // Firebase is ready for use
                databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    public int ReadAvatarValue(string UID)
    {
        // Construct the path to the user's avatar value
        string path = $"Users/{UID}/avatar";
        int value = -1;
        // Read from the specified path
        databaseReference.Child(path).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError("Error accessing the database");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Check if the avatar value exists
                if (snapshot.Exists && int.TryParse(snapshot.Value.ToString(), out int avatarValue))
                {
                    // Use the avatarValue as needed
                    Debug.Log($"Avatar value: {avatarValue}");
                    value =  avatarValue;
                    avatars[value].SelectAvatar();
                }
                else
                {
                    
                    // The path does not exist or is not an int
                    Debug.Log("Avatar value does not exist or is not an int");
                }
            }
        });

        return value;
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
            CurrentUser = newUser;
            UserHolder.USER = newUser;
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
            CurrentUser = user;
            UserHolder.USER = user;
            Debug.LogFormat("User signed in successfully: {0} ({1})", user.DisplayName, user.UserId);
            //AvatarPage.SetActive(true);
            LoadScene(1);
        });
    }

    

    public void LoadScene(int index)
    {
        
        SceneManager.LoadScene(index);
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

public class PlayerData
{
    public string UserID { get; private set; }
    public long Score { get; private set; }
    public int Avatar { get; private set; }
    public string Username { get; private set; } // Added Username property

    public PlayerData(string userID, long score, int avatar, string username) // Include username in the constructor
    {
        UserID = userID;
        Score = score;
        Avatar = avatar;
        Username = username;
    }
}

