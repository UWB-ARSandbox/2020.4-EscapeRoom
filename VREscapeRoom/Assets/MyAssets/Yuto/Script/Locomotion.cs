using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public float x = 0.0f;
    public float z = 0.0f;
    public GameObject cam;
    public float speed = 1.0f;

    public void Update()
    {
        Vector3 camF = cam.transform.forward;
        Vector3 camR = cam.transform.right;
        camF.y = 0;
        camR.y = 0;
        camF = camF.normalized;
        camR = camR.normalized;

        transform.position += (camF * z + camR * x) * speed * Time.deltaTime;
    }
}
