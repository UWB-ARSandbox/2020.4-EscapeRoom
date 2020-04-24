using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoweredDoor : ActivatedObject
{
    [SerializeField]
    private bool isOpen;

    [SerializeField]
    private bool isLocked;

    [SerializeField]
    private bool isPoweredOn;

    [SerializeField]
    private Animation objAnim;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(objAnim != null);
    }

    public void SetPowerStatus(bool isOn)
    {
        isPoweredOn = isOn;
    }

    public void SetLockedStatus(bool locked)
    {
        isLocked = locked;
    }

    protected override void OnActivate()
    {
        if (isPoweredOn)
        {
            if (!isLocked)
            {
                if (!isOpen)
                {
                    objAnim.Play("Open");
                }
            }
        }
    }

    protected override void OnDeactivate()
    {
        if (isPoweredOn)
        {
            if (!isLocked)
            {
                if (isOpen)
                {
                    objAnim.Play("Open");
                }
            }
        }
    }

}
