using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{
    public string username;
   
    public string coin;
    public string[] FriendLists;


    public User(string Name, string Coin, string[] frdlist)
    {
        this.username = Name;
       
        this.coin = Coin;
        this.FriendLists = frdlist;
    }
   
}
