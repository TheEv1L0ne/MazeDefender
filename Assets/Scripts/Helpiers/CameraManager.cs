﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera _mainCamera;

    public Camera MainCamera { get => _mainCamera;}

    private float _left;
    private float _top;
    private float _right;
    private float _bottom;


    public void InitCameraAtLocation(Vector3 playerPos, int mazeX, int mazeY)
    {
        float y = _mainCamera.orthographicSize;
        float x = _mainCamera.aspect * _mainCamera.orthographicSize;

        _left = x;
        _top = -y;
        _right = mazeX - x;
        _bottom = -mazeY + y;

        Debug.Log($"left {_left} top {_top} right {_right} bottom {_bottom}");

        UpdateCameraPos(playerPos);
    }

    public void UpdateCameraPos(Vector3 playerPos)
    {
        _mainCamera.transform.position = new Vector3
         (
            Mathf.Clamp(playerPos.x, _left, _right),
            Mathf.Clamp(playerPos.y, _bottom, _top),
            _mainCamera.transform.position.z
        );
    }
}
