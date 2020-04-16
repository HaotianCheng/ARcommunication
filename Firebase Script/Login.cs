using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class Login : MonoBehaviour
{
    public InputField UserName;
    public InputField Password;
    public Storage storage;
    public GameObject RoomPanel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("t"))
        //{
        //    foreach (string key in storage.dictionary.Keys)
        //    {
        //        Debug.Log(key + ": " + storage.dictionary[key]);
        //    }
        //}
    }

    public void OnCreateButton()
    {
        int result = storage.createAccount(UserName.text, Password.text);
        if (result == -1)
        {
            Debug.LogWarning("Username is taken, please choose another one");
        }
        else
        {
            Debug.Log("Account created");
        }


    }
    public void onLoginCallback(int res)
    {
        if(res == -1)
        {
            Debug.LogWarning("Account not exist");
        }
        else if(res == 0)
        {
            Debug.LogWarning("Incorrect password");
        }
        else
        {
            Debug.Log("Login Successful");
            RoomPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OnSignInButton()
    {
        storage.SignIn(UserName.text, Password.text);
    }

}
