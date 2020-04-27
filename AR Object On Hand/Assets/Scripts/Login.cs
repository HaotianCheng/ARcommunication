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
    public GameObject roomPanel;
    public GameObject lobbyPanel;
    public GameObject loginPanel;
    public Text log;

    public async void OnCreateButton()
    {
        int result = await storage.CreateAccount(UserName.text, Password.text);
        if (result == 0)
        {
            log.text = "Username is taken, please choose another one";
            Debug.LogWarning("Username is taken, please choose another one");
        }
        else
        {
            Debug.Log("Account created");
            SetScreen(lobbyPanel);
        }
    }

    public async void OnSignInButton()
    {
        int result = await storage.SignIn(UserName.text, Password.text);
        if (result == -1)
        {
            log.text = "Account not exist";
            Debug.LogWarning("Account not exist");
        }
        else if (result == 0)
        {
            log.text = "Incorrect password";
            Debug.LogWarning("Incorrect password");
        }
        else
        {
            log.text = "Login Successful";
            Debug.Log("Login Successful");
            SetScreen(lobbyPanel);
        }
    }

    public void SetScreen(GameObject screen)
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(false);
        loginPanel.SetActive(false);
        screen.SetActive(true);
    }

}
