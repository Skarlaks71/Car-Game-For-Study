using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform car;

    private void FixedUpdate()
    {
        transform.position = new Vector3(car.position.x, transform.position.y, car.position.z);
        //transform.rotation = 
    }
}
