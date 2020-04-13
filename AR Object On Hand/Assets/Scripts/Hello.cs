using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hello : MonoBehaviour
{
    public GameObject pointer;
    public GameObject t;

    public void Update()
    {
        Debug.Log("update");
        pointer.transform.rotation.SetLookRotation(t.transform.forward, Vector3.up);
        // pointer.transform.rotation = Quaternion.LookRotation(t.transform.forward, Vector3.up);
    }



}
