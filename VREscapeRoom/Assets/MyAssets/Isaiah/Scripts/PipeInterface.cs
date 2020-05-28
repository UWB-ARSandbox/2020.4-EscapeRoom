using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is used as an interface to unlock a door when
 * all inputs (Pipes) are turned on.
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class PipeInterface : MonoBehaviour
{
    //List of pipes that are used to check if the door needs to be opened
    [SerializeField] private List<Pipe> inputs;
    //The door to open
    [SerializeField] private Door door;
    private bool prev;
    private bool on;
    // Start is called before the first frame update
    void Start()
    {
        //If the door is not setup then send a warning
        if(!door)
        {
            Debug.LogWarning("Door not setup in Pipe Interface!");
        }
    }

    public bool isOn()
    {
        return on;
    }

    // Update is called once per frame
    void Update()
    {
        bool isOn = true;
        //If any of the pipes are off then don't set the door to on
        for(int i = 0; i < inputs.Count; i++)
        {
            if(!inputs[i].isOn())
            {
                isOn = false;
            }
        }
        on = isOn;
        //Only toggle the door open/closed if the prev frame on is different than this frames
        if(prev != on)
        {
            prev = on;
            if (on && door)
                door.open();
            else if (!on && door)
                door.close();
        }
    }
}
