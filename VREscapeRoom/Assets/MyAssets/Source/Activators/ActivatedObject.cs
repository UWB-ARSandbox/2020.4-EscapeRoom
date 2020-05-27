using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActivatedObject : MonoBehaviour
{
    // Abstract methods to be overriden by subclasses representing specific
    // types of objects that can be activated by an Activator. 
    // For example, lights, power-doors, electronics (turn on/off), etc.
    public abstract void Activate();
    public abstract void Deactivate();

}
