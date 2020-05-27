using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaser
{
    Vector3 OnLaserHit(int inboundSegmentIndex, LaserBeam beam, Vector3 hitNormal);
    void OnLaserSourceRemoved(int inboundSegmentIndex, LaserBeam beam);
}
