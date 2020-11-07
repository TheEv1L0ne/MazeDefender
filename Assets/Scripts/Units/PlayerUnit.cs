using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public override void AdjustCamera()
    {
        CameraManager.Instance.UpdateCameraPos(this.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PLAYER HIT T");
    }
}
