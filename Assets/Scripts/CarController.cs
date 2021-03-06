using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    [SerializeField]
    private float horizontalInput;
    [SerializeField]
    private float accelerateInput;
    private float currentSteerAngle;
    private float currentBreakForce;
    private bool isBreaking;
    public bool isBackTransmission = false;

    //private bool backMode;

    public float motorForce;
    public float breakForce;
    public float maxSteerAngle;
    public float maxFriction;

    [Header("Wheels References")]
    [SerializeField]
    private WheelCollider frontLeftWheelCollider;
    [SerializeField]
    private WheelCollider frontRightWheelCollider;
    [SerializeField]
    private WheelCollider rearLeftWheelCollider;
    [SerializeField]
    private WheelCollider rearRightWheelCollider;

    [SerializeField]
    private Transform frontLeftWheelTransform;
    [SerializeField]
    private Transform frontRightWheelTransform;
    [SerializeField]
    private Transform rearLeftWheelTransform;
    [SerializeField]
    private Transform rearRightWheelTransform;

    private bool canDraw = false;


    private void Start()
    {
        
    }


    private void FixedUpdate()
    {
        //GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        //Debug.Log(isBreaking);
    }

   

    private void HandleMotor()
    {
        rearLeftWheelCollider.motorTorque = accelerateInput * motorForce;
        rearRightWheelCollider.motorTorque = accelerateInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        applyBreaking();
        

    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSngleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSngleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSngleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSngleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSngleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void applyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void GetInput()
    {
        // active rear transmission
        if (Input.GetKeyDown(KeyCode.C))
        {
            isBackTransmission = !isBackTransmission;
        }

        //back to initial position
        if (Input.GetKey(KeyCode.P))
        {
            //StartCoroutine(RestoredToInitialPos());
        }

        horizontalInput = Input.GetAxis(HORIZONTAL);
        if (isBackTransmission)
        {
            accelerateInput = Mathf.Abs(Input.GetAxis(VERTICAL)) * -1f;
        }
        else
        {
            accelerateInput = Input.GetAxis(VERTICAL);
        }
        isBreaking = Input.GetKey(KeyCode.Space);
        
    }
    public void GetExternalInputs(float forwardAxis, float turnAxis)
    {
        accelerateInput = forwardAxis;
        horizontalInput = turnAxis;
    }

    public float GetSpeed()
    {
        return GetComponent<Rigidbody>().velocity.sqrMagnitude;
    }

}
