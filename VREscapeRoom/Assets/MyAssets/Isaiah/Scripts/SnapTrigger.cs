using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Pickable")
        {
            if (other.gameObject.GetComponent<Snappable>())
                other.gameObject.GetComponent<Snappable>().snap();
        }
    }
}
