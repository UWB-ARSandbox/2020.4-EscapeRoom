using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    //bool isInteractable { get; set; }
    //StatusInfo Hover(Transform t);
    void Interact(Transform t);
    void InteractRelease();
    bool CanInteract(Transform t);
}
