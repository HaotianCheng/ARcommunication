using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using System.Security.Cryptography;

public class Crypto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        String plaintext = "I'm Alex";
        RijndaelManaged crypto = new RijndaelManaged();
        String Key = crypto.GenerateKey();
        String IV = crypto.GenerateIV();
    }
    public static byte[] Encrypt(string plaintext, byte[] Key, byte[] IV)
    {
        RijndaelManaged rij = new RijndaelManaged();
        rij.Key = Key;
        rij.IV = IV;

        //base case
        if (plaintext.Length <= 0) Console.WriteLine("text not found");
        if (Key == null || Key.Length <= 0) Console.WriteLine("Key not found");
        if (IV == null || IV.Length <= 0) Console.WriteLine("IV not found");

        ICryptoTransform encryptor = rij.CreateEncryptor(rij.Key, rij.IV);
        byte[] encrypted;
        String clearText;
        // create streams for encryption
        using (MemoryStream ms = new MemoryStream())
        {
            using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    //Write all data to the stream.
                    sw.Write(plaintext);
                }
                encrypted = ms.ToArray();
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        Console.WriteLine(clearText); // debugging purpose
        return encrypted;
    }

    public static string Decrypt(byte[] ciphertext, byte[] Key, byte[] IV)
    {
        RijndaelManaged rij = new RijndaelManaged();
        rij.Key = Key;
        rij.IV = IV;

        //base case
        if (ciphertext.Length <= 0) Console.WriteLine("ciphertext not found");
        if (Key == null || Key.Length <= 0) Console.WriteLine("Key not found");
        if (IV == null || IV.Length <= 0) Console.WriteLine("IV not found");

        ICryptoTransform decryptor = rij.CreateDecryptor(rij.Key, rij.IV);
        // create streams for decryption
        MemoryStream ms = new MemoryStream(ciphertext);
        CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cs);

        String plaintext = sr.ReadToEnd();
        return plaintext;
    }
    
    static void Main(string[] args)
    {
        String plaintext = "I'm Alex, here shows the message decryped correctly";
        RijndaelManaged crypto = new RijndaelManaged();
        crypto.GenerateKey(); // obtain by cryto.Key
        crypto.GenerateIV(); // obtain by cryto.IV

        Console.WriteLine(plaintext);
        byte[] encryptedMessage = Encrypt(plaintext, crypto.Key, crypto.IV);

        String decryptedMessage = Decrypt(encryptedMessage, crypto.Key, crypto.IV);
        Console.WriteLine(decryptedMessage);
    }
}
