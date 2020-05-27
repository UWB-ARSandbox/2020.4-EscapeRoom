using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedDoor : ActivatedObject
{
    [SerializeField]
    protected bool isActivated;

    public Animator doorAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = this.gameObject.GetComponent<Animator>();
        Debug.Assert(doorAnimator != null);
        if (doorAnimator == null)
        {
            Debug.Log("Door animator not assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ToggleActivation();
        }
    }

    public void ToggleActivation()
    {
        //Debug.Log("Begin ActivatedDoor::ToggleActivation()");

        if (isActivated)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public override void Activate()
    {
        doorAnimator.SetBool("IsOpen", true);
        isActivated = true;

        Debug.Log("End ActivatedDoor::Activate()");
    }

    public override void Deactivate()
    {
        doorAnimator.SetBool("IsOpen", false);
        isActivated = false;

        Debug.Log("End ActivatedDoor::Deactivate()");
    }
}
