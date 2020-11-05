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

    public virtual void AdjustCamera()
    {

    }
}
