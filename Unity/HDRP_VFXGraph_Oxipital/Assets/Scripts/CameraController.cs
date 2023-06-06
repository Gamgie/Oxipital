﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public enum CameraMovementType { Orbital, Spaceship, Freefly }

    [Header("General Parameters")]
    public Transform lookAtTarget;
    public new Camera camera;
    public float fov;
    public Vector3 positionTarget;
    public float followZOffset;
    public float resetDuration;
    public CameraMovementType cameraType;

    public OrbitalMovement orbitalCamera;
    public SpaceshipMovement spaceshipCamera;
    //public CinemachineVirtualCamera freeflyCamera;

    
    private Camera _cameraFeedback;
    private List<CameraMovement> _cameraList;
    private CameraMovement _activeCamera;

    private void OnEnable()
    {
        _cameraFeedback = camera.transform.GetChild(0).GetComponent<Camera>();

        _cameraList = new List<CameraMovement>();
        _cameraList.Add(orbitalCamera);
        _cameraList.Add(spaceshipCamera);

        foreach (CameraMovement c in _cameraList)
        {
            c.Init();
        }

        SwitchCamera(cameraType);
    }

    // Update is called once per frame
    void Update()
    {
        if(_activeCamera != null && cameraType != _activeCamera.type)
		{
            SwitchCamera(cameraType);
		}

        // Update our active camera
        foreach (CameraMovement c in _cameraList)
        {
            if(c != null && c.isActive)
			{             
                c.UpdateMovement();
                c.UpdateZOffset(followZOffset);
                c.UpdateFOV(fov);
            }
        }

        // update feedback camera FOV
        if (_cameraFeedback != null)
        {
            _cameraFeedback.fieldOfView = fov;
        }
    }

    void SwitchCamera(CameraMovementType type)
	{
        // Deactivate all cameras
        foreach(CameraMovement c in _cameraList)
		{
            c.isActive = false;
		}

        // Activate the selected camera
        switch (type)
		{
            case CameraMovementType.Freefly:
                break;
            case CameraMovementType.Orbital:
                _activeCamera = orbitalCamera;
                _activeCamera.isActive = true;
                break;
            case CameraMovementType.Spaceship:
                break;
        }
	}

    public void ResetCameraPosition()
    {
        foreach (CameraMovement c in _cameraList)
        {
            if (c.isActive)
                c.Reset(resetDuration);
        }
    }

}