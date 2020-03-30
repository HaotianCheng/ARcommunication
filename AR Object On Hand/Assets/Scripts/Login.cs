using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Login : MonoBehaviour
{
    public InputField playerName;
    public InputField password;
    public Storage storage;
    public GameObject roomPanel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            foreach (string key in storage.dictionary.Keys)
            {
                Debug.Log(key + ": " + storage.dictionary[key]);
            }
        }
    }

    public void OnCreateButton()
    {
        int result = storage.createAccount(playerName.text, password.text);
        if (result == -1)
        {
            Debug.LogWarning("Username is taken, please choose another one");
        }
        else
        {
            Debug.Log("Account created");
        }


    }

    public void OnSignInButton()
    {
        int result = storage.SignIn(playerName.text, password.text);
        if (result == 0)
        {
            Debug.LogWarning("Account not exist");
        }
        else if (result == -1)
        {
            Debug.LogWarning("Incorrect password");
        }
        else
        {
            Debug.Log("Login Successful");
            roomPanel.SetActive(true);
            gameObject.SetActive(false);
        }
    }

}
