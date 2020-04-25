using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflector : MonoBehaviour, ILaser
{
    [SerializeField]
    private LaserBeam inboundLaser;

    [SerializeField]
    private LaserBeam reflectedLaser;

    [SerializeField]
    private Vector3 prevPosition;

    [SerializeField]
    private Quaternion prevRotation;

    [SerializeField]
    private Vector3 prevScale;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = this.transform.position;
        prevRotation = this.transform.rotation;
        prevScale = this.transform.localScale;
        Debug.Assert(reflectedLaser != null);
    }

    // Update is called once per frame
    void Update()
    {
        // If reflector has changed, update laser & prev xform info
        if (   this.prevPosition != this.transform.position
            || this.prevRotation != this.transform.rotation
            || this.prevScale != this.transform.localScale  )
        {
            inboundLaser.OnSourceChange(inboundLaser.transform.position, inboundLaser.transform.up);
            this.prevPosition = this.transform.position;
            this.prevRotation = this.transform.rotation;
            this.prevScale = this.transform.localScale;
        }
        // Consider removing this and having the user call a function any time
        // the transform is changed instead of checking to see if it changed.
    }

    public void OnLaserHit(LaserBeam inLaser, RaycastHit hit)
    {
        inboundLaser = inLaser;
        if (!reflectedLaser.GetOnOff())
            reflectedLaser.SetOnOff(true);
        Vector3 reflectedDir = Vector3.Reflect(laserDir, hit.normal);
        reflectedLaser.OnSourceChange(hitPos, reflectedDir);
    }

    public void OnLaserSourceRemoved()
    {
        ILaser outboundLaserHitObj = reflectedLaser.GetHitObject().GetComponent<ILaser>();
        if (outboundLaserHitObj != null)
        {
            outboundLaserHitObj.OnLaserSourceRemoved();
        }
        reflectedLaser.SetOnOff(false);
        if (inboundLaser != null)
        {
            inboundLaser = null;
        }
    }

}
