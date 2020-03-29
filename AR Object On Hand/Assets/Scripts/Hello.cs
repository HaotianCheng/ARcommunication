using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hello : MonoBehaviour
{
    public Text h;

    public void PrintInConsole()
    {
        Debug.Log(Hashing(h.text));
    }

    public string Hashing(string plainText)
    {
        return plainText.ToUpper();
    }



}
