using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using ASL;

public class InventoryController : MonoBehaviour
{
    //    public InventorySystem inventory;
    private Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointer leftPointer;
    private Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointer rightPointer;

    List<GameObject> leftInventory = new List<GameObject>();
    List<GameObject> rightInventory = new List<GameObject>();
    GameObject leftItem;
    GameObject rightItem;

    // Start is called before the first frame update
    void Start()
    {
        if (leftPointer == null && rightPointer == null)
        {
            foreach (var source in MixedRealityToolkit.InputSystem.DetectedInputSources)
            {
                // Ignore anything that is not a hand because we want articulated hands
                if (source.SourceType == Microsoft.MixedReality.Toolkit.Input.InputSourceType.Controller)
                {
                    foreach (var p in source.Pointers)
                    {
                        if (p is Microsoft.MixedReality.Toolkit.Input.IMixedRealityNearPointer)
                        {
                            // Ignore near pointers, we only want the rays
                            continue;
                        }
                        else
                        {
                            if (rightPointer == null)
                            {
                                rightPointer = p;
                            }
                            else
                            {
                                leftPointer = p;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (leftPointer == null && rightPointer == null)
        {
            foreach (var source in MixedRealityToolkit.InputSystem.DetectedInputSources)
            {
                // Ignore anything that is not a hand because we want articulated hands
                if (source.SourceType == Microsoft.MixedReality.Toolkit.Input.InputSourceType.Controller)
                {
                    foreach (var p in source.Pointers)
                    {
                        if (p is Microsoft.MixedReality.Toolkit.Input.IMixedRealityNearPointer)
                        {
                            // Ignore near pointers, we only want the rays
                            continue;
                        }
                        else
                        {
                            if (rightPointer == null)
                            {
                                rightPointer = p;
                            }
                            else
                            {
                                leftPointer = p;
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (leftInventory.Count > 0)
        {
            foreach (var obj in leftInventory)
            {
                //obj.transform.position = leftPointer.Position;
                obj.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    obj.GetComponent<ASL.ASLObject>().SendAndSetLocalPosition(leftPointer.Position);
                });

                //obj.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                //{
                //    obj.GetComponent<ASL.ASLObject>().SendAndIncrementWorldRotation(leftPointer.Rotation);
                //});


                if (obj == leftItem)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }

        if (rightInventory.Count > 0)
        {
            foreach (var obj in rightInventory)
            {
                //obj.transform.position = rightPointer.Position;
                obj.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    obj.GetComponent<ASL.ASLObject>().SendAndSetLocalPosition(rightPointer.Position);
                });

                //obj.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                //{
                //    obj.GetComponent<ASL.ASLObject>().SendAndIncrementWorldRotation(rightPointer.Rotation);
                //});

                if (obj == rightItem)
                {
                    obj.SetActive(true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }

        if(leftItem != null && leftItem.GetComponent<PickableBehaviour>() != null && leftItem.GetComponent<PickableBehaviour>().isClose())
        {
            leftItem.GetComponent<PickableBehaviour>().setItemToPos();
            leftInventory.Remove(leftItem);
            if(leftInventory.Count > 0)
            {
                leftItem = leftInventory[0];
            }
            else
            {
                leftItem = null;
            }
        }

        if (rightItem != null && rightItem.GetComponent<PickableBehaviour>() != null && rightItem.GetComponent<PickableBehaviour>().isClose())
        {
            rightItem.GetComponent<PickableBehaviour>().setItemToPos();
            rightInventory.Remove(rightItem);
            if (rightInventory.Count > 0)
            {
                rightItem = rightInventory[0];
            }
            else
            {
                rightItem = null;
            }
        }
    }

    public void grabItem(string str)
    {
        if (str == "L")
        {
            UnityEngine.Debug.Log("Called 3");
            if (leftPointer.Result != null)
            {
                UnityEngine.Debug.Log("Called 4");
                var startPoint = leftPointer.Position;
                var endPoint = leftPointer.Result.Details.Point;
                Vector3 normalV = (endPoint - startPoint).normalized;
                float distance = Vector3.Distance(endPoint, startPoint);
                var hitObject = leftPointer.Result.Details.Object;

                if (hitObject)
                {
                    GameObject hitItem = leftPointer.Result.CurrentPointerTarget;

                    UnityEngine.Debug.Log("Hit object: " + hitItem);

                    if (hitItem.tag == "Pickable")
                    {
                        UnityEngine.Debug.Log("leftPointer: " + hitItem);


                        if (!leftInventory.Contains(hitItem) && !rightInventory.Contains(hitItem) && leftInventory.Count < 2)
                        {
                            leftInventory.Add(hitItem);

                            if (leftItem == null)
                            {
                                leftItem = hitItem;
                            }
                            else
                            {
                                hitItem.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
        else if (str == "R")
        {
            UnityEngine.Debug.Log("Called 3");
            if (rightPointer.Result != null)
            {
                UnityEngine.Debug.Log("Called 4");
                var startPoint = rightPointer.Position;
                var endPoint = rightPointer.Result.Details.Point;
                Vector3 normalV = (endPoint - startPoint).normalized;
                float distance = Vector3.Distance(endPoint, startPoint);
                var hitObject = rightPointer.Result.Details.Object;

                if (hitObject)
                {
                    GameObject hitItem = rightPointer.Result.CurrentPointerTarget;

                    UnityEngine.Debug.Log("Hit object: " + hitItem);

                    if (hitItem.tag == "Pickable")
                    {
                        UnityEngine.Debug.Log("rightPointer: " + hitItem);


                        if (!leftInventory.Contains(hitItem) && !rightInventory.Contains(hitItem) && rightInventory.Count < 2)
                        {
                            rightInventory.Add(hitItem);

                            if (rightItem == null)
                            {
                                rightItem = hitItem;
                            }
                            else
                            {
                                hitItem.SetActive(false);
                            }
                        }
                    }
                }
            }
        }
    }

    public void switchItem(string str)
    {
        if(str == "L")
        {
            if(leftInventory.Count > 1)
            {
                int index = leftInventory.IndexOf(leftItem);
                if (index == 0)
                {
                    leftItem = leftInventory[index + 1];
                }
                else
                {
                    leftItem = leftInventory[index - 1];
                }
            }
        }
        else if(str == "R")
        {
            if (rightInventory.Count > 1)
            {
                int index = rightInventory.IndexOf(rightItem);
                if (index == 0)
                {
                    rightItem = rightInventory[index + 1];
                }
                else
                {
                    rightItem = rightInventory[index - 1];
                }
            }
        }
    }

    public string[] getCurrentItem()
    {
        string left, right;
        string[] result = new string[2];

        if(leftItem == null)
        {
            left = "No Item";
        }
        else
        {
            left = leftItem.name;
        }

        if (rightItem == null)
        {
            right = "No Item";
        }
        else
        {
            right = rightItem.name;
        }

        result[0] = left;
        result[1] = right;

        return result;
    }
}
