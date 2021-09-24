using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera currentCamera;

    public Camera mainCamera;
    public Camera frontCamera;
    public Camera backCamera;

    private void Start()
    {
        currentCamera = mainCamera;
        frontCamera.enabled = false;
        backCamera.enabled = false;
    }

    void Update()
    {
        SwitchCameras();
    }

    void SwitchCameras()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            mainCamera.enabled = true;
            if (currentCamera != mainCamera)
                currentCamera.enabled = false;
            currentCamera = frontCamera;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            frontCamera.enabled = true;
            if (currentCamera != frontCamera)
                currentCamera.enabled = false;
            currentCamera = frontCamera;

        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            backCamera.enabled = true;
            if (currentCamera != backCamera)
                currentCamera.enabled = false;
            currentCamera = backCamera;

        }
    }
}
