using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * This class is to be combined with the KeyPad
 * It is the light that gives the user feeback if the code is correct of 
 * incorrect upon submission of a code
 * =================================
 * Author:     Isaiah Snow
 * Assignment: CSS 499
 * Date:       5/7/2020
 * Group:      VREscapeRoom
 * =================================
 */
public class KeyPadLight : MonoBehaviour
{
    //Material for correct submission
    [SerializeField] private Material correct;
    //Material for incorrect submission
    [SerializeField] private Material incorrect;
    [SerializeField] private Material defaultMat;
    private MeshRenderer render;
    //Int to track which material is used, 0 = default, 1 = correct, 3 = incorrect
    private int material = 0;
    //How long the light will stay on before changing back to default material
    [SerializeField] private float time = 5.0f;
    private float counter = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        render = gameObject.GetComponent<MeshRenderer>();
        counter = time;
    }

    // Update is called once per frame
    void Update()
    {
        //If the active material is not the default
        if(material != 0)
        {
            //decrement counter
            counter -= Time.deltaTime;
            //if counter is below 0, switch to default material and reset time
            if(counter <= 0.0f)
            {
                material = 0;
                render.material = defaultMat;
                counter = time;
            }
        }
    }

    //interface to set correct material
    public void setCorrect()
    {
        material = 1;
        counter = time;
        render.material = correct;
    }

    //interface to set incorrect material
    public void setIncorrect()
    {
        material = 2;
        counter = time;
        render.material = incorrect;
    }
}
