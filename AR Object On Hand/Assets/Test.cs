using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Test : MonoBehaviourPunCallbacks
{
    public Text text;

    public void Update()
    {
        if (text.text == "")
        {
            text.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            text.transform.parent.gameObject.SetActive(true);
        }
    }

}
