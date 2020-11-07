using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera _mainCamera;

    [SerializeField] private Camera _uiCamera;

    public Camera MainCamera { get => _mainCamera;}
    public Camera UiCamera { get => _uiCamera;}

    private float _left;
    private float _top;
    private float _right;
    private float _bottom;


    public void InitCameraAtLocation((int, int) atIndex)
    {
        float y = _mainCamera.orthographicSize;
        float x = _mainCamera.aspect * _mainCamera.orthographicSize;

        float mazeX = MazeManager.Instance.Maze.MazeSizeX;
        float mazeY = MazeManager.Instance.Maze.MazeSizeY;

        Vector3 pos = MazeManager.Instance.Maze.mazeMatrix[atIndex.Item1, atIndex.Item2].NodePosition;

        _left = x;
        _top = -y;
        _right = mazeX - x;
        _bottom = -mazeY + y;

        Debug.Log($"left {_left} top {_top} right {_right} bottom {_bottom}");

        UpdateCameraPos(pos);
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
