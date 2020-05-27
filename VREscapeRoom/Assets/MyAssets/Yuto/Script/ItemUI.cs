using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Camera cam;
    public TextMeshPro left;
    public TextMeshPro right;
    public InventoryController inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        left.SetText(inventory.getCurrentItem()[0]);
        right.SetText(inventory.getCurrentItem()[1]);

        this.transform.position = cam.transform.position + cam.transform.forward * 2f;
        this.transform.rotation = cam.transform.rotation;
    }
}
