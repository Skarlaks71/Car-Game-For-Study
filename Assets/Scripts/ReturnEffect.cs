using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnEffect : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRot;
    private Vector3 fallPoint;
    private bool isAbovePlane;
    private int numPoints = 50;
    private Vector3[] positions = new Vector3[50];
    private bool canDrawWay = false;

    [Header("Return Attributes")]
    public float speedReturn;
    public float speedRotReturn;
    public LineRenderer wayLine;
    public Transform cubeR;
    public GameObject camera;
    public Transform anchorPoint1;
    public Transform anchorPoint2;
    public bool returnViaEditor = false;


    private void Start()
    {

        initialPos = transform.position;
        initialRot = transform.rotation;



        wayLine.positionCount = numPoints;

    }
    IEnumerator RestoredToInitialPos()
    {
        Vector3 returnPos;
        //float totalDistance = (initialPos - transform.position).magnitude;
        float moveSpeed = 0;
        int indexNum = 0;
        //transform.rotation = Quaternion.LookRotation(initialRot.eulerAngles);
        while (Vector3.Distance(transform.position, initialPos) > .5f)
        {
            indexNum = (int)moveSpeed;
            moveSpeed += speedReturn / Vector3.Distance(wayLine.GetPosition(indexNum), wayLine.GetPosition(indexNum + 1));

            transform.position = Vector3.Lerp(wayLine.GetPosition(indexNum), wayLine.GetPosition(indexNum + 1), moveSpeed - indexNum);

            //Quaternion toRot = Quaternion.LookRotation(wayLine.GetPosition(indexNum + 1) - wayLine.GetPosition(indexNum));

            transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRot, speedRotReturn * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        Quaternion toRotEnd = Quaternion.LookRotation(initialPos - wayLine.GetPosition(indexNum));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotEnd, speedRotReturn * Time.deltaTime);
        wayLine.enabled = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = false;
        //camera.SendMessage("ChangeValueForAngle");
        StopCoroutine("RestoredToInitialPos");
        //yield return null;
    }

    IEnumerator WaitForReturn(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine("RestoredToInitialPos");
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
            wayLine.enabled = true;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
            
            anchorPoint1.position = new Vector3(transform.position.x, Vector3.up.y * 5, transform.position.z);
            anchorPoint2.position = new Vector3(initialPos.x, initialPos.y + 5, initialPos.z);

            DrawQuadraticCurve();

        }
    }

    public void DrawQuadraticCurve()
    {

        float t = 0;
        for (int i = 1; i < numPoints + 1; i++)
        {
            t = i / (float)numPoints;
            positions[i - 1] = CalculateCubicBezierCurve(t, transform.position, anchorPoint1.position, anchorPoint2.position, initialPos);
        }

        wayLine.SetPositions(positions);
        wayLine.transform.position = ((transform.position - initialPos) / 2 - (transform.position - initialPos) / 2) / 2;

        if (returnViaEditor == false)
        {
            StartCoroutine("WaitForReturn", 1.5f);
        }
        //
    }

    private Vector3 CalculateCubicBezierCurve(float t, Vector3 from, Vector3 anchor1, Vector3 anchor2, Vector3 end)
    {
        //B(t) = (1-t)^3b0  + 3t(1-t)^2b1 + 3t^2(1-t)b2 + t^3b3
        Vector3 b;
        b = Mathf.Pow((1 - t), 3) * from +
            3 * t * Mathf.Pow((1 - t), 2) * anchor1 +
            3 * Mathf.Pow(t, 2) * (1 - t) * anchor2 +
            Mathf.Pow(t, 3) * end;
        return b;
    }
}
