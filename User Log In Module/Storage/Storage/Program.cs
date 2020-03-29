using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Storage
{
    class Storage
    {
        string storageName;
        Dictionary<string, string> dictionary;

        public Storage(string storageName, Dictionary<string, string> dictionary)
        {
            this.storageName = storageName;
            this.dictionary = dictionary;
        }

        // User create account
        public void createAccount(string username, string password) // username and password obtain from textbox
        {
            if (!this.dictionary.ContainsKey(username))
            {
                string hashOutput = hashpassword(password);
                this.dictionary.Add(username, hashOutput);
            }
            else Console.WriteLine("Username is taken, please choose another one"); // error display
        }

        // User sign in
        public void SignIn(string username, string password) // username and password obtain from textbox
        {
            if (this.dictionary.ContainsKey(username))
            {
                string storedHashedPassword = this.dictionary[username];
                string inputHashedPassword = hashpassword(password);
                if (storedHashedPassword == inputHashedPassword)
                {
                    Console.WriteLine("Log in successfully"); // replace with "redirecting to app page"
                }
                else Console.WriteLine("Username or password is incorrect, please try again"); // error display
            }
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
        static void Main(string[] args)
        {
            Dictionary<string, string> testing = new Dictionary<string, string>();
            Storage test = new Storage("alex", testing);
            // create account
            string username = "alexlin";
            string password = "alexalex";
            test.createAccount(username, password);
            test.createAccount("alexlin", password); // same username
            // sign in
            test.SignIn(username, password);
            test.SignIn(username, "wrongpassword");
        }
    }
}
