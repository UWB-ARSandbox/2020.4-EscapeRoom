using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/*
 * This class is a Pipe. It can be used to simulate a system of pipes that have a liquid or gas flowing though them
 * (Could be anything but referenced in a way that assumes water).
 * When pipes connect together, they will chain the connection and continue the flow of water
 * If a pipe is a source, then it will always output water and can't be turned off
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */

public class Pipe : MonoBehaviour
{
    //List that stores all the triggers for the pipes
    //Zone where pipes will connect to each other to turn on/off
    [SerializeField] private List<Collider> triggers;
    //If this pipe is a source pipe, turns on other pipes, can't be turned off
    [SerializeField] private bool isSource = false;
    //Material if water is flowing through the pipe
    [SerializeField] private Material onMat;
    //Material is water is not flowing through the pipe
    [SerializeField] private Material offMat;
    //List of all the pipes. Used to create junction pipes (T-Pipe, L-Pipe)
    [SerializeField] private List<GameObject> pipes;
    //Serialized for debugging purposes, bool to tell if water if flowing
    [SerializeField] private bool on = false;
    //List of every Pipe that is connected to this pipe
    private List<Pipe> connectedTo;
    //bool that stores what on was the prev frame
    private bool prev = false;

    // Start is called before the first frame update
    void Start()
    {
        //Init the list
        connectedTo = new List<Pipe>();
        //set on
        on = isSource;
        if(on)
        {
            //For every pipe, set the material to on material
            for(int i = 0; i < pipes.Count; i++)
            {
                MeshRenderer mesh = pipes[i].GetComponent<MeshRenderer>();
                mesh.material = onMat;
            }
        }
        else
        {
            //For every pipe, set the material to off material
            for (int i = 0; i < pipes.Count; i++)
            {
                MeshRenderer mesh = pipes[i].GetComponent<MeshRenderer>();
                mesh.material = offMat;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Only check if just turned on or just turned off
        if(on != prev)
        {
            if (on)
            {   //For every pipe, set the on material if turned on
                for (int i = 0; i < pipes.Count; i++)
                {
                    MeshRenderer mesh = pipes[i].GetComponent<MeshRenderer>();
                    mesh.material = onMat;
                }
            }
            else
            {
                //For every pipe, set the off material if turned off
                for (int i = 0; i < pipes.Count; i++)
                {
                    MeshRenderer mesh = pipes[i].GetComponent<MeshRenderer>();
                    mesh.material = offMat;
                }
            }
            //set prev so we do not set the mesh every frame
            prev = on;
        }
    }

    /*
     * When a pipe connects to another pipe, add the other pipe to the list
     * 
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WaterFlow")
        {
            Pipe toAdd = other.gameObject.GetComponent<Pipe>();
            //If pipe is already contained in the list, don't add   SHOULD NOT OCCUR, PIPE IS REMOVED DURING OnTriggerExit
            if(!connectedTo.Contains(toAdd))
                connectedTo.Add(other.gameObject.GetComponent<Pipe>());
            //If this pipe is a source or on, turn on the other pipe
            if (isSource || on)
            {
                other.gameObject.GetComponent<Pipe>().on = true;
            }
            //If the other pipe is on, turn this pipe on
            else if (other.gameObject.GetComponent<Pipe>().on)
            {
                this.on = true;
            }
            else
            {
                this.on = false;
            }
        }
    }

    /*
     * When a pipe is disconnected, check whether to keep the pipe on or off
     */
    private void OnTriggerExit(Collider other)
    {
        
        if (other.gameObject.tag == "WaterFlow")
        {
            Pipe toRemove = other.gameObject.GetComponent<Pipe>();
            //Remove the pipe from the list
            connectedTo.Remove(toRemove);
            //the other pipe should be turned off if it is not connected to a source
            bool remove = toRemove.connectedToSource();
            if (!remove)
            {
                toRemove.turnOff();
            }
        }
        //If this pipe is no longer connected to a source, turn it off
        if(!this.connectedToSource())
        {
            on = false;
        }
    }

    /*
     * Have to do so that if multiple pipes are connected then if one becomes disconnect then other pipes will keep it on
     * Will also cascade turning a pipe on when one pipe is turned on
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "WaterFlow")
        {
            //No need to add the pipe because it is already registered in the list
            //If this pipe is a source or on, turn on the other pipe
            if (isSource || on)
            {
                other.gameObject.GetComponent<Pipe>().on = true;
            }
            //If the other pipe is on, turn this pipe on
            else if (other.gameObject.GetComponent<Pipe>().on)
            {
                this.on = true;
            }
            else
            {
                this.on = false;
            }
        }
    } 

    /*
     * Returns true if the pipe is on
     */
    public bool isOn()
    {
        return on;
    }

    /*
     * Returns true if this pipe is a source or if it is connect to a source pipe
     * Searches every pipe that is connect using recursion. 
     */
    public bool connectedToSource()
    {
        if (isSource)
        {
            return true;
        }
        //List to store all checked Pipes
        List<Pipe> checkedPipes = new List<Pipe>();

        for(int i = 0; i < connectedTo.Count; i++)
        {
            //Add next pipe to list and check it
            checkedPipes.Add(connectedTo[i]);
            //Source Pipe found
            if (connectedTo[i]._connectedToSource(checkedPipes))
            {
                return true;
            }
        }
        //No source pipes were found
        return false;
    }

    /*
     * Returns true if this pipe is a source or if it is connect to a source pipe
     * Searches every pipe that is connect using recursion. 
     * Takes a list that stores all the previously checked pipes
     */
    private bool _connectedToSource(List<Pipe> checkedPipes)
    {
        if(isSource)
        {
            return true;
        }

        for (int i = 0; i < connectedTo.Count; i++)
        {
            Pipe nextPipe = connectedTo[i];
            //If the pipe was not already checked, add it to the list and check it. Prevents StackOverflow
            if(!checkedPipes.Contains(nextPipe))
            {
                checkedPipes.Add(nextPipe);
                //Source Pipe found
                if (nextPipe._connectedToSource(checkedPipes))
                {
                    return true;
                }
            }
        }
        //Source Pipe not found
        return false;
    }

    /*
     * Sets on for this pipe and all pipes it is connected to, to false
     * Uses recursion to traverse all pipes
     */
    public void turnOff()
    {
        //Turn the pipe off
        this.on = false;
        List<Pipe> offPipes = new List<Pipe>();
        //Turn off all connected pipes
        for(int i = 0; i < connectedTo.Count; i++)
        {
            Pipe next = connectedTo[i];
            offPipes.Add(next);
            next._turnOff(offPipes);
        }
    }

    /*
     * Sets on for this pipe and all pipes it is connected to, to false
     * Uses recursion to traverse all pipes
     * Uses a list to store all pipes already turned off
     */
    private void _turnOff(List<Pipe> offPipes)
    {
        //Turn the pipe off
        this.on = false;
        //turn off all connected pipes
        for(int i = 0; i < connectedTo.Count; i++)
        {
            Pipe next = connectedTo[i];
            //If the pipe was not already turned off, turn it off. Prevents StackOverflow
            if(!offPipes.Contains(next))
            {
                offPipes.Add(next);
                next._turnOff(offPipes);
            }
        }
    }
}