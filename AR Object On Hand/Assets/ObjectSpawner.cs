using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject pointer;

    private ARRaycastManager raycastManager;
    private ARSessionOrigin arOrigin;
    private Pose pose;
    private bool isHit;
    

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePose();
        UpdatePointer();

        if (isHit && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Instantiate(characterPrefab, pointer.transform.position, pointer.transform.rotation);
        }

    }

    private void UpdatePointer()
    {
        if (isHit)
        {
            pointer.SetActive(true);
            pointer.transform.position = pose.position;

            Vector3 cameraForward = Camera.current.transform.forward;
            pointer.transform.rotation.SetLookRotation(new Vector3(cameraForward.x, 0, cameraForward.z));
        }
        else
        {
            pointer.SetActive(false);
        }
    }

    public void UpdatePose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        raycastManager.Raycast(screenCenter, hits,  TrackableType.Planes);

        isHit = hits.Count > 0;
        if (isHit)
        {
            pose = hits[0].pose;
        }

    }

}
