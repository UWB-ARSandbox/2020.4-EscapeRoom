using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snappable : MonoBehaviour
{
    private bool isSnapped = false;
    [SerializeField] private Transform snappedTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if(this.transform.position != snappedTransform.position && isSnapped)
        {
            this.transform.position = snappedTransform.position;
        }
    }

    public void snap()
    {
        isSnapped = true;
        this.transform.position = snappedTransform.position;
    }
}
