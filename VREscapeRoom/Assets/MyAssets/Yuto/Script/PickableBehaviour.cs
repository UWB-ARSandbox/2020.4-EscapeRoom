using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using ASL;

public class PickableBehaviour : MonoBehaviour
{
    public GameObject parentToBe;
    public Vector3 positionToBe;
    public Vector3 scaleToBe;
    public Vector3 adjust;

    private bool placed;
    private float activate;

    void Start()
    {
        placed = false;
        activate = 0f;
        this.GetComponent<ASL.ASLObject>()._LocallySetFloatCallback(callBackMethod);
    }

    void Update()
    {
        if (activate == 1f && !placed)
        {
            setItemToPos();
            placed = true;
        }
    }

    public void callBackMethod(string _id, float[] f)
    {
        activate = f[0];
    }

    public void setActivate()
    {
        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            float[] myValue = new float[1];
            myValue[0] = 1f;
            //In this example, playerHealth would be updated to 3.5 for all users
            this.GetComponent<ASL.ASLObject>().SendFloatArray(myValue);
        });
    }

    void setItemToPos()
    {
        this.transform.SetParent(parentToBe.transform);

        //this.transform.localPosition = positionToBe;
        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetLocalPosition(positionToBe);
        });


        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetLocalRotation(new Quaternion(0, 0, 0, 1));
        });

        //this.transform.localScale = scaleToBe;

        this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        {
            this.GetComponent<ASL.ASLObject>().SendAndSetLocalScale(scaleToBe);
        });

        this.transform.tag = "Untagged";
    }

    public Vector3 getLocationToBe()
    {
        return positionToBe;
    }

    public bool isClose()
    {
        return Vector3.Distance(this.transform.position, parentToBe.transform.position + adjust) < 0.5f;
    }
}
