using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [SerializeField]
    private bool isOn;

    [SerializeField]
    private Vector3 beamOrigin;

    [SerializeField]
    private GameObject prevHit;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(laserBeamObj != null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnOff(bool status)
    {
        isOn = status;
        this.transform.gameObject.SetActive(isOn);
    }

    public bool GetOnOff()
    {
        return isOn;
    }

    public GameObject GetHitObject()
    {
        return prevHit;
    }

    public void OnSourceChange(Vector3 newOrigin, Vector3 newDir)
    {
        if (isOn)
        {
            beamOrigin = newOrigin;
            this.transform.up = newDir.normalize;
            RaycastHit hit;
            if (Physics.Raycast(beamOrigin, this.transform.up, out hit, Mathf.Infinity, layerMask))
            {
                this.transform.localScale.y = hit.distance;
                Debug.Log("Laser hit at " + hit.point);

                ILaser hitILaser = hit.transform.GetComponent<ILaser>();
                if (hitILaser != null)
                {
                    if (hit.transform.gameObject == prevHit)
                    {
                        // Update downstream ILaser object hit by laser
                        hitILaser.OnLaserHit(this.transform.up, hit);
                    }
                    else
                    {
                        // If previous object hit by laser was an ILaser object, 
                        // update that object and any downstream beams/beam-hits
                        ILaser prevLaserObj = prevHit.transform.GetComponent<ILaser>();
                        if (prevLR != null)
                        {
                            prevLaserObj.OnLaserSourceRemoved();
                        }

                        // Set prevHit to current hit
                        prevHit = hit.transform.gameObject;
                        
                        // Update hit object(s)
                        hitILaser.OnLaserHit(this.transform.up, hit);
                    }
                }
                else
                {
                    Debug.Log("Laser hit non-ILaser object");
                }
            }
            else
            {
                this.transform.localScale.y = 1000f;
                Debug.Log("Laser did not hit. Laser length set to 1000m");
            }
        }
    }

}
