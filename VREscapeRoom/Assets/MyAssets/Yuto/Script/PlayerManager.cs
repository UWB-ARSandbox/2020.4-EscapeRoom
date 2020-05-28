using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private GameObject playspace;
    [SerializeField] private GameObject playspaceCamera;
    [SerializeField] private PlayspaceInitializer playspaceInitializer;
    private Vector3 P1PreviousPos;
    private Vector3 P2PreviousPos;

    // Start is called before the first frame update
    void Start()
    {
        //// Yuto's Original Version
        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 1)
        //{
        //    Player1.transform.position = playerCamera.transform.position;
        //    Player1.transform.rotation = playerCamera.transform.rotation;
        //    Player1.transform.parent = playerCamera.transform;
        //}
        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 2)
        //{
        //    Player2.transform.position = playerCamera.transform.position;
        //    Player2.transform.rotation = playerCamera.transform.rotation;
        //    Player2.transform.parent = playerCamera.transform;
        //}

        // Cody's Version for use with PlayerController_R
        PlayerController_R p1_Controller = player1.GetComponent<PlayerController_R>();
        PlayerController_R p2_Controller = player2.GetComponent<PlayerController_R>();
        if (ASL.GameLiftManager.GetInstance().m_PeerId == 1 && p1_Controller != null)
        {
            // Set the player collider in the playspaceInitializer to that the 
            // playspace colliders will ignore the player's rigidbody.
            Collider P1_Collider = player1.GetComponent<Collider>();
            if (P1_Collider != null)
            {
                playspaceInitializer.SetPlayerCollider(P1_Collider);
            }
            else
            {
                Debug.Log("Player 1 GameObject is missing a collider. Is Player1's G.O. set correctly?");
            }

            // Assign the playspace and camera variables on Player1's 
            // PlayerController_R script.
            p1_Controller.Initialize(playspace, playspaceCamera);

            // Disable other player object's PlayerController_R script to
            // avoid input conflict.
            p2_Controller.enabled = false;
        }
        else if (ASL.GameLiftManager.GetInstance().m_PeerId == 2 && p2_Controller != null)
        {
            // Set the player collider in the playspaceInitializer to that the 
            // playspace colliders will ignore the player's rigidbody.
            Collider P2_Collider = player2.GetComponent<Collider>();
            if (P2_Collider != null)
            {
                playspaceInitializer.SetPlayerCollider(P2_Collider);
            }
            else
            {
                Debug.Log("Player 1 GameObject is missing a collider. Is Player1's G.O. set correctly?");
            }

            // Assign the playspace and camera variables on Player1's 
            // PlayerController_R script.
            p2_Controller.Initialize(playspace, playspaceCamera);

            // Disable other player object's PlayerController_R script to
            // avoid input conflict.
            p1_Controller.enabled = false;
        }
        else
        {
            Debug.Log("A PlayerController_R script is not attached to a player body object");
            Debug.Assert(p1_Controller != null);
            Debug.Assert(p2_Controller != null);
        }
    }

    void Update()
    {
        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 1 && P1PreviousPos != player1.transform.position)
        //{
        //    player1.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        //    {
        //        player1.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(player1.transform.position);
        //    });
        //}

        //if (ASL.GameLiftManager.GetInstance().m_PeerId == 2 && P2PreviousPos != player2.transform.position)
        //{
        //    player2.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
        //    {
        //        player2.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(player2.transform.position);
        //    });
        //}

        //P1PreviousPos = player1.transform.position;
        //P2PreviousPos = player2.transform.position;
    }
}
