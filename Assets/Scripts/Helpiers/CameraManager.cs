using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private Camera _mainCamera;

    public Camera MainCamera { get => _mainCamera;}
}
