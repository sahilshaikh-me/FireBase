using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System;

public class DBManager : MonoBehaviour
{

    private string userID;
    public User newUser;
    private DatabaseReference databaseReference;

    public string[] FrndList;

    private static DBManager _instance;
    public static DBManager instance { get { return _instance; } }

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

    // Start is called before the first frame update
    void Start()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        Createuser();
        //FrndList = new string[2] { "sahil", "mariyam" };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetUserInfo();
        }
    }

    public void Createuser()
    {
         newUser = new User(AuthControl.instance.Name, 1.ToString(), FrndList);
        string json = JsonUtility.ToJson(newUser);
        databaseReference.Child("User").Child(AuthControl.instance.Name).SetRawJsonValueAsync(json);
    }

    public IEnumerator GetName(Action<string> onCallback)
    {
        var userNameData = databaseReference.Child("User").Child(AuthControl.instance.Name).Child("username").GetValueAsync();
        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);
        if(userNameData != null)
        {
            DataSnapshot dataSnapshot = userNameData.Result;
            onCallback.Invoke(dataSnapshot.Value.ToString());
        }
    } 
    public IEnumerator FriendList()
    {
        var userNameData = databaseReference.Child("User").Child(AuthControl.instance.Name).Child("FriendLists").GetValueAsync();
        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);
        if (userNameData != null)
        {
            DataSnapshot dataSnapshot = userNameData.Result;

            if (dataSnapshot != null && dataSnapshot.ChildrenCount > 0)
            {
                foreach (var childSnapshot in dataSnapshot.Children)
                {
                    var Friendist = childSnapshot.Value.ToString();

                    Debug.Log(Friendist + "LLLLLLLLLLLLLLL");/////////////>>>>>>>>>>>>>>>>>>>Get friendList Here<<<<<<<<<<<<<<<<<<<<
                    //text.text = childSnapshot.ToString();

                }
            }
        }
    }

    public void GetUserInfo()
    {
        StartCoroutine(GetName((string name) => {

            Debug.Log(name + " USERNAME");
        }));
        StartCoroutine(FriendList());
    }
}

