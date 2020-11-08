using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float _distance;
    private float _speed;
    private ProjectileTargetType _targetType;
    private Vector3 _targetPos;
    private int _damage;
    private Unit _enemyUnit = null;

    public void Init(float withDistance, float withSpeed, int withDamage, ProjectileTargetType withTargetType, Vector3 targetPos)
    {
        _distance = withDistance;
        _targetType = withTargetType;
        _speed = withSpeed;
        _targetPos = targetPos;
        _damage = withDamage;
    }

    public void Init(float withDistance, float withSpeed, int withDamage, ProjectileTargetType withTargetType, Unit enemyUnit)
    {
        _distance = withDistance;
        _targetType = withTargetType;
        _speed = withSpeed;
        _targetPos = enemyUnit.transform.position;
        _enemyUnit = enemyUnit;
        _damage = withDamage;
    }

    public void Fly()
    {
        StartCoroutine(IFly());
    }

    public IEnumerator IFly()
    {
        var heading = _targetPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        var endLocation = transform.position + (direction * _distance);

        float step = 0;
        float speed = _speed;

        Vector3 currentPos = transform.position;
        if (this != null)
            while (transform.position != endLocation)
            {
                yield return null;
                step += speed * Time.deltaTime;

                transform.position = Vector2.MoveTowards(currentPos, endLocation, step);
            }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "City" && _targetType == ProjectileTargetType.City)
        {
            StopAllCoroutines();

            GameManager.Instance.PlayerBase.TakeDamage(_damage);

            Destroy(this.gameObject);
        }

        if(collision.tag == "Player" && _targetType == ProjectileTargetType.Player)
        {
            StopAllCoroutines();

            (GameManager.Instance.PlayerUnit as PlayerUnit).TakeDamage(_damage);

            Destroy(this.gameObject);
        }

        if (collision.tag == "Enemy" && _targetType == ProjectileTargetType.Enemy)
        {
            StopAllCoroutines();

            (_enemyUnit as EnemyUnit).TakeDamaga(_damage);

            Destroy(this.gameObject);
        }
    }
}

public enum ProjectileTargetType
{
    City,
    Player,
    Enemy
}
