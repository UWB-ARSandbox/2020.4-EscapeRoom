using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using ASL;

public class PlaneBehaviour : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private int currentIndex;
    float DefaultRotationY;
    float timer;

    void Awake()
    {
        DefaultRotationY = this.transform.rotation.eulerAngles.y;
        currentIndex = id;
    }

    // Start is called before the first frame update
    void Start()
    {
        


        //Debug.Log(this.transform.rotation.eulerAngles.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer < 0.5f)
        {
            int randInt = UnityEngine.Random.Range(0, 4);

            //this.transform.Rotate(0.0f, 90.0f * randInt, 0.0f);

            Quaternion rotation = Quaternion.Euler(0, 90 * randInt, 0);
            this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                this.GetComponent<ASL.ASLObject>().SendAndIncrementWorldRotation(rotation);
            });

            timer += Time.deltaTime;
        }
    }

    public int getID() { return id; }

    public void setCurrentIndex(int i)
    {
        currentIndex = i;
    }

    public int getCurrentIndex()
    {
        return currentIndex;
    }

    public bool isInCorrectDirection()
    {
        return Mathf.Abs(this.transform.rotation.eulerAngles.y % 360) == DefaultRotationY;
    }

    public bool isInCorrectPosition()
    {
        return currentIndex == id;
    }
}
