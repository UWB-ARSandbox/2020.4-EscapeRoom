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

    public void setItemToPos()
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
