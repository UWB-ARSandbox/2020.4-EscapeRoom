using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] private Game g;
    void OnTriggerEnter(Collider other)
    {
        g.LoadNextScene();
    }
}
