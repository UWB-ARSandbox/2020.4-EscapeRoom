using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedObject : MonoBehaviour
{
    [SerializeField]
    private GameObject controlledObject;

    [SerializeField]
    private Activator[] activators;

    [SerializeField]
    private bool isActivated;

    [SerializeField]
    private StatusInfo status;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(controlledObject != null);
        Debug.Assert(activators != null && activators.Length > 0);
    }

    public void ToggleActivation()
    {
        isActivated = !isActivated;
        foreach (Activator activator in activators)
        {
            activator.SetActivationStatus(isActivated);
        }
        if (isActivated)
        {
            OnActivate();
        }
        else
        {
            OnDeactivate();
        }
    }

    public StatusInfo GetStatus()
    {
        return status;
    }

    // Virtual methods to be overriden by subclasses representing specific
    // types of objects that can be activated by an Activator. 
    // For example, lights, power-doors, electronics (turn on/off), etc.
    protected virtual void OnActivate() { Debug.Log("ERROR: Virtual OnActivate() not implemented"); }
    protected virtual void OnDeactivate() { Debug.Log("ERROR: Virtual OnDeactivate() not implemented"); }

}
