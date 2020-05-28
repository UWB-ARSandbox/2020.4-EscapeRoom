using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    [SerializeField] private LaserBeam laser;
    [SerializeField] private bool isOn;
    //[SerializeField] private bool isInteractable;
    //[SerializeField] private bool isInteracting;
    //[SerializeField] private bool canMove;
    //[SerializeField] private bool canRotate;
    //private Vector3 prevInteractPos;
    //private Quaternion prevInteractRot;

    [SerializeField] private float interactDistance;

    [SerializeField] private ASL.ASLObject aslObject;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(laser != null);
        SetOnOff(false);

        aslObject = this.gameObject.GetComponent<ASL.ASLObject>();
        Debug.Assert(aslObject != null);

        aslObject._LocallySetFloatCallback(ASL_EmitterActivationCallback);
    }

    // Update is called each frame
    // Used to update other player's scene with local changes via ASL
    void Update()
    {
        //gameobject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        aslObject.SendAndSetClaim(() =>
        {
            //gameobject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(this.transform.position);
            //gameobject.GetComponent<ASL.ASLObject>().SendAndSetWorldRotation(this.transform.rotation);
            aslObject.SendAndSetWorldPosition(this.transform.position);
            aslObject.SendAndSetWorldRotation(this.transform.rotation);

        });
    }

    public void ToggleOnOff()
    {
        //Debug.Log("Toggled emitter to: ");
        //Debug.Log(!isOn);
        SetOnOff(!isOn);

        // Update other player's LaserEmitter to match this one via ASL
        aslObject.SendAndSetClaim(() =>
        {
            float[] onOffValue = new float[1];
            if (isOn)
                onOffValue[0] = 1.0f;
            else
                onOffValue[0] = -1.0f;
            
            aslObject.SendFloatArray(onOffValue);
        });
    }

    public void SetOnOff(bool on)
    {
        isOn = on;
        //Debug.Log("isOn = " + isOn);
        if (isOn)
        {
            laser.ActivateBeam();
        }
        else
        {
            laser.DeactivateBeam();
        }
    }

    public bool GetOnOff()
    {
        return isOn;
    }

    public float GetInteractDistance()
    {
        return interactDistance;
    }

    // Callback function for ASL's SendFloatArray() on this object
    // Uses the first value of the float array (f[0]) to set the LaserEmitter's
    // activation status to ON if it is greater than 0, and OFF if 0 or less
    public void ASL_EmitterActivationCallback(string _id, float[] f)
    {
        if (f.Length > 0)
        {
            if (f[0] > 0)
            {
                SetOnOff(true);
            }
            else
            {
                SetOnOff(false);
            }
        }
    }


    //public void Interact(Transform other)
    //{
    // OLD Interact Code
    //if (isInteracting)
    //{
    //    if (canMove)
    //    {
    //        Vector3 differenceV = other.position - prevInteractPos;
    //        Vector3 newPos = this.transform.position + differenceV;
    //        this.transform.position = newPos;
    //    }
    //    // NOTE: For stationary objects with rotation enabled, it probably
    //    // makes sense to handle this differently (i.e. based on pivot and
    //    // interactor location). This should work well for carrying objects.
    //    if (canRotate)
    //    {
    //        Quaternion differenceQ = prevInteractRot * Quaternion.Inverse(other.rotation);
    //        this.transform.rotation = differenceQ * this.transform.rotation;
    //    }
    //}
    //else
    //{
    //    // QUESTION: Should we check for interactability here or in the
    //    // interaction controller?
    //    if (CanInteract(other))
    //    {
    //        // Establish interact connection
    //        isInteracting = true;
    //        prevInteractPos = other.position;
    //        prevInteractRot = other.rotation;
    //    }
    //}
    //}

    //public void InteractRelease()
    //{
    //isInteracting = false;
    //}

    //public bool CanInteract(Transform other)
    //{
    //if (isInteractable && !isInteracting)
    //{
    //    Vector3 differenceV = other.position - this.transform.position;
    //    if (differenceV.magnitude <=  interactDistance)
    //    {
    //        return true;
    //    }
    //}
    //return false;
    //}

}
