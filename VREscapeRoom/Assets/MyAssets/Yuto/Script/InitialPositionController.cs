using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialPositionController : MonoBehaviour
{
    [SerializeField] private Vector3 P1InitialPos;
    [SerializeField] private Vector3 P2InitialPos;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = P1InitialPos;

        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 1)
        //{
        //    this.transform.position = P1InitialPos;
        //}
        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 2)
        //{
        //    this.transform.position = P2InitialPos;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
