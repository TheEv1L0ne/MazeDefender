using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    private Unit _enemy;

    public override void AdjustCamera()
    {
        CameraManager.Instance.UpdateCameraPos(this.transform.position);
    }

    public override void ExecuteUnitState()
    {
        if(endNode == null)
        {
            _enemy = GetClosestEnemy();
            if (_enemy != null)
            {
                CheckCooldown();
            }
        }
    }

    protected override void Attack()
    {
        PlayAnim(ATTACK);

        SpawnProjectile(transform.position);
    }

    private void SpawnProjectile(Vector3 atLocation)
    {
        GameObject projectile = Instantiate(GameManager.Instance.ProjectilePlayer);
        projectile.transform.position = atLocation;
        Projectile p = projectile.GetComponent<Projectile>();

        p.Init(
            Data.AttackDistance,
            3f,
            Data.AttackDamage,
            ProjectileTargetType.Enemy,
            _enemy);

        p.Fly();
    }

    public Unit GetClosestEnemy()
    {
        float minDistance = 10000;
        Unit closestEnemy = null;

        foreach (var enemy in GameManager.Instance.EnemyUnits)
        {
            var heading = enemy.transform.position - transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            bool noWallsBetween = distance <= 3;

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
                            noWallsBetween = false;
                    }
                }
            }

            if(noWallsBetween)
            {
                if(distance < minDistance)
                {
                    closestEnemy = enemy;
                }
            }

        }

        return closestEnemy;
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;

        UIManager.Instance.playUI.SetPlayerHp(currentHitPoints, Data.HitPoints);
    }
}
