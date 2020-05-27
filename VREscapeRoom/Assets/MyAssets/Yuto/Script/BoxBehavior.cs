using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehavior : MonoBehaviour
{
    public GameObject lid;
    public GameObject objInside;
    public bool open;

    public Vector3 defaultAngle;
    public Vector3 targetAngle;


    private Vector3 currentAngle;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
        objInside.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - this.transform.localScale.y * 0.5f, this.transform.position.z);
        objInside.SetActive(false);
        //defaultAngle = lid.transform.eulerAngles;
        currentAngle = lid.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        open = GetComponent<PipeInterface>().isOn();

        if (open)
        {
            currentAngle = new Vector3(
                            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
                            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
                            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

            lid.transform.eulerAngles = currentAngle;

            objInside.SetActive(true);
        }
        else
        {
            //currentAngle = new Vector3(
            //                Mathf.LerpAngle(currentAngle.x, defaultAngle.x, Time.deltaTime),
            //                Mathf.LerpAngle(currentAngle.y, defaultAngle.y, Time.deltaTime),
            //                Mathf.LerpAngle(currentAngle.z, defaultAngle.z, Time.deltaTime));

            //lid.transform.eulerAngles = currentAngle;
        }
    }
}