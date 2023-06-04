﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("General Parameters")]
    public Transform lookAtTarget;
    public new Camera camera;
    public float fov;
    public Vector3 positionTarget;
    public float followZOffset;
    public float resetDuration;
    public float moveToDuration;
    public CinemachineVirtualCamera orbitalCamera;
    public CinemachineVirtualCamera spaceshipCamera;
    public CinemachineVirtualCamera freeflyCamera;

    [Header("Orbital")]
    public float rotateYSpeed;
    public float rotateXSpeed;
    public float rotateZSpeed;
    public bool controlRotateWithAngle = false;
    public float rotateYAngle;
    public float rotateXAngle;
    public float rotateZAngle;

    private CinemachineTransposer transposer;
    private Camera _cameraFeedback;
    

    private void OnEnable()
    {
        transposer = orbitalCamera.GetCinemachineComponent<CinemachineTransposer>();
        _cameraFeedback = camera.transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate target transform according to speed
        if(controlRotateWithAngle == false)
        {
            lookAtTarget.Rotate(rotateXSpeed * Time.deltaTime, rotateYSpeed * Time.deltaTime, rotateZSpeed * Time.deltaTime);
            rotateXAngle = lookAtTarget.eulerAngles.x;
            rotateYAngle = lookAtTarget.eulerAngles.y;
            rotateZAngle = lookAtTarget.eulerAngles.z;
        }
        else // We want to control rotation with angle directly
        {
            lookAtTarget.transform.eulerAngles = new Vector3(rotateXAngle, rotateYAngle, rotateZAngle);
        }

        lookAtTarget.transform.position = positionTarget;

        // Control distance between target and camera
        if (transposer != null)
        {
            transposer.m_FollowOffset = new Vector3(0, 0, -followZOffset);
        }

        UpdateFOV();
    }

    void UpdateFOV()
	{
        orbitalCamera.m_Lens.FieldOfView = fov;

        // update feedback camera FOV
        if (_cameraFeedback != null)
        {
            _cameraFeedback.fieldOfView = fov;
        }
    }

    public void ResetCameraPosition()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        //rotateXAngle = 0;
        //rotateYAngle = 0;
        lookAtTarget.transform.DOMove(Vector3.zero,resetDuration);
        lookAtTarget.transform.DORotate(Vector3.zero, resetDuration);
    }

    public void TopView()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        lookAtTarget.transform.DORotate(new Vector3(90,0,0), moveToDuration);
    }

    public void DownView()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        lookAtTarget.transform.DORotate(new Vector3(-90, 0, 0), moveToDuration);
    }

    public void LeftView()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        lookAtTarget.transform.DORotate(new Vector3(0, 90, 0), moveToDuration);
    }

    public void RightView()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        lookAtTarget.transform.DORotate(new Vector3(0, -90, 0), moveToDuration);
    }

    public void FrontView()
    {
        rotateYSpeed = 0;
        rotateXSpeed = 0;
        lookAtTarget.transform.DORotate(new Vector3(0, 0, 0), moveToDuration);
    }

}
