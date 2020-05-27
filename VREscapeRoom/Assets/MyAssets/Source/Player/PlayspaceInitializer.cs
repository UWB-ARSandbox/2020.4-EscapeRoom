using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayspaceInitializer : MonoBehaviour
{
    // IMPORTANT: This script must be attached to the playspace instance
    // in order for player assignment and movement to work correctly.

    // This player's body collider 
    [SerializeField] private Collider playerCollider;

    // Bool used in order to run the collider ignore code a single time on the
    // first update only. NOTE: This MUST occur in update, not in start because
    // of how the playspace is getting setup in the current "standalone scene." 
    // Colliders for the playspace objects like controllers don't seem to exist
    // when this script runs Start() but they do on the first Update().
    private bool isFirstUpdate = true;

    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    // Update is called once per frame
    void Update()
    {
        if (isFirstUpdate)
        {
            Debug.Assert(playerCollider != null);
            Debug.Log("BEGIN: Playspace First Update()");

            foreach (Transform child in transform)
            {
                Debug.Log("Playspace Child: " + child.name);
                Collider childCollider = child.GetComponent<Collider>();
                if (childCollider != null)
                {
                    Debug.Log("Playespace child: " + child.gameObject.name + " has collider." + childCollider);
                    Physics.IgnoreCollision(playerCollider, childCollider);
                    Physics.IgnoreCollision(childCollider, playerCollider);
                    Debug.Log("Playspace child set to ignore player; Player set to ignore playspace child collision.");
                }
            }
            isFirstUpdate = false;
            Debug.Log("END: Playspace First Update()");
        }
    }

    // Sets the playerCollider reference to match the player object for the 
    // local instance of the player. 
    public void SetPlayerCollider(Collider pCollider)
    {
        playerCollider = pCollider;
    }

}
