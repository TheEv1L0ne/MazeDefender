using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    public Vector3 basePosition => this.transform.position;
    private int _baseHitPoints;
    public bool IsAlive => _baseCurrentHp > 0;

    private int _baseCurrentHp;
    
    public void Init(int withHp, (int, int) atIndex)
    {
        _baseHitPoints = withHp;
        _baseCurrentHp = withHp;
        MazeManager.Instance.InitBase(atIndex);

        UIManager.Instance.playUI.SetBaseHp(_baseCurrentHp, _baseHitPoints);
    }

    public void TakeDamage(int damage)
    {
        _baseCurrentHp -= damage;
        _baseCurrentHp = Mathf.Clamp(_baseCurrentHp, 0, _baseHitPoints);

        UIManager.Instance.playUI.SetBaseHp(_baseCurrentHp, _baseHitPoints);
    }

    public void DestroyBase()
    {
        //Play some animations here
    }
}
