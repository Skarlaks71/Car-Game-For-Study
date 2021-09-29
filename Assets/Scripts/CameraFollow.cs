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
    private float yPos;

    private void Start()
    {
        initialPos = transform.position;
        
        yPos = (car.position - transform.position).magnitude;
        StartCoroutine("FollowCar");
    }
    
    IEnumerator FollowCar()
    {
        Debug.Log("Follow Car");
        while (true)
        {
            
            transform.position = new Vector3(car.position.x, car.position.y+yPos, car.position.z);
            yield return new WaitForSeconds(.001f);
        }
        
    }
    /*IEnumerator LookReturn()
    {
        Debug.Log("look return");
        Vector3 lookPos = new Vector3(transform.position.x,transform.position.y+5,transform.position.z);
        //transform.position = Vector3.MoveTowards(transform.position, lookPos, 0.1f * Time.deltaTime);
        *//*while (Vector3.Distance(car.position, initialPos) > 1f)
        {
            Quaternion.LookRotation(car.position);
            yield return new WaitForSeconds(.01f);
        }*//*
        if(Vector3.Distance(car.position, initialPos) < 2f)
        {
            ChangeValueForAngle();
        }
        yield return new WaitForSeconds(.01f);
    }*/
    /*public void ChangeValueForAngle()
    {
        Debug.Log(changeAngle);
        changeAngle = !changeAngle;

        if (changeAngle)
        {
            StopCoroutine("LookReturn");
            StopAllCoroutines();
            StartCoroutine("FollowCar");

        }
        else
        {
            StopCoroutine("FollowCar");
            StartCoroutine("LookReturn");
        }

    }*/
}
