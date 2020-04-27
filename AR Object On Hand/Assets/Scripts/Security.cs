using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public class Security : MonoBehaviour
{
    private static RSACryptoServiceProvider myRSA;

    public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
    {
        try
        {
            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {

                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException e)
        {
            // Console.WriteLine(e.Message);
            Debug.Log(e.Message);
            return null;
        }
    }

    public static byte[] RSADecrypt(byte[] DataToDecrypt, bool DoOAEPPadding)
    {
        try
        {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider.

            decryptedData = myRSA.Decrypt(DataToDecrypt, DoOAEPPadding);

            return decryptedData;
        }
        //Catch and display a CryptographicException  
        //to the console.
        catch (CryptographicException e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }

    public static byte[] HashAndSignBytes(byte[] DataToSign)
    {
        try
        {
            // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
            // to specify the use of SHA1 for hashing.
            return myRSA.SignData(DataToSign, new SHA1CryptoServiceProvider());
        }
        catch (CryptographicException e)
        {
            Debug.Log(e.Message);

            return null;
        }
    }

    public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
    {
        try
        {
            // Create a new instance of RSACryptoServiceProvider using the
            // key from RSAParameters.
            RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

            RSAalg.ImportParameters(Key);

            // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
            // to specify the use of SHA1 for hashing.
            return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);
        }
        catch (CryptographicException e)
        {
            Debug.Log(e.Message);

            return false;
        }
    }

    public static string CreateKeyPair() // username and password obtain from textbox
    {
        myRSA = new RSACryptoServiceProvider();
        return myRSA.ToXmlString(false);

    }



}
