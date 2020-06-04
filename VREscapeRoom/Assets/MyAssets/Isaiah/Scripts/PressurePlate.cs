using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is a pressure plate that hooks up to a switch
 * (The switch is not really needed as of right now, Need to abstract classes more)
 * The pressure plate will turn on when a gameobject with a rigidbody
 * and a collider (not trigger) collides with it
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class PressurePlate : MonoBehaviour
{
    [SerializeField] private Switch trigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Players")
        {
            trigger.on = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Players")
        {
            trigger.on = false;
        }
    }

    //Needed incase two objects are colliding and one exits, the trigger
    //will stay on
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Players")
        {
            trigger.on = true;
        }
    }
}
