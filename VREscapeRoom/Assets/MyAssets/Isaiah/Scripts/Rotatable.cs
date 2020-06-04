using ASL;
using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rotatable : MonoBehaviour
{
    [SerializeField] private GameObject parent;
    private GameObject controller;
    private ASLObject asl;
    private bool active = false;
    private bool prev = false;
    private Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        asl = parent.GetComponent<ASLObject>();
        asl._LocallySetFloatCallback(callBackMethod);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void callBackMethod(string id, float[] f)
    {
            Vector3 r = new Vector3(f[0], f[1], f[2]);
            Vector3 p = new Vector3(f[3], f[4], f[5]);
            Vector3 sp = new Vector3(f[6], f[7], f[8]);
            parent.transform.position = p;
            parent.transform.eulerAngles = r;
            startPos = p;
    }

    private float[] setCallbackArray()
    {
        float[] f = new float[9];
        Vector3 r = parent.transform.eulerAngles;
        Vector3 p = parent.transform.position;
        f[0] = r.x;
        f[1] = r.y;
        f[2] = r.z;
        f[3] = startPos.x;
        f[4] = startPos.y;
        f[5] = startPos.z;
        return f;
    }

    /*
     * Activate the rotatable
     */
    public void activate()
    {
        Debug.Log("activate");
        if (controller)
        {
            if (!active)
            {
                active = true;
                parent.transform.parent = controller.transform;
                asl.SendAndSetClaim(null, 0, true);
            }
        }
    }


    /*
     * Deactivate the rotatable
     */
    public void deactivate()
    {
        Debug.Log("deactivate");
        if (active)
        {
            active = false;
            parent.transform.parent = null;
            setNewRotation();
            asl.SendAndSetClaim(send, 500, true);
            parent.transform.position = startPos;
            controller = null;
        }
    }

    private void setNewRotation()
    {
        Vector3 rot = parent.transform.eulerAngles;
        rot.x = Mathf.Round(rot.x/90)*90;
        rot.y = Mathf.Round(rot.y / 90) * 90;
        rot.z = Mathf.Round(rot.z / 90) * 90;
        parent.transform.eulerAngles = rot;
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
        }
    }

    private void send()
    {
        asl.SendFloatArray(setCallbackArray());
    }
}
