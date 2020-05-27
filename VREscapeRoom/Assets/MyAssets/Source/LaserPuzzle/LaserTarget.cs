using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour, ILaser
{
    [SerializeField] private ActivatedObject[] activatedObjects;
    //[SerializeField] private ASL.ASLObject aslObject;

    private bool isInitialHit;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(activatedObjects.Length > 0);
        isInitialHit = true;

        //if (aslObject == null)
        //{
        //    aslObject = this.gameObject.GetComponent<ASL.ASLObject>();
        //    Debug.Assert(aslObject != null);
        //}
    }

    public Vector3 OnLaserHit(int inboundSegmentIndex, LaserBeam beam, Vector3 hitNormal)
    {
        if (isInitialHit)
        {
            foreach (ActivatedObject ao in activatedObjects)
            {
                ao.Activate();
            }
            isInitialHit = false;

            //float[] on = new float[1];
            //on[0] = 1.0f;
            //aslObject.SendFloatArray(on);
        }
        return Vector3.negativeInfinity;
    }

    public void OnLaserSourceRemoved(int inboundSegmentIndex, LaserBeam beam)
    {
        Debug.Log("Inside OnLaserSourceRemoved() on " + this.gameObject.name);
        foreach (ActivatedObject ao in activatedObjects)
        {
            ao.Deactivate();
        }
        isInitialHit = true;

        //float[] off = new float[1];
        //off[0] = -1.0f;
        //aslObject.SendFloatArray(off);
    }

    //// Callback function for ASL's SendFloatArray() on this object
    //// Uses the first value of the float array (f[0]) to set the LaserEmitter's
    //// activation status to ON if it is greater than 0, and OFF if 0 or less
    //public void ASL_TargetActivationCallback(string _id, float[] f)
    //{
    //    if (f.Length > 0)
    //    {
    //        if (f[0] > 0)
    //        {
                
    //        }
    //        else
    //        {
                
    //        }
    //    }
    //}
}
