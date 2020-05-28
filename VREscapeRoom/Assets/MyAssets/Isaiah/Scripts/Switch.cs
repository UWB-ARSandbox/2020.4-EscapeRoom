using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used as a switch that hooks up to a door and is connected to
 * a pressure plate
 * (Right now this is redendunt need to abstract the Switch and door to get
 *      better use out of the switch)
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class Switch : MonoBehaviour
{
    //If on this frame
    public bool on = false;
    //If on previous frame
    private bool prev = false;
    //Door to open
    [SerializeField] private Door[] doors;

    private void Update()
    {
        //only update the door is there is a change with the on bool
        if(on != prev)
        {
            prev = on;
            foreach(Door door in doors)
            {
                if (on)
                    door.open();
                else
                    door.close();
            }
        }
    }
}
