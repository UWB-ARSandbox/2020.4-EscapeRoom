using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraOnTrigger : MonoBehaviour
{
    void onCollisionStay(Collision other)
    {
        UnityEngine.Debug.Log(other.gameObject);
        if (other.gameObject.tag == "Wall")
        {
            UnityEngine.Debug.Log("collide");
            //this.transform.position -= this.transform.forward * 20f;
        }
    }
}
