using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera _mainCamera;

    public Camera MainCamera { get => _mainCamera;}

    float left;
    float top;
    float right;
    float bottom;


    public void InitCameraAtLocation(Vector3 playerPos, int mazeX, int mazeY)
    {
        float y = _mainCamera.orthographicSize;
        float x = _mainCamera.aspect * _mainCamera.orthographicSize;

        left = x;
        top = -y;
        right = mazeX - x;
        bottom = -mazeY + y;

        Debug.Log($"left {left} top {top} right {right} bottom {bottom}");

        UpdateCameraPos(playerPos);
    }

    public void UpdateCameraPos(Vector3 playerPos)
    {
        _mainCamera.transform.position = new Vector3
         (
            Mathf.Clamp(playerPos.x, left, right),
            Mathf.Clamp(playerPos.y, bottom, top),
            _mainCamera.transform.position.z
        );
    }
}
