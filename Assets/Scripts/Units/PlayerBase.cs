using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Vector3 basePosition => this.transform.position;
    public int baseHitPoints;
    public bool IsAlive => baseHitPoints > 0;
    
    public void Init(int withHp, (int, int) atIndex)
    {
        baseHitPoints = withHp;
        MazeManager.Instance.InitBase(atIndex);
    }

    public void TakeDamage(int damage)
    {
        baseHitPoints -= damage;
    }

    public void DestroyBase()
    {
        //Play some animations here
    }
}
