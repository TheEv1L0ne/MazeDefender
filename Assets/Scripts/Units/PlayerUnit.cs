using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public override void AdjustCamera()
    {
        CameraManager.Instance.UpdateCameraPos(this.transform.position);
    }
}
