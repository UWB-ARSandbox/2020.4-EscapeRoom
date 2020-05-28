using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedKeyPad : ActivatedObject
{
    [SerializeField]
    protected bool isActivated;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
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
        this.gameObject.SetActive(true);
        target.SetActive(false);
        isActivated = true;
    }

    public override void Deactivate()
    {
        //this.gameObject.SetActive(false);
        //target.SetActive(true);
        //isActivated = false;
    }
}
