using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController_R : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject playspace;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed;
    //[SerializeField] private float maxSpeed;
    //private float maxSpeedSquared;
    private Vector3 moveDirection;
    private float _xInput;
    private float _zInput;

    void Start()
    {
        if (rigidbody == null || rigidbody != this.transform.GetComponent<Rigidbody>())
        {
            rigidbody = this.transform.GetComponent<Rigidbody>();
        }
        Debug.Assert(rigidbody != null);
        rigidbody.freezeRotation = true;
        Debug.Assert(speed > 0);
        //Debug.Assert(playspace != null);
        //maxSpeedSquared = maxSpeed * maxSpeed;
    }

    public void Initialize(GameObject wmrPlayspace, GameObject wmrCamera)
    {
        playspace = wmrPlayspace;
        cam = wmrCamera;
        Debug.Assert(playspace != null);
        Debug.Assert(cam != null);
    }

    void Update()
    {
        _xInput = Input.GetAxis("AXIS_17");
        _zInput = Input.GetAxis("AXIS_18");
    }

    void FixedUpdate()
    {
        if (_xInput > 0.1f || _xInput < -0.1f
            || _zInput > 0.1f || _zInput < -0.1f)
        {
            //Debug.Log("CamDirectionForward: " + cam.transform.forward);
            //Debug.Log("CamDirectionRight: " + cam.transform.right);

            Vector3 camF = cam.transform.forward;
            Vector3 camR = cam.transform.right;
            camF.y = 0;
            camR.y = 0;
            camF = camF.normalized;
            camR = camR.normalized;

            moveDirection = camF * _zInput * -1.0f + camR * _xInput;

            //Debug.Log("MoveDirection = " + moveDirection);

            // Move rigidbody using MovePosition();
            //rigidbody.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);

            this.GetComponent<ASL.ASLObject>().SendAndSetClaim(() =>
            {
                this.GetComponent<ASL.ASLObject>().SendAndIncrementWorldPosition(moveDirection * speed * Time.fixedDeltaTime);

            });
        }

        // Ensure playspace follows player object
        playspace.transform.position = this.transform.position;
    }
}

