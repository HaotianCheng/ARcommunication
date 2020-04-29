using System.Collections;
using System.Collections.Generic;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RSA_TEST
{
    class RSA
    {
        static RSACryptoServiceProvider rsa;
        static RSACryptoServiceProvider rsa_encrypt;
        static ASCIIEncoding myAscii;
        static byte[] dataToEncrypt;
        static byte[] encryptedData;
        static string decryptedData;
        static string Signedstring;
        static string message;

        static void Main()
        {
            // this script is mainly to simulate RSA double layer security by using key pairs to securely complete communication
            // between User A and B. Also applying Digital Signatature concept to authenticate the identities of sender and 
            // receiver. Therefore to prevent Man in the middle attack in our project.
            rsa_encrypt = new RSACryptoServiceProvider();
            rsa = new RSACryptoServiceProvider();
            myAscii = new ASCIIEncoding();
            message = "Hello world, I'm Alex";

            // Get the public keyy   
            string publickey = rsa.ToXmlString(false); // public key to verify signature   
            string privatekey = rsa.ToXmlString(true); // private key sign signature

            string pub_encrypt = rsa_encrypt.ToXmlString(false); // public key to encrypt
            string pri_decrypt = rsa_encrypt.ToXmlString(true); // private key to decrypt

            // Create Signature
            Signedstring = CreateSignature(message, privatekey); // A signed a signature on message
            // Call the encryption method
            encryptedData = EncryptText(pub_encrypt, message); // A encrypt message with B's public key 
            // Call the decryption methon
            decryptedData = DecryptData(pri_decrypt, encryptedData); // B decrypt the cipher with own private key
            
            // check if the message is decrypted correctly & 
            // B verify with A's publickey to authenticate A is the real originator
            if (decryptedData == message && VerifySignature(message, Signedstring, publickey)) 
            {
                Console.WriteLine("successfully verfied, message: {0}", message);
            }
        }

        // Create a method to encrypt a text using a RSA algorithm public key   
        static byte[] EncryptText(string pubkey, string signedmessage)
        {
            dataToEncrypt = myAscii.GetBytes(signedmessage);
            rsa_encrypt.FromXmlString(pubkey); 
            return rsa_encrypt.Encrypt(dataToEncrypt, false);
        }

        // Method to decrypt the data using a RSA algorithm private key   
        static string DecryptData(string prikey, byte[] encryptedData)
        {
            byte[] dataToDecrypt = encryptedData;
            rsa_encrypt.FromXmlString(prikey);
            return myAscii.GetString(rsa_encrypt.Decrypt(dataToDecrypt, false));
        }

        public static string CreateSignature(string message, string privateKey)
        {
            // setup the rsa from the private key:
            rsa.FromXmlString(privateKey);
            byte[] originalData = myAscii.GetBytes(message);
            byte[] signedBytes = rsa.SignData(originalData, new SHA1CryptoServiceProvider());
            // note here we cannot use ASCIIEndoing on encoded message because it contains
            // invalid ASCII characters. So store encoded message in a base64 string.
            return Convert.ToBase64String(signedBytes);
        }

        public static bool VerifySignature(string message, string signature, string publicKey)
        {
            // setup the rsa from the public key:
            rsa.FromXmlString(publicKey);
            byte[] originalData = myAscii.GetBytes(message);
            byte[] signatureData = Convert.FromBase64String(signature);
            // verify that the license-terms match the signature data
            if (rsa.VerifyData(originalData, new SHA1CryptoServiceProvider(), signatureData) == false) return false;
            else return true;
        }
    }
}
