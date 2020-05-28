using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/*
 * This class is to be combined with the KeyPad
 * It is the display that shows the user the enteredCode on the keyPad
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/6/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class KeyPadDisplay : MonoBehaviour
{
    //text to display
    private TextMeshPro text;
    //Reference to the referenced keyPad
    [SerializeField] private KeyPad pad;

    //init text object
    void Start()
    {
        text = gameObject.GetComponentInChildren<TextMeshPro>();
    }

    // Update text each frame
    void Update()
    {
        text.text = pad.getCode();
    }
}
