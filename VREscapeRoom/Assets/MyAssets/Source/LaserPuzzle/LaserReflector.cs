using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserReflector : MonoBehaviour, ILaser
{
    [SerializeField]
    private List<int> inboundIndices;

    [SerializeField]
    private List<LaserBeam> inboundBeams;

    [SerializeField]
    private Vector3 prevPosition;

    [SerializeField]
    private Quaternion prevRotation;

    [SerializeField]
    private Vector3 prevScale;

    void Awake()
    {
        prevPosition = this.transform.position;
        prevRotation = this.transform.rotation;
        prevScale = this.transform.localScale;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    //void Update()
    //{
        // If reflector has changed, update laser & prev xform info
        //if (   this.prevPosition != this.transform.position
        //    || this.prevRotation != this.transform.rotation
        //    || this.prevScale != this.transform.localScale  )
        //{
        //    for (int i = 0; i < inboundBeams.Count; i++)
        //    {
        //        inboundBeams[i].UpdateBeamAt(i);
        //    }
        //    this.prevPosition = this.transform.position;
        //    this.prevRotation = this.transform.rotation;
        //    this.prevScale = this.transform.localScale;
        //}
        // Consider removing this and having the user call a function any time
        // the transform is changed instead of checking to see if it changed.
    //}

    // Implements ILaser Interface
    // This method is called by the beam when it hits the reflector object.
    // It stores the index of the segment that hit it, the beam it is a part of,
    // and then calculates the direction of the reflected incoming direction
    // using the provided hitNormal. 
    // (note: this last part could be moved outside this method since it doesn't
    // require data from this object but it seemed reasonable to have the 
    // reflector calculate the reflected direction)
    public Vector3 OnLaserHit(int inboundSegmentIndex, LaserBeam beam, Vector3 hitNormal)
    {
        if (!inboundBeams.Contains(beam))
        {
            inboundBeams.Add(beam);
            inboundIndices.Add(inboundSegmentIndex);
        }
        Vector3 inDir = beam.GetSegmentDirection(inboundSegmentIndex);
        Vector3 reflectDir = Vector3.Reflect(inDir, hitNormal);
        return reflectDir;
    }

    // Implements ILaser Interface
    // In the event of a LazerBeam no longer hitting this reflector, this clears
    // any information stored by the reflector for the given beam/segment.
    public void OnLaserSourceRemoved(int inboundSegmentIndex, LaserBeam beam)
    {
        if (!inboundBeams.Contains(beam))
        {
            inboundBeams.Remove(beam);
            inboundIndices.Remove(inboundSegmentIndex);
        }
    }

}
