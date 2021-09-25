using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class CameraFollow : MonoBehaviour
{
    public Transform car;
    [HideInInspector]
    public bool changeAngle = false;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.position;
        //initialPos.y += 10;
    }
    private void FixedUpdate()
    {
        if (!changeAngle)
        {
            transform.position = new Vector3(car.position.x, transform.position.y, car.position.z);
        }
        else
        {
            if (Vector3.Distance(transform.position, initialPos) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPos, 7f * Time.deltaTime);
                GetComponent<Camera>().fieldOfView = 60;
            }
            else
            {
                //GetComponent<Camera>().fieldOfView = 31;
                //ChangeValueForAngle();
            }
            
            //transform.position = new Vector3(car.position.x, transform.position.y, car.position.z);
        }
        //transform.rotation = 

    }

    public void ChangeValueForAngle()
    {
        changeAngle = !changeAngle;
    }
}
