using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualHandle : MonoBehaviour
{
    public Text visualDisplay;
    public GameObject palmCenter;
    public Text log;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GestureInfo gestureInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        TrackingInfo trackingInfo = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        Warning warning = ManomotionManager.Instance.Hand_infos[0].hand_info.warning;
        Session session = ManomotionManager.Instance.Manomotion_Session;

        visualDisplay.text = "";

        DisplayContinuousGestures(gestureInfo.mano_gesture_continuous);
        DisplayManoclass(gestureInfo.mano_class);
        DisplayTriggerGesture(gestureInfo.mano_gesture_trigger, trackingInfo);
        DisplayHandState(gestureInfo.state);
        DisplayPalmCenter(trackingInfo.palm_center, gestureInfo, warning);
        DisplayHandSide(gestureInfo.hand_side);
        DisplayDepthEstimation(trackingInfo.depth_estimation);

    }

    private void DisplayDepthEstimation(float depth_estimation)
    {
        visualDisplay.text += depth_estimation.ToString("F2") + "\n";
    }

    private void DisplayHandSide(HandSide hand_side)
    {
        switch (hand_side)
        {
            case HandSide.Palmside:
                visualDisplay.text += "Handside: Palm Side\n";
                break;
            case HandSide.Backside:
                visualDisplay.text += "Handside: Back Side\n";
                break;
            case HandSide.None:
                visualDisplay.text += "Handside: None\n";
                break;
            default:
                visualDisplay.text += "Handside: None\n";
                break;
        }
    }

    private void DisplayPalmCenter(Vector3 palm_center, GestureInfo gestureInfo, Warning warning)
    {
        if (warning != Warning.WARNING_HAND_NOT_FOUND)
        {
            if (!palmCenter.activeSelf)
            {
                palmCenter.SetActive(true);
            }
            float smoothing = 1 - ManomotionManager.Instance.Manomotion_Session.smoothing_controller;

            palmCenter.transform.position = Camera.main.ViewportToScreenPoint(palm_center);
            float newFillAmmount = 1 - gestureInfo.state / 6 * 0.25f;
            palmCenter.transform.localScale = Vector3.Lerp(palmCenter.transform.localScale, Vector3.one * newFillAmmount, 0.9f);
        }
        else
        {
            if (palmCenter.activeSelf)
            {
                palmCenter.SetActive(false);
            }
        }
    }

    private void DisplayHandState(int state)
    {
        visualDisplay.text += "HandState: " + state + "\n";
    }

    private void DisplayTriggerGesture(ManoGestureTrigger triggerGesture, TrackingInfo trackingInfo)
    {
        switch (triggerGesture)
        {
            case ManoGestureTrigger.NO_GESTURE:
                visualDisplay.text += "ManoGestureTrigger: NO_GESTURE\n";
                break;
            case ManoGestureTrigger.CLICK:
                visualDisplay.text += "ManoGestureTrigger: CLICK\n";
                break;
            case ManoGestureTrigger.DROP:
                visualDisplay.text += "ManoGestureTrigger: DROP\n";
                break;
            case ManoGestureTrigger.GRAB_GESTURE:
                visualDisplay.text += "ManoGestureTrigger: GRAB_GESTURE\n";
                break;
            case ManoGestureTrigger.PICK:
                visualDisplay.text += "ManoGestureTrigger: PICK\n";
                break;
            case ManoGestureTrigger.RELEASE_GESTURE:
                visualDisplay.text += "ManoGestureTrigger: RELEASE_GESTURE\n";
                break;
            default:
                visualDisplay.text += "ManoGestureTrigger: \n";
                break;
        }
    }

    private void DisplayManoclass(ManoClass mano_class)
    {
        switch (mano_class)
        {
            case ManoClass.NO_HAND:
                visualDisplay.text += "Manoclass: No Hand\n";
                break;
            case ManoClass.GRAB_GESTURE_FAMILY:
                visualDisplay.text += "Manoclass: Grab Class\n";
                break;
            case ManoClass.PINCH_GESTURE_FAMILY:
                visualDisplay.text += "Manoclass: Pinch Class\n";
                break;
            case ManoClass.POINTER_GESTURE_FAMILY:
                visualDisplay.text += "Manoclass: Pointer Class\n";
                break;
            default:
                visualDisplay.text += "Manoclass: \n";
                break;
        }
    }

    private void DisplayContinuousGestures(ManoGestureContinuous mano_gesture_continuous)
    {

        switch (mano_gesture_continuous)
        {
            case ManoGestureContinuous.CLOSED_HAND_GESTURE:
                visualDisplay.text += "Continuous: Closed Hand\n";
                break;
            case ManoGestureContinuous.OPEN_HAND_GESTURE:
                visualDisplay.text += "Continuous: Open Hand\n";
                break;
            case ManoGestureContinuous.HOLD_GESTURE:
                visualDisplay.text += "Continuous: Hold\n";
                break;
            case ManoGestureContinuous.OPEN_PINCH_GESTURE:
                visualDisplay.text += "Continuous: Open Pinch\n";
                break;
            case ManoGestureContinuous.POINTER_GESTURE:
                visualDisplay.text += "Continuous: Pointing\n";
                break;
            case ManoGestureContinuous.NO_GESTURE:
                visualDisplay.text += "Continuous: None\n";
                break;
            default:
                visualDisplay.text += "Continuous: None\n";
                break;
        }
        
    }
}
