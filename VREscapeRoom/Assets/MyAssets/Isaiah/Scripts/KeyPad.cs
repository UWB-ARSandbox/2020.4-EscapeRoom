using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ASL;
using System.Xml.Serialization;
/*
* This class is a Keypad that is synced with the UWB ASL (Augmented Space Library)
* It is used to open and close a single door
* The code required to open the door can be set (Length of code must be 4)
* Other classes may submit, cancel, and enter chars to as the interface to this class
* =================================
* Author:     Isaiah Snow
* Assignment: CSS 499
* Date:       5/6/2020
* Group:      VREscapeRoom
* =================================
*/
public class KeyPad : MonoBehaviour
{
    //Door to open if code is set
    [SerializeField] private Door door;
    //Master code (DOESN'T CHANGE AFTER INIT)
    [SerializeField] private List<char> code;
    //Code that users enter
    [SerializeField] private List<char> enteredCode;
    private ASLObject asl = null;
    //Light for user feedback
    private KeyPadLight display;
    //bool to clear code
    public bool cancel = false;
    //bool to submit code
    public bool enter = false;
    //bool to toggle door unlocked/locked
    public bool unlocked = false;
    //char to enter for sendandsetclaim press
    private char toEnter = '0';

    // Start is called before the first frame update
    void Start()
    {
        
        enteredCode = new List<char>(code.Count);
        display = gameObject.GetComponentInChildren<KeyPadLight>();
        asl = gameObject.GetComponent<ASLObject>();
        asl._LocallySetFloatCallback(floatCallback);
    }

    /*
     * Method that is called everytime sendFloatArray is called.
     * Syncs up the enteredCode for all players
     */
    private void floatCallback(string _id, float[] f)
    {
        //if the first float is not 0 then cancel
        if (f[0] != 0)
        {
            cancel = true;
        }
        //if second float is not 0 then submit the code
        else if (f[1] != 0)
        {
            enter = true;
        }
        //sync up the entered code
        else
        {
            int length = checkASLFloat(f);
            for (int i = 0; i < length; i++)
            {
                char codeChar = (char)(f[i + 2] + 48);
                enteredCode[i] = codeChar;
            }
        }
    }

    /*
     * returns how many characters are entered in the float array
     * checks to see if index 2-5 are not -1
     * returns how many non '-1' values are found
     * when a -1 is found or when iterated through the end
     */
    private int checkASLFloat(float[] f)
    {
        int toReturn = 0;
        for (int i = 0; i < 4; i++)
        {
            if(f[2+i] != -1)
            {
                toReturn++;
            }
            else
            {
                break;
            }
        }
        return toReturn;
    }

    // Update is called once per frame
    void Update()
    {
        //if entered code is submitted
        if (enter)
        {
            //if the lengths don't match then code is incorrect
            if (enteredCode.Count == code.Count)
            {
                bool test = true;
                //check all chars in entered and master code
                for (int i = 0; i < enteredCode.Count; i++)
                {
                    //if one char is not correct then entered code is incorrect
                    if (enteredCode[i] != code[i])
                    {
                        //no real need to set test to false because we break out of loop
                        test = false;
                        //set display
                        display.setIncorrect();
                        break;
                    }
                }
                //if correct code is entered
                if (test)
                {
                    //toogle door and set display
                    unlocked = !unlocked;
                    if (unlocked)
                        door.open();
                    else
                        door.close();
                    display.setCorrect();
                }
            }
            //clear and toggle enter
            enter = false;
            enteredCode.Clear();
        }
        //if canceled
        else if (cancel)
        {
            //clear and toggle cancel
            enteredCode.Clear();
            cancel = false;
        }
    }

    /*
     * Submit entered code
     */
    public void submit()
    {
        asl.SendAndSetClaim(_submit, 20, false);
    }

    private void _submit()
    {
        enter = true;
        asl.SendFloatArray(getFloatCode());
    }
    /*
     * Clear entered Code
     */
    public void canceled()
    {
        asl.SendAndSetClaim(_canceled, 20, false);
    }

    private void _canceled()
    {
        cancel = true;
        asl.SendFloatArray(getFloatCode());
    }

    /*
     * Interface for adding a char to the entered code
     */
    public void enterChar(char c)
    {
        toEnter = c;
        asl.SendAndSetClaim(_enterChar, 20, true);
    }

    private void _enterChar()
    {
        if (enteredCode.Count < 4)
        {
            enteredCode.Add(toEnter);
            asl.SendFloatArray(getFloatCode());
        }
    }

    /*
     * returns the set array of floats that indicate if the enteredCode
     * is submitted, canceled, or to sync up entered code with all players
     */
    private float[] getFloatCode()
    {
        float[] toReturn = new float[6];
        if(enter)
        {
            toReturn[1] = 1;
        }
        if(cancel)
        {
            toReturn[0] = 1;
        }
        int length = enteredCode.Count;
        for(int i = 0; i < length; i++)
        {
            toReturn[i + 2] = enteredCode[i] - '0';
        }
        for(int i = length; i < 4; i++)
        {
            toReturn[2 + i] = -1;
        }
        return toReturn;
    }

    /*
     * Returns a string of the entered code
     */
    public string getCode()
    {
        string s = "";
        for(int i = 0; i < enteredCode.Count; i++)
        {
            s += enteredCode[i];
        }
        return s;
    }
}