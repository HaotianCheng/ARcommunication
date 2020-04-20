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


public class Storage: MonoBehaviour
{
    public Login login;
    public string storedHashedPassword;
    public string inputHashedPassword;
    DatabaseReference reference;
    public Text Data;
    bool isCalledback; 
    [SerializeField] public Dictionary<string, string> dictionary;

    public void Start()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("Your firebase URL");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        storedHashedPassword = "";
        dictionary = new Dictionary<string, string>();
    }

    public void Update()
    {
        if (storedHashedPassword != "" && !isCalledback)
        {
            storedcallback();
            isCalledback = true;
        }
    }

    public Storage(Dictionary<string, string> dictionary)
    {
        this.dictionary = dictionary;
    }

    // User create account
    public int createAccount(string username, string password) // username and password obtain from textbox
    {
        if (!this.dictionary.ContainsKey(username))
        {
            string hashOutput = hashpassword(password);
            this.dictionary.Add(username, hashOutput);
            reference.Child("Users").Child(username).Child("Password").SetValueAsync(hashOutput);
            return 0;
        }
        else
        {
            // Console.WriteLine("Username is taken, please choose another one"); // error display
            return -1;
        }
    }

    // User sign in
    public void SignIn(string username, string password) // username and password obtain from textbox
    {
        inputHashedPassword = hashpassword(password);
        FirebaseDatabase.DefaultInstance.GetReference("Users").GetValueAsync().ContinueWith
        (task =>
            {
                DataSnapshot snapshot = task.Result;
                storedHashedPassword = snapshot.Child(username).Child("Password").Value.ToString();
                Data.text = storedHashedPassword;
            }
        );
    }

    // hashing method
    public string hashpassword(string input)
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

    public void storedcallback()
    {
        Debug.LogWarning("callback: " + storedHashedPassword);
        Debug.LogWarning("callback: " + inputHashedPassword);
        if(storedHashedPassword == inputHashedPassword)
        {
            login.onLoginCallback(1);
        }
        else
        {
            login.onLoginCallback(0);
        }
    }

    //static void Main(string[] args)
    //{
    //    Dictionary<string, string> testing = new Dictionary<string, string>();
    //    Storage test = new Storage("alex", testing);
    //    // create account
    //    string username = "alexlin";
    //    string password = "alexalex";
    //    test.createAccount(username, password);
    //    test.createAccount("alexlin", password); // same username
    //    // sign in
    //    test.SignIn(username, password);
    //    test.SignIn(username, "wrongpassword");
    //}
}

