﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private Gradient _gradient;


    bool playerInRange = false;
    bool baseinRange = false;

    public override void ExecuteUnitState()
    {
        playerInRange = CheckIfInRange(GameManager.Instance.PlayerPos);
        baseinRange = CheckIfInRange(GameManager.Instance.CityPos);

        if (!playerInRange
            && !baseinRange)
        {       
            isAttacking = false;

            if (!isMoving)
            {
                isMoving = true;
                startNode = GetCurrentTile();
                Move(GameManager.Instance.CityNode);
            }
        }
        else
        {
            isMoving = false;

            if (!isAttacking)
            {
                isAttacking = true;
                Stop();
            }

            CheckCooldown();
        }
    }

    private void Stop()
    {
        endNode = null;

        if (IMove != null)
            StopCoroutine(IMove);

        IMove = null;

        PlayAnim(IDLE);
    }

    protected override void Attack()
    {
        PlayAnim(ATTACK);

        SpawnEnemyProjectile(transform.position, playerInRange ? 1 : 0);
    }

    private void SpawnEnemyProjectile(Vector3 atLocation, int withTarget)
    {
        GameObject projectile = Instantiate(GameManager.Instance.Projectile);
        projectile.transform.position = atLocation;
        Projectile p = projectile.GetComponent<Projectile>();

        p.Init(
            Data.AttackDistance,
            3f,
            Data.AttackDamage,
            withTarget == 0 ? ProjectileTargetType.City : ProjectileTargetType.Player,
            withTarget == 0 ? GameManager.Instance.CityPos : GameManager.Instance.PlayerPos);

        p.Fly();
    }

    private bool CheckIfInRange(Vector3 targetPos)
    {
        var heading = targetPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        bool noWallsBetween = distance <= 4;

        if (noWallsBetween)
        {
            RaycastHit2D[] hit1 = Physics2D.RaycastAll(transform.position, direction, distance);
            {
                foreach (var item in hit1)
                {
                    int arrayIndex = item.transform.GetSiblingIndex();
                    int x = arrayIndex / MazeManager.Instance.Maze.MazeSizeY;
                    int y = arrayIndex % MazeManager.Instance.Maze.MazeSizeY;

                    if (MazeManager.Instance.Maze.mazeMatrix[x, y].Type == MazeNode.TileType.Wall)
                    {

                        noWallsBetween = false;
                    }
                }
            }
        }

        return noWallsBetween;
    }

    public void TakeDamaga(int damage)
    {
        currentHitPoints -= damage;
        SetHPBarColor(currentHitPoints);
    }

    private void SetHPBarColor(int currentHp)
    {
        float x = (float)currentHp / (float)Data.HitPoints;

        if(this != null)
            hpBar.color = _gradient.Evaluate(x);
    }

    public void OnDestroy()
    {
        StopAllCoroutines();
    }

    private MazeNode GetCurrentTile()
    { 
        int x = Mathf.Abs((int)transform.position.x);
        int y = Mathf.Abs((int)transform.position.y);

        MazeNode clickedNode = MazeManager.Instance.GetNode(x, y);

        return clickedNode;
    }
}
