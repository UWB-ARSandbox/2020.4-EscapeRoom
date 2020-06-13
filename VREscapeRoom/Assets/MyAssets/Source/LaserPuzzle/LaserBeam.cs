using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private GameObject laserSegmentPrefab;

    [SerializeField]
    private List<GameObject> segments;

    [SerializeField]
    private List<GameObject> hitObjects;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(laserSegmentPrefab != null);
        Debug.Assert(segments.Count == 0);
        Debug.Assert(hitObjects.Count == 0);
        //ActivateBeam();
    }

    // Update is called each frame
    // Used here for updating ASL if the beam is activated or deactivated.
    //void Update()
    //{

    //}

    // Activates the laser beam, creating a new initial beam segment and runs
    // the initial calculations for the beams intersections using 
    // RecalculateAtSegment().
    // ASSUMES: that LaserBeam.cs is attached to an object whose up vector is the
    // direction of the beam and that this object is not inside any other objects
    public void ActivateBeam()
    {   
        // Clear beam segment objects (if not clear already) and object lists
        if (segments.Count > 0)
            ClearSegmentsAt(0);
        segments = new List<GameObject>();
        hitObjects = new List<GameObject>();

        // Start new beam
        GameObject newSegment = Instantiate(laserSegmentPrefab);
        segments.Add(newSegment);
        RecalculateAtSegment(0, this.transform.position, this.transform.up);
        //Debug.Log("ActivateBeam() complete, starting coroutine: UpdateBeam()");
        StartCoroutine("UpdateBeam");
    }

    // Deactivates the laser beam, clearing all segments in the beam and any
    // related data on ILaser objects it had hit using ClearSegmentsAfter()
    public void DeactivateBeam()
    {
        Debug.Log("Inside DeactivateBeam()");
        ClearSegmentsAt(0);
        ClearHitObjectsAtIndex(0);
        StopCoroutine("UpdateBeam");
    }

    // Returns the direction of a given segment of the laser beam.
    // Returns (0, 0, 0) if the provided index is invalid.
    public Vector3 GetSegmentDirection(int index)
    {
        if (index >= 0 && index <= segments.Count - 1)
        {
            return segments[index].transform.up;
        }
        else return Vector3.zero;
    }

    private IEnumerator UpdateBeam()
    {
        while (true)
        {
            //Debug.Log("In UpdateBeam()");

            // Clear all segment objects after the starting segment
            ClearSegmentsAt(1);
            // Recalculate beam from the starting segment using current position
            // and direction of this LaserBeam obj (usually attached emitter obj)
            RecalculateAtSegment(0, this.transform.position, this.transform.up);

            yield return new WaitForSeconds(0.05f);
        }
    }

    // Public method for updating the beam at a given segment. Mainly for use by
    // ILaser objects when they are modified so that the beam hitting the object
    // is also updated accordingly.
    public void UpdateBeamAt(int index)
    {
        if (index >= 0 && index <= segments.Count - 1)
        {
            RecalculateAtSegment(index, segments[index].transform.position, segments[index].transform.up);
        }
    }

    // RecalculateAtSegment()
    // Recursive helper function for checking the intersection of a beam segment
    // with other objects, updating it, and clearing or creating other segments
    // as needed.
    private void RecalculateAtSegment(int index, Vector3 newOrigin, Vector3 newDir)
    {
        // DEBUG
        //Debug.Log("Inside RecalculateAtSegment()");
        //Debug.Log("index = " + index);
        //Debug.Log("newDir = " + newDir);
        //Debug.Log("newOrigin = " + newOrigin);
        //Debug.Log("segments.Count = " + segments.Count);
        //Debug.Log("hitObjects.Count = " + hitObjects.Count);

        // Update the position and direction of the beam using parameter values
        segments[index].transform.position = newOrigin;
        segments[index].transform.up = newDir;

        // Re-check where the beam hits using a raycast
        RaycastHit hit;
        if (Physics.Raycast(newOrigin, segments[index].transform.up, out hit, Mathf.Infinity))
        {
            // Set segment scale to match distance to hit location
            Vector3 scale = segments[index].transform.localScale;
            scale.y = hit.distance;
            segments[index].transform.localScale = scale;

            //// First, clear any segments beyond this one (if there are any)
            //if (index + 1 < segments.Count)
            //    ClearSegmentsAt(index + 1);

            //// Next, add the object the laser just hit to the hitObjects list
            //hitObjects.Add(hit.transform.gameObject);

            // If the current object that was hit is different than what was hit
            // previously, then clear hitObjects from here on and add current 
            // hit objects
            if (index >= hitObjects.Count || hit.transform.gameObject != hitObjects[index])
            {
                ClearHitObjectsAtIndex(index);
                hitObjects.Add(hit.transform.gameObject);
            }

            // If the object the beam just hit is an ILaser object, call the
            // OnLaserHit() method for that object and recurse to calculate
            // for the new beam segment.
            ILaser hitILaser = hit.transform.GetComponent<ILaser>();
            if (hitILaser != null) // Hit ILaser object
            {
                Vector3 resultDir = hitILaser.OnLaserHit(index, this, hit.normal);
                
                //Debug.Log("Hit ILaser Object. Result Direction = " + resultDir);

                if (   resultDir.x == Mathf.NegativeInfinity
                    || resultDir.y == Mathf.NegativeInfinity
                    || resultDir.z == Mathf.NegativeInfinity )
                {
                    return;
                }
                else
                {
                    GameObject newSegment = Instantiate(laserSegmentPrefab);
                    segments.Add(newSegment);
                    RecalculateAtSegment(index + 1, hit.point, resultDir);
                }
            }
            else
            {
                return; // hit object doesn't interact with laser, beam is at endpoint (done)
            }
        }
        // Else, the beam didn't hit anything, so we set segment length to
        // 1000m and clear any beam segments that used to come after it.
        else
        {
            if (index + 1 < segments.Count)
                ClearSegmentsAt(index + 1);
            Vector3 scale = segments[index].transform.localScale;
            scale.y = scale.y * 1000.0f;
            segments[index].transform.localScale = scale;
            Debug.Log("Laser did not hit. Laser length set to 1000m.");
        }

    }

    // Clears the all beam segments from a specified index onwards
    private void ClearSegmentsAt(int index)
    {
        //Debug.Log("Begin: ClearSegmentsAt()");
        //Debug.Log("index = " + index);

        if (index < segments.Count)
        {
            //Debug.Log("Before For-Loop: segments.Count = " + segments.Count + " and hitObjects.Count = " + hitObjects.Count);

            for (int i = index; i < segments.Count; i++)
            {
                Destroy(segments[i]);
            }

            int segsToRemove = segments.Count - index;

            Debug.Log("After For-Loop: segments.Count = " + segments.Count);
            Debug.Log("segsToRemove = " + segsToRemove);

            segments.RemoveRange(index, segsToRemove);

            Debug.Log("After RemoveRange was called: segments.Count = " + segments.Count);
            Debug.Log("End: ClearSegmentsAt()");
        }
    }

    private void ClearHitObjectsAtIndex(int index)
    {
        //Debug.Log("Begin: ClearHitObjectsAt()");
        //Debug.Log("index = " + index);

        if (index < hitObjects.Count && index >= 0)
        {
            for (int i = index; i < hitObjects.Count; i++)
            {
                if (hitObjects[i] != null)
                {
                    ILaser il = hitObjects[i].GetComponent<ILaser>();
                    if (il != null)
                    {
                        il.OnLaserSourceRemoved(i, this);
                    }
                }
            }
            //Debug.Log("hitObjects.Count = " + hitObjects.Count);
            int hitObjsToRemove = hitObjects.Count - index;
            //Debug.Log("hitObjsToRemove = " + hitObjsToRemove);
            hitObjects.RemoveRange(index, hitObjsToRemove);
            //Debug.Log("After RemoveRange() was called, hitObjects.Count = " + hitObjects.Count);
            //Debug.Log("End: ClearHitObjectsAt()");
        }
    }

}
