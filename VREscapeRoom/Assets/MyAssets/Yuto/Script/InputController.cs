using System.Collections;
using System.Collections.Generic;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class InputController : InputSystemGlobalListener, IMixedRealityInputHandler<Vector2>, IMixedRealityInputHandler
{
    public MixedRealityInputAction locomotion;
    public MixedRealityInputAction menu;
    public MixedRealityInputAction select;
    public MixedRealityInputAction grabL;
    public MixedRealityInputAction grabR;
    public MixedRealityInputAction switchL;
    public MixedRealityInputAction switchR;

    //public Locomotion controller;
    public InventoryController inventory;

    public float deadZone = .4f;

    public void OnInputChanged(InputEventData<Vector2> eventData)
    {
        //if (eventData.MixedRealityInputAction == locomotion)
        //{
        //    Debug.Log("InputEventData = " + eventData.InputData);

        //    float x = eventData.InputData.x;
        //    float z = eventData.InputData.y;
        //    if (x < deadZone && x > -deadZone)
        //    {
        //        x = 0;
        //    }
        //    if (z < deadZone && z > -deadZone)
        //    {
        //        z = 0;
        //    }
        //    controller.x = x;
        //    controller.z = z;
        //    Debug.Log("controller.x = " + controller.x + " and controller.z = " + controller.z);
        //}
    }

    //public void OnInputChanged(InputEventData<float> eventData)
    //{
    //    if (eventData.MixedRealityInputAction == grabL)
    //    {
    //        UnityEngine.Debug.Log("Grab L");
    //        if (eventData.InputData == 1f)
    //        {
    //            inventory.grabItem("L");
    //        }

    //    }
    //    if (eventData.MixedRealityInputAction == grabR)
    //    {
    //        UnityEngine.Debug.Log("Grab R");
    //        if (eventData.InputData == 1f)
    //        {
    //            inventory.grabItem("R");
    //        }
    //    }
    //}

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == menu)
        {
            Debug.Log("called Menu");
        }

        if (eventData.MixedRealityInputAction == select)
        {
            Debug.Log("called Select");
        }

        if (eventData.MixedRealityInputAction == grabL)
        {
            UnityEngine.Debug.Log("Grab L");
            inventory.grabItem("L");

        }
        if (eventData.MixedRealityInputAction == grabR)
        {
            UnityEngine.Debug.Log("Grab R");
            inventory.grabItem("R");
        }
        if (eventData.MixedRealityInputAction == switchL)
        {
            UnityEngine.Debug.Log("Switch L");
            inventory.switchItem("L");
        }
        if (eventData.MixedRealityInputAction == switchR)
        {
            UnityEngine.Debug.Log("Switch R");
            inventory.switchItem("R");
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
    }
}