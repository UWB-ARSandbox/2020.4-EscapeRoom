using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject activatorObj;

    [SerializeField]
    private ActivatedObject activatedObj;

    [SerializeField]
    private bool isActivated;

    [SerializeField]
    private bool isInteractable;

    [SerializeField]
    private float interactDistance;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(activatorObj != null);
        Debug.Assert(activatedObj != null);
        Debug.Assert(interactDistance > 0);

        // Consider adding a "register activator" method to ActivatedObject and
        // calling it at start. Might be a problem with initialization order
        // which is why this hasn't been added yet.
    }

    // Activation Status Getters and Setters
    public void SetActivationStatus(bool active) { isActivated = active; }
    public bool GetActivationStatus() { return isActivated; }


    /************************** IInteractable - Start *************************/

    //[SerializeField]
    //private bool _isInteractable;
    //public bool isInteractable
    //{
    //    get => _isInteractable;
    //    set => _isInteractable = value;
    //}

    //[SerializeField]
    //private float _interactionRange;
    //public float interactionRange
    //{
    //    get => _interactionRange;
    //    set { _interactionRange = (value > 0) ? value : float.PositiveInfinity; }
    //}

    //public StatusInfo Hover(Transform t)
    //{
    //    if(_isInteractable)
    //    {
    //        Vector3 actorToActivator = t.position - this.transform.position;
    //        if (actorToActivator.magnitude < _interactionRange)
    //        {
    //            return activatedObj.GetStatus();
    //        }
    //    }
    //    return null;
    //}

    public void Interact(Transform t)
    {

    }

    public void InteractRelease()
    {
        //activatedObj.ToggleActivation();
    }

    public bool CanInteract(Transform other)
    {
        if (isInteractable)
        {
            Vector3 differenceV = other.position - this.transform.position;
            if (differenceV.magnitude <= interactDistance)
            {
                return true;
            }
        }
        return false;
    }

    /************************** IInteractable - End ***************************/

}
