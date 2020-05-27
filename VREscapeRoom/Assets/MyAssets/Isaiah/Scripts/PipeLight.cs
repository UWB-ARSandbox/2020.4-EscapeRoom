using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is used with the Pipes
 * It is meant to be used on a pipe input or output.
 * It will give the user feedback to tell if the I/O is currently on or off
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class PipeLight : MonoBehaviour
{
    //The pipe refernece to tell if the I/O is on or off
    [SerializeField] private Pipe pipe = null;
    //On material
    [SerializeField] private Material onMat = null;
    //Off material
    [SerializeField] private Material offMat = null;
    //The mesh renderer of this object
    private MeshRenderer mesh = null;
    //Bool that saves the prev frame on/off
    private bool prev;

    // Start is called before the first frame update
    void Start()
    {
        //Init starting material
        mesh = gameObject.GetComponent<MeshRenderer>();
        setMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        //Only change if different from previous frame
        if(pipe.isOn() != prev)
        {
            prev = pipe.isOn();
            setMaterial();
        }
    }

    /*
     * Sets the material depending on the status of the pipe
     */
    private void setMaterial()
    {
        if(pipe.isOn())
        {
            mesh.material = onMat;
        }
        else
        {
            mesh.material = offMat;
        }
    }
}
