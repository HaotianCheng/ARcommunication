using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject characterPrefab;
    public GameObject pointer;

    private ARRaycastManager raycastManager;
    private ARSessionOrigin arOrigin;
    private Pose pose;
    private bool isHit;

    public Text log;
    public Text log2;
    

    // Start is called before the first frame update
    void Start()
    {
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Update is called once per frame
    public void Update()
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
            log.text = cameraForward.ToString();
            pointer.transform.rotation = Quaternion.LookRotation(new Vector3(cameraForward.x, 0, cameraForward.z), Vector3.up);
        }
        else
        {
            pointer.SetActive(false);
        }
    }

    public void UpdatePose()
    {
        //Debug.Log("PoseUpdate: " + Camera.current != null + " " + isHit);
        if (Camera.current != null)
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
            var hits = new List<ARRaycastHit>();
            raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
            //Debug.Log("PoseHitCount: " + hits.Count.ToString());           
            isHit = hits.Count > 0;
            if (isHit)
            {
                pose = hits[0].pose;
                //Debug.Log("Pose: " + pose.ToString());
            }
        }
        

    }

    public void OnLogButton()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        bool main = Camera.main != null;
        bool cur = Camera.current != null;
        Debug.Log(main + " " + cur);
        Debug.Log("A: " + raycastManager.Raycast(screenCenter, hits, TrackableType.Planes) + " HIT " + hits.Count + " " + hits[0].pose);
        Debug.Log("B: " + raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon) + " HIT " + hits.Count + " " + hits[0].pose);
        isHit = hits.Count > 0;
        if (isHit)
        {
            log2.text = hits[0].pose.position.ToString();
            pose = hits[0].pose;
        }
    }

}
