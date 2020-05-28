using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.Versioning;
using Microsoft.MixedReality.Toolkit;
using UnityEngine;
using ASL;

public class PlaneManipulator : MonoBehaviour
{
    List<Transform> planes = new List<Transform>();
    List<Vector3> positions = new List<Vector3>();

    private bool movable;
    private Transform selected = null;
    private Transform[] unselectable = new Transform[2];
    private Microsoft.MixedReality.Toolkit.Input.IMixedRealityPointer selectedPointer = null;
    private float pointerZ;
    private bool rotatable = true;
    private bool done = false;
    private Vector3 initialPos;
    private float timer;
    private bool shuffled;

    [SerializeField] private GameObject border;
    [SerializeField] private GameObject tilePuzzle;


    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.position;

        movable = true;

        shuffled = false;

        foreach (Transform child in transform)
        {
            planes.Add(child);
            positions.Add(child.position);
        }

        //for (int i = 0; i < planes.Count; i++)
        //{
        //    UnityEngine.Debug.Log(planes[i]);
        //    UnityEngine.Debug.Log(planes[i].position);
        //    UnityEngine.Debug.Log(positions[i]);
        //}



        



        //for (int i = 0; i < planes.Count; i++)
        //{
        //    UnityEngine.Debug.Log(planes[i]);
        //    UnityEngine.Debug.Log(planes[i].position);
        //    UnityEngine.Debug.Log(positions[i]);
        //}
        Debug.Log(planes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //if(ASL.GameLiftManager.GetInstance().m_PeerId == 1 && timer < 0.5f)
        //{
        //    Shuffle();
        //    timer += Time.deltaTime;
        //}

        if (selected != null && selectedPointer != null && Vector3.Distance(selectedPointer.Position,selected.position) > 0.5f)
        {
            deselect();
        }

        if (selectedPointer != null)
        {
            var currentZ = selectedPointer.Rotation.eulerAngles.z;


            Debug.Log(currentZ);
            Debug.Log(pointerZ);

            if (currentZ > 80f && currentZ < 100f && rotatable)
            {
                rotate("R");
                rotatable = false;
            }
            else if (currentZ > 260f && currentZ < 280f && rotatable)
            {
                rotate("L");
                rotatable = false;
            }
            else if ((currentZ < 10f && currentZ > 0f) || (currentZ < 360f && currentZ > 350f))
            {
                rotatable = true;
            }
        }

        selectPlane();

        if(selected != null)
        {
            border.active = true;
            border.transform.position = new Vector3(selected.position.x - 0.001f, selected.position.y, selected.position.z);
        }
        else
        {
            border.active = false;
        }

        bool completed = true;
        int num = 0;
        while(completed && (num < planes.Count))
        {
            bool v = planes[num].GetComponent<PlaneBehaviour>().isInCorrectPosition() && planes[num].GetComponent<PlaneBehaviour>().isInCorrectDirection();
            completed = v;

            num++;
        }

        if(timer < 0.5f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (completed)
            {
                Completed();
            }
        }
    }

    float Zcalculation(float z, float delta)
    {
        z += delta;
        if(z >= 360f)
        {
            z -= 360f;
        }
        else if(z < 0f)
        {
            z += 360f;
        }

        return z;
    }

    void Shuffle()
    {
        for (int i = 0; i < planes.Count; i++)
        {
            int rnd = UnityEngine.Random.Range(i, planes.Count);
            //var tempGO = planes[rnd];
            //planes[rnd] = planes[i];
            //planes[i] = tempGO;
            int temp = planes[rnd].GetComponent<PlaneBehaviour>().getCurrentIndex();
            planes[rnd].GetComponent<PlaneBehaviour>().setCurrentIndex(planes[i].GetComponent<PlaneBehaviour>().getCurrentIndex());
            planes[i].GetComponent<PlaneBehaviour>().setCurrentIndex(temp);

            //planes[i].position = positions[temp];
            planes[i].gameObject.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                planes[i].gameObject.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(positions[temp]);
            });
        }
    }

    public void selectPlane()
    {
        if(movable)
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
                        if (p.Result != null)
                        {

                            var startPoint = p.Position;
                            var endPoint = p.Result.Details.Point;
                            Vector3 normalV = (endPoint - startPoint).normalized;
                            float distance = Vector3.Distance(endPoint, startPoint);
                            var hitObject = p.Result.Details.Object;

                            RaycastHit hit;
                            // Does the ray intersect any objects excluding the player layer
                            //if (Physics.Raycast(startPoint, transform.TransformDirection(normalV), out hit, Mathf.Infinity))
                            //{
                            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                            //    UnityEngine.Debug.Log("Called 4");

                            //    if (planes.Contains(hit.transform))
                            //    {
                            //        UnityEngine.Debug.Log("Called 5: " + hit.transform);

                            //        if (selected == null)
                            //        {
                            //            selected = hit.transform;
                            //        }
                            //        else
                            //        {
                            //            swap(hit.transform);
                            //            break;
                            //        }


                            //    }
                            //    else
                            //    {
                            //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                            //    }
                            //}

                            if (hitObject)
                            {

                                if (planes.Contains(p.Result.CurrentPointerTarget.transform) && distance < 0.1f)
                                {
                                    //UnityEngine.Debug.Log("Called 5: " + p.Result.CurrentPointerTarget);

                                    var obj = p.Result.CurrentPointerTarget.transform;

                                    if (selected == null && selectedPointer == null && !Array.Exists(unselectable, element => element == obj))
                                    {
                                        selected = obj;
                                        selectedPointer = p;
                                        Debug.Log(selected);
                                    }
                                    //else if(p != selectedPointer)
                                    //{
                                    //    swap(obj);
                                    //    break;
                                    //}


                                }
                            }
                        }

                    }
                }
            }
        }
    }

    public void rotate(string str)
    {
        if(selected != null && movable)
        {
            if(str == "L")
            {
                Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
                selected.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    selected.GetComponent<ASL.ASLObject>().SendAndIncrementWorldRotation(rotation);
                });
                //selected.Rotate(0.0f, 90.0f, 0.0f);
            }
            else if(str == "R")
            {
                //selected.Rotate(0.0f, - 90.0f, 0.0f);
                Quaternion rotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
                selected.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
                {
                    selected.GetComponent<ASL.ASLObject>().SendAndIncrementWorldRotation(rotation);
                });
            }

            unselectable[0] = null;
            unselectable[1] = null;
        }
    }

    public void debug()
    {
        for (int i = 0; i < planes.Count; i++)
        {
            UnityEngine.Debug.Log(planes[i]);
            UnityEngine.Debug.Log("Plane in the currect position: ");
            UnityEngine.Debug.Log(planes[i].GetComponent<PlaneBehaviour>().isInCorrectPosition());
            UnityEngine.Debug.Log("Plane in the currect direction: ");
            UnityEngine.Debug.Log(planes[i].GetComponent<PlaneBehaviour>().isInCorrectDirection());
        }
    }

    public void deselect()
    {
        selected = null;
        selectedPointer = null;
        unselectable[0] = null;
        unselectable[1] = null;
    }

    void swap(Transform T)
    {
        Debug.Log("Swap");
        if(movable)
        {
            Debug.Log("Swap 2");
            var index = T.GetComponent<PlaneBehaviour>().getCurrentIndex();
            var pos = T.position;

            Debug.Log(planeIndex(selected));
            Debug.Log(planeIndex(T));

            T.GetComponent<PlaneBehaviour>().setCurrentIndex(selected.GetComponent<PlaneBehaviour>().getCurrentIndex());
            //T.position = selected.position;
            T.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                T.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(selected.position);
            });

            selected.GetComponent<PlaneBehaviour>().setCurrentIndex(index);
            //selected.position = pos;
            selected.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                selected.GetComponent<ASL.ASLObject>().SendAndSetWorldPosition(pos);
            });

            unselectable[0] = selected;
            unselectable[1] = T;
            selected = null;
            selectedPointer = null;
        }
    }

    void Completed()
    {
        movable = false;
        border.active = false;
        //border.transform.localScale = new Vector3(4.0f, 1.0f, 4.0f);

        //var x = (planes[1].position.x + planes[2].position.x)/ 2;
        //var y = (planes[1].position.y + planes[5].position.y) / 2;

        //border.transform.position = new Vector3(x, y, planes[0].position.x - 0.001f);

        var pos = this.transform.position;

        if (pos.z > initialPos.z - 2f)
        {
            //this.transform.position -= new Vector3(0, 0, Time.deltaTime);

            tilePuzzle.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                tilePuzzle.GetComponent<ASL.ASLObject>().SendAndIncrementWorldPosition(new Vector3(0, 0, -Time.deltaTime));
            });
        }
        else
        {
            done = true;
            //Destroy(tilePuzzle);
        }


       //UnityEngine.Debug.Log("Good job");
    }

    int planeIndex(Transform T)
    {
        return planes.IndexOf(T);
    }
}
