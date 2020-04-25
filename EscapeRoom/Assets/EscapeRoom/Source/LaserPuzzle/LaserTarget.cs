using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour, ILaser
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnLaserHit(LaserBeam inLaser, RaycastHit hit)
    {
        OnTargetHit();
    }

    public void OnLaserSourceRemoved()
    {

    }

    private void OnTargetHit()
    {

    }
}
