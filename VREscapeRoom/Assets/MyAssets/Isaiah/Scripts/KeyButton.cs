using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This class is used with the KeyPad. This represents a single button on the keypad
 * buttonValues should be a number through 0-9 or has the option of being a Y or N to be 
 * asl compatible. 
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class KeyButton : MonoBehaviour
{
    //The value of the button
    [SerializeField] char buttonValue = '1';
    //The keypad that it belongs to
    private KeyPad parent = null;

    private void Start()
    {
        //Get parent keypad
        parent = gameObject.GetComponentInParent<KeyPad>();
        if (!parent)
            Debug.LogError("Parent Keypad not found");
    }

    /*
     * The button press
     */
    public void press()
    {
        parent.enterChar(buttonValue);
    }

    /*
     * Submit the entered code to the Keypad
     */
    public void submit()
    {
        parent.submit();
    }

    /*
     * Clears the entered code in the Keypad
     */
    public void cancel()
    {
        parent.canceled();
    }
}
