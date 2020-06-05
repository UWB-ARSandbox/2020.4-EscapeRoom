using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private DisplayTimer dt;
    void OnTriggerEnter(Collider other)
    {
        dt.quitGame();
    }
}
