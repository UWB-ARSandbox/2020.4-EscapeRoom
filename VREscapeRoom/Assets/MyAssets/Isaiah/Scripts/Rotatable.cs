using ASL;
using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    [SerializeField] private Transform xForm;
    private GameObject controller;
    private ASLObject asl;
    private bool active = false;
    private bool prev = false;
    private Quaternion startRot;
    private Quaternion endRot;
    // Start is called before the first frame update
    void Start()
    {
        asl = xForm.gameObject.GetComponent<ASLObject>();
        asl._LocallySetFloatCallback(callBackMethod);
    }

    // Update is called once per frame
    void Update()
    {
        
        if(active && controller)
        {
            endRot = controller.transform.rotation.normalized;
        }
    }

    private void callBackMethod(string id, float[] f)
    {
        Vector3 r = new Vector3(f[0], f[1], f[2]);
        xForm.eulerAngles = r;
    }

    private float[] setCallbackArray()
    {
        float[] f = new float[3];
        Vector3 r = xForm.eulerAngles;
        f[0] = r.x;
        f[1] = r.y;
        f[2] = r.z;
        return f;
    }

    /*
     * Activate the rotatable
     */
    public void activate()
    {
        active = true;
    }


    /*
     * Deactivate the rotatable
     */
    public void deactivate()
    {
        active = false;
    }

    private void setNewRotation()
    {
        xForm.rotation = xForm.rotation * (Quaternion.Inverse(startRot) * endRot);
        Vector3 rot = xForm.eulerAngles;
        rot.x = Mathf.Round(rot.x/90)*90;
        rot.y = Mathf.Round(rot.y / 90) * 90;
        rot.z = Mathf.Round(rot.z / 90) * 90;
        xForm.eulerAngles = rot;
    }

    /*
     * If the object that collides with the rotatable is a hand, then save that hand
     * Only save the first hand to collide with it
     */
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Hand")
        {
            if(!controller)
            {
                controller = other.gameObject;
                activate();
                startRot = controller.transform.rotation.normalized;
                asl.SendAndSetClaim(send, 1500, true);
            }
        }
    }

    /*
     * If the saved controller leaves the rotable stop rotating
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == controller)
        {
            controller = null;
            deactivate();
            setNewRotation();
        }
    }

    private void send()
    {
        asl.SendFloatArray(setCallbackArray());
    }
}
