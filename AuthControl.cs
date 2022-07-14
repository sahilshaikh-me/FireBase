using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using System;

public class AuthControl : MonoBehaviour
{
  
   // public InputField LoginPass;

    public string RegisterEmail;
    public string RegisterPass;
    public string Name;
    public bool isLogin;
    public bool isRegister;

    private static AuthControl _instance;
    public static AuthControl instance { get { return _instance; } }



    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
         Register();
        Login();
    }
    private void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LogOut();
        }
        DontDestroyOnLoad(gameObject);
    }
    public void Login()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(RegisterEmail, RegisterPass).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
              //  Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;

                GetErrorMessage((AuthError)e.ErrorCode);
                return;
            }
            if (task.IsCompleted)
            {
               // DBManager.instance.Createuser();

                Debug.Log("Login Successfull Congrats ");
                isLogin = true;
               // LoadScene.Instance.CheckUser();
                //Score.Instance.wr;
               // Score.Instance.LoadData();

            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

    }

  

    public void Register()
    {
        if (RegisterEmail.Equals("") && RegisterPass.Equals("") && Name.Equals(""))
        {
            print("Please Enter Email And Password");
            return;
        }
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(RegisterEmail, RegisterPass).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
              Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;

              GetErrorMessage((AuthError)e.ErrorCode);
              return;
              
            }
          if (task.IsCompleted)
          {
                isRegister = true;
                Debug.Log("Registration is completed");
                 Login();

                //Score.Instance.SavaData();
                //  LoadScene.Instance.CheckUser();
                // Score.Instance.LoadData();




            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
           
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
       

    }

    public void AnonymousLogin()
    {
        FirebaseAuth.DefaultInstance.SignInAnonymouslyAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Firebase.FirebaseException e = task.Exception.Flatten().InnerExceptions[0] as Firebase.FirebaseException;

                GetErrorMessage((AuthError)e.ErrorCode);
                return;

            }
            if (task.IsCompleted)
            {
                Debug.Log(" Anonymous Login Success");
            }
        });
    }
    private void GetErrorMessage(AuthError errorCode)
    {
        string msg = "";
        msg = errorCode.ToString();

        print(msg);
    }
    public void LogOut()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignOut();
      //  LoadScene.Instance.LoginPanel.SetActive(true);
        
    }

}
