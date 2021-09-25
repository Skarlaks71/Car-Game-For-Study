using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class CarController : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
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

    #region ReturnEffect

    private Vector3 initialPos;
    private Quaternion initialRot;
    private Vector3 fallPoint;
    private bool isAbovePlane;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];
    private bool canDrawWay = false;

    [Header("Return Attributes")]
    public float speedReturn;
    public LineRenderer wayLine;
    public Transform cubeR;
    public GameObject camera;

    #endregion

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

    private void Start()
    {
        
        initialPos = transform.position;
        initialRot = transform.rotation;

        

        wayLine.positionCount = numPoints;
        
    }

    private void FixedUpdate()
    {
        GetInput();
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

    IEnumerator RestoredToInitialPos(Vector3 anchorPoint)
    {
        Vector3 returnPos;
        //float totalDistance = (initialPos - transform.position).magnitude;
        float moveSpeed=0;
        int indexNum=0;
        
        while (Vector3.Distance(transform.position,initialPos)>1f)
        {
            indexNum = (int)moveSpeed;
            moveSpeed += speedReturn / Vector3.Distance(positions[indexNum], positions[indexNum + 1]);

            transform.position = Vector3.Lerp(positions[indexNum], positions[indexNum+1], moveSpeed-indexNum);
            transform.rotation = Quaternion.LookRotation(initialRot.eulerAngles);
            
            yield return new WaitForEndOfFrame();
        }
        //transform.position = Vector3.Lerp(transform.position, initialPos, speedReturn * Time.fixedDeltaTime);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        camera.SendMessage("ChangeValueForAngle");
        yield return null;
    }
    IEnumerator RestoredToFallPoint()
    {
        while (transform.position != fallPoint)
        {
            transform.position = Vector3.Lerp(transform.position, fallPoint, speedReturn * Time.fixedDeltaTime);
            //isAbovePlane = false;
            yield return new WaitForSeconds(.05f);
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "FallWall")
        {
            fallPoint = transform.position;
            Debug.Log(other.gameObject.name);
        }

        if (other.gameObject.tag == "OutOfBounds")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            camera.SendMessage("ChangeValueForAngle");
            DrawQuadraticCurve();
            //Gizmos.color = Color.red;
            //DrawQuadraticCurve();

            //StartCoroutine(RestoredToInitialPos());
            Debug.Log(other.gameObject.name);
        }
    }
    
    private void DrawQuadraticCurve()
    {
        Vector3 anchorPoint = new Vector3(transform.position.x, initialPos.y+5, transform.position.z);
        float t = 0;
        for (int i = 1; i < numPoints + 1; i++)
        {
            t =  i / (float)numPoints; //Time.deltaTime * speedReturn;
            positions[i - 1] = CalculateQuadraticBezierCurve(t, transform.position, anchorPoint, initialPos);
        }
        
        wayLine.SetPositions(positions);
        wayLine.transform.position = ((transform.position - initialPos) / 2 - (transform.position - initialPos) / 2)/2;

        StartCoroutine(RestoredToInitialPos(anchorPoint));
    }

    private Vector3 CalculateQuadraticBezierCurve(float t, Vector3 from, Vector3 anchor, Vector3 end)
    {
        //B(t) = (1-t)^2b0 + 2t(1-t)b1 + t^2b2
        Vector3 b;
        b = Mathf.Pow((1 - t), 2) * from +
            2 * t * (1 - t) * anchor +
            Mathf.Pow(t, 2) * end;
        return b;
    }

}
