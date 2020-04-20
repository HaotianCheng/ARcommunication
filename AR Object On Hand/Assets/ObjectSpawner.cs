using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ObjectSpawner : MonoBehaviour
{
    public VisualHandle visualHandle;
    public GameObject character;
    public GameObject characterPrefab;
    public GameObject pointer;

    private ARRaycastManager raycastManager;
    private ARSessionOrigin arOrigin;
    private Pose pose;
    private bool isHit;

    public Text log;
    public Text log2;

    public float depthMultiplyer = 3;
    public Vector3 velocity = Vector3.zero;
    public float backSideTolerrence = 0.2f;
    public float backSideTime;


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

        /*
        if (isHit && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            GameObject gm = Instantiate(characterPrefab, pointer.transform.position, pointer.transform.rotation);
            gm.transform.eulerAngles = gm.transform.eulerAngles + Vector3.up * 180;
        }*/
        if (visualHandle.warning != Warning.WARNING_HAND_NOT_FOUND && visualHandle.gestureInfo.mano_gesture_continuous == ManoGestureContinuous.OPEN_HAND_GESTURE)
        {
            if (visualHandle.gestureInfo.hand_side == HandSide.Palmside)
            {
                backSideTime = 0;
                if (!character.activeSelf) { character.SetActive(true); }

                Vector3 cameraForward = Camera.current.transform.forward;
                Vector3 palmCenter = Camera.main.ViewportToScreenPoint(visualHandle.trackingInfo.palm_center);
                Ray ray = Camera.main.ScreenPointToRay(palmCenter);
                Vector3 targetPos = ray.origin + ray.direction * visualHandle.trackingInfo.depth_estimation * depthMultiplyer;

                character.transform.position = Vector3.SmoothDamp(character.transform.position, targetPos, ref velocity, 0.1f);
                character.transform.rotation = Quaternion.LookRotation(new Vector3(-cameraForward.x, 0, -cameraForward.z), Vector3.up);
            }
            else if (visualHandle.gestureInfo.hand_side == HandSide.Backside)
            {
                backSideTime += Time.deltaTime;
                if (backSideTime > backSideTolerrence)
                {
                    if (character.activeSelf) { character.SetActive(false); }
                }
            }
            

        }
        else
        {
            if (character.activeSelf) { character.SetActive(false); }
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

    public void SetDepthMutiplyer(float value)
    {
        depthMultiplyer = value;
    }

    public void SetSmoothingValue(float value)
    {
        visualHandle.session.smoothing_controller = value;
    }

}
