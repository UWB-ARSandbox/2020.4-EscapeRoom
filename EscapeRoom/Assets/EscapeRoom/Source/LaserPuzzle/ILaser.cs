using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILaser
{
    void OnLaserHit(LaserBeam inLaser, RaycastHit hit);
    void OnLaserSourceRemoved();
}
