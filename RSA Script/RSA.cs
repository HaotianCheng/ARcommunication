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
            // dafault RSA 1024 key can encrypt 117 bytes at most.
            // with a 2048 RSA key, you can encrypt 245 bytes, the byte array for signature had 128 bytes.
            // therefore we use 2048 RSA key for encryption/decryption. and default RSA for sign/verify
            rsa_encrypt = new RSACryptoServiceProvider(2048);
            rsa = new RSACryptoServiceProvider();
            myAscii = new ASCIIEncoding();
            message = "Hello world, I'm Alex";

            // Get the public keyy   
            string publickey = rsa.ToXmlString(false); // false to get the public key   
            string privatekey = rsa.ToXmlString(true); // true to get the private key

            string pub2048 = rsa_encrypt.ToXmlString(false);
            string pri2048 = rsa_encrypt.ToXmlString(true);

            // Create Signature
            Signedstring = CreateSignature(message, privatekey); // A signed a signature on message
            Console.WriteLine("signature: {0}", Signedstring);

            // Call the encryption method
            encryptedData = EncryptText(pub2048, Signedstring); // A encrypt cipher with B's public key 
            // Call the decryption methon
            decryptedData = DecryptData(pri2048, encryptedData); // B decrypt the cupher with own private key

            if (decryptedData == Signedstring && VerifySignature(message, Signedstring, publickey))
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