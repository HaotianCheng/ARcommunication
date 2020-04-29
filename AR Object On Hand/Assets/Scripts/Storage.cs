using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class Storage : MonoBehaviour
{
    public Login login;
    public string userName;
    public string storedHashedPassword;
    public string inputHashedPassword;
    DatabaseReference reference;
    

    public void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://hideandseek-42a89.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        FirebaseDatabase.DefaultInstance.GetReference("Users").ChildChanged += HandleChildAdded;
    }


    // User create account
    public async Task<int> CreateAccount(string username, string password) // username and password obtain from textbox
    {
            string hashOutput = Hashpassword(password);
            // Debug.Log(reference.Child("Users").Child(username));
            int result = await reference.Child("Users").Child(username).GetValueAsync().ContinueWith(task => 
            {               
                if (task.IsFaulted)
                {
                    Debug.Log("error");
                    return 0;
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    // Do something with snapshot...
                    if (snapshot == null)
                    {
                        Debug.Log("snapshot is null");
                        return 0;
                    }
                    else
                    {
                        if (snapshot.HasChild("Password"))
                        {
                            Debug.Log("Account exist");
                            return 0;
                        }
                        else
                        {
                            Debug.Log("Account not exist");
                            reference.Child("Users").Child(username).Child("Password").SetValueAsync(hashOutput);
                            userName = username;
                            return 1;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            });
        return result;
    }

    // User sign in
    public async Task<int> SignIn(string username, string password) // username and password obtain from textbox
    {
        inputHashedPassword = Hashpassword(password);
        int result = await reference.Child("Users").Child(username).GetValueAsync().ContinueWith
        (task =>
        {
            DataSnapshot snapshot = task.Result;
            if (task.IsCompleted && snapshot.HasChild("Password"))
            {
                Debug.Log("Account exist");
                storedHashedPassword = snapshot.Child("Password").Value.ToString();
                
                if (storedHashedPassword == inputHashedPassword) 
                {
                    userName = username;
                    return 1; 
                }
                else{ return 0; }
            }
            else
            {
                Debug.Log("Account not exist");
                return -1;
            }           
        }
        );
        return result;
    }

    // hashing method
    public string Hashpassword(string input)
    {
        using (MD5 md5Hash = MD5.Create())
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i].ToString("x2"));
            }
            string hash = sb.ToString();
            Console.WriteLine("The MD5 hash of " + input + " is: " + hash + ".");
            return hash;
        }
    }

    public async void CreateAndUploadPublicKey() // username and password obtain from textbox
    {
        // Debug.Log(reference.Child("Users").Child(username));
        string xmlString = Security.CreateKeyPair();
        Debug.Log("upload " + userName + " \n" + xmlString);
        await reference.Child("Users").Child(userName).Child("PublicKey").SetValueAsync(xmlString);
    }

    public async Task<string> GetXmlFromUser(string username)
    {
        //UpdateReference();
        string xmlString = await FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith
        (task =>
        {
            DataSnapshot snapshot = task.Result;
            string storedPublicKey = snapshot.Child(username).Child("PublicKey").Value.ToString();
            return storedPublicKey;
        }
        );
        Debug.Log(xmlString);
        return xmlString;
    }

    void HandleChildAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot
        Debug.Log("childchange");
    }

}

