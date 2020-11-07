using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{

    protected static readonly string IDLE = "Idle";
    protected static readonly string ATTACK = "Attack";
    protected static readonly string RUN = "Run";

    [SerializeField] private Animator _animator;
    public UnitData Data { get; private set; }

    public bool isMoving = false;
    public bool isAttacking = false;
    public float attackCooldown = 3f;
    float attackTimer;

    MazeNode startNode = null;
    MazeNode endNode = null;

    IEnumerator IMove = null;

    public void Init((int, int) atIndex, UnitData data)
    {
        Data = data;
        startNode = MazeManager.Instance.Maze.mazeMatrix[atIndex.Item1, atIndex.Item2];
        endNode = null;
    }    

    private void PlayAnim(string animName)
    {
        _animator.Play(animName);
    }

    public void ExecuteUnitState()
    {

        if (!CheckIfInRange(GameManager.Instance.CityPos)
            && !CheckIfInRange(GameManager.Instance.PlayerPos))
        {

            attackTimer = attackCooldown;
            isAttacking = false;

            if (!isMoving)
            {
                isMoving = true;
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
            else if(attackTimer == attackCooldown)
            {
                Attack();
                attackTimer -= Time.deltaTime;
            }
            else if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attackTimer = attackCooldown;
            }
        }
    }

    private void Attack()
    {
        PlayAnim(ATTACK);

        GameManager.Instance.SpawnProjectile(transform.position);
    }

    public void Move(MazeNode toNode)
    {
        if (endNode == null || (endNode != toNode))
        {     
            endNode = toNode;

            PathFinding aStar = new AStarPathfinding();
            aStar.InitPathfinder(MazeManager.Instance.Maze, startNode, endNode);
            aStar.FindPath();
            //aStar.GenerateMazeGraphics(MazeManager.Instance.MazeGraphicsHolder);

            if (IMove != null)
            {
                StopCoroutine(IMove);
                IMove = null;
            }

            IMove = IMoveToDestination(aStar.GetPath());
            StartCoroutine(IMove);
        }
    }

    private IEnumerator IMoveToDestination(List<MazeNode> path)
    {
        if(path.Count > 0)
            PlayAnim(RUN);

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 destinationPos = path[i].NodePosition;
            Vector3 currentPos = this.transform.position;

            if(destinationPos.x < currentPos.x)
            {
                _animator.transform.localScale = new Vector3(-1f, 1f, 1f);
            }
           
            if(destinationPos.x > currentPos.x)
            {
                _animator.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            startNode = path[i];

            float step = 0;
            float speed = Data.MovementSpeed;

            while (this.transform.position != destinationPos)
            {
                yield return null;
                step += speed * Time.deltaTime;
                this.transform.position = Vector2.MoveTowards(currentPos, destinationPos, step);              
                AdjustCamera();
            }         
        }

        PlayAnim(IDLE);

        endNode = null;
    }

    public void Stop()
    {
        endNode = null;

        if (IMove != null)
            StopCoroutine(IMove);
        IMove = null;
        PlayAnim(IDLE);
    }

    public bool CheckIfInRange(Vector3 targetPos)
    {
        var heading = targetPos - transform.position;
        var distance = heading.magnitude;
        var direction = heading / distance;

        bool noWallsBetween = distance <= 5;

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

        return noWallsBetween;
    }

    public virtual void AdjustCamera()
    {

    }
}
