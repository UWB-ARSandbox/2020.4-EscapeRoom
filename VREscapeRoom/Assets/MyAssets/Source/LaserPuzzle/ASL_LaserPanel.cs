using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ASL_LaserPanel : MonoBehaviour
{
    [SerializeField] private ASL.ASLObject aslObject;

    // Start is called before the first frame update
    void Start()
    {
        if (aslObject == null)
        {
            aslObject = this.gameObject.GetComponent<ASL.ASLObject>();
            Debug.Assert(aslObject != null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        aslObject.SendAndSetClaim(() =>
        {
            // Note: currently not allowing these panels to be moved. If we do,
            // then uncomment the following line in order to:
            // Update position of this object for other player(s)
            //aslObject.SendAndSetWorldPosition(this.transform.position);

            // Update rotation of this object for other player(s)
            aslObject.SendAndSetWorldRotation(this.transform.rotation);
        });
    }
}
