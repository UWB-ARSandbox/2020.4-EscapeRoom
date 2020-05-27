using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

/*
 * This class is a Door that can either be a sliding door, or a hinged door. The 
 * door is ASL compatible. The door can be opened a max distance or angle.
 * Requires an outside class to open and close the door using the open and close
 * methods.
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */

public class Door : MonoBehaviour
{
    //If the door is a sliding door or a hinged door
    [SerializeField] private bool slidingDoor = false;
    //If the door moves up/down or rotates left/right
    [SerializeField] public bool invertDir = false;
    //angle that the door will be opened at. Distance that it moves if sliding
    [SerializeField] public float angleDistance = 90.0f;
    //How fast the door moves
    [SerializeField] public float speed = 50.0f;
    //if the door is currently open
    private bool opened = false;
    //the state of opened the previous frame
    private bool prev = false;
    //the asl object that syncs the door for all players
    private ASLObject asl = null;
    //current delta angle/height of the door
    private float angle = 0.0f;
    //starting position of the door
    private Vector3 startPos;

    void Start()
    {
        //Init start position and set callback function
        startPos = transform.position;
        asl = gameObject.GetComponent<ASLObject>();
        asl._LocallySetFloatCallback(floatCallback);
    }
    
    /*
     * Called everytime any door uses a asl.SendFloatArray
     * sets the opened state of the door
     */
    private void floatCallback(string id, float[] f)
    {
        //if f is 0 then the door is not open
        if(f[0] == 0)
        {
            opened = false;
        }
        //if f is 1 then the door is open
        else
        {
            opened = true;
        }
    }

    /*
     * returns an array with a single float where 0 is a closed door
     * and 1 is an open door
     */
    private float[] setCallbackArray()
    {
        if(opened)
        {
            return new float[] { 1 };
        }
        return new float[] { 0 };
    }

    /*
     * Called every update
     * Determins if a asl.sendFloatArray is appropriate
     */
    private void checkToSend()
    {
        //If opened is not the same as prev then sendFloatArray
        if(opened != prev)
        {
            prev = opened;
            asl.SendAndSetClaim(send, 20, true);
        }
    }

    private void send()
    {
        asl.SendFloatArray(setCallbackArray());
    }

    // Update is called once per frame
    void Update()
    {
        //check if a sendfloat is needed
        checkToSend();
        //If the direction is inverted
        if (invertDir)
        {
            if (slidingDoor)
            {
                //If the door is open and the angle is not greater than maxDistance
                //Increment distance and set transform
                if (opened && angle < angleDistance)
                {
                    angle += speed * Time.deltaTime;
                    transform.Translate(0, -speed * Time.deltaTime, 0);
                    if (angle > angleDistance)
                    {
                        angle = angleDistance;
                        transform.position = new Vector3(startPos.x, startPos.y - angleDistance, startPos.z);
                    }
                }
                //If the door is closed and the door has not finished closing
                //Decrement and set transform
                if (!opened && angle > 0)
                {
                    angle -= speed * Time.deltaTime;
                    transform.Translate(0, speed * Time.deltaTime, 0);
                    if (angle < 0)
                    {
                        angle = 0;
                        transform.position = startPos;
                    }
                }
            }
            else
            {
                //If the door is open and the angle is not greater than maxDistance
                //Increment distance and set transform
                if (opened && angle < angleDistance)
                {
                    angle += speed * Time.deltaTime;
                    transform.Rotate(Vector3.up, -speed * Time.deltaTime);
                    if (angle > angleDistance)
                    {
                        angle = angleDistance;
                        transform.rotation = Quaternion.Euler(0, -angleDistance, 0);
                    }
                }

                //If the door is closed and the door has not finished closing
                //Decrement and set transform
                if (!opened && angle > 0)
                {
                    angle -= speed * Time.deltaTime;
                    transform.Rotate(Vector3.up, speed * Time.deltaTime);
                    if (angle < 0)
                    {
                        angle = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
        }
        else 
        {
            if (slidingDoor)
            {
                //If the door is open and the angle is not greater than maxDistance
                //Increment distance and set transform
                if (opened && angle < angleDistance)
                {
                    angle += speed * Time.deltaTime;
                    transform.Translate(0, speed * Time.deltaTime, 0);
                    if (angle > angleDistance)
                    {
                        angle = angleDistance;
                        transform.position = new Vector3(startPos.x, startPos.y + angleDistance, startPos.z);
                    }
                }

                //If the door is closed and the door has not finished closing
                //Decrement and set transform
                if (!opened && angle > 0)
                {
                    angle -= speed * Time.deltaTime;
                    transform.Translate(0, -speed * Time.deltaTime, 0);
                    if (angle < 0)
                    {
                        angle = 0;
                        transform.position = startPos;
                    }
                }
            }
            else
            {
                //If the door is open and the angle is not greater than maxDistance
                //Increment distance and set transform
                if (opened && angle < angleDistance)
                {
                    angle += speed * Time.deltaTime;
                    transform.Rotate(Vector3.up, speed * Time.deltaTime);
                    if (angle > angleDistance)
                    {
                        angle = angleDistance;
                        transform.rotation = Quaternion.Euler(0, angleDistance, 0);
                    }
                }

                //If the door is closed and the door has not finished closing
                //Decrement and set transform
                if (!opened && angle > 0)
                {
                    angle -= speed * Time.deltaTime;
                    transform.Rotate(Vector3.up, -speed * Time.deltaTime);
                    if (angle < 0)
                    {
                        angle = 0;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
            }
        }
    }

    /*
     * Used as an interface to open the door
     */
    public void open()
    {
        opened = true;
    }

    /*
     * Used as an interface to close the door
     */
    public void close()
    {
        opened = false;
    }
}