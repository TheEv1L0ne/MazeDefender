using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public static readonly string IDLE = "Idle";
    public static readonly string ATTACK = "Attack";
    public static readonly string RUN = "Run";

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

    public void PlayAnim(string animName)
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

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 destinationPos = path[i].NodePosition;
            Vector3 currentPos = this.transform.position;

            startNode = path[i];

            float step = 0;
            float speed = 5f;

            while (this.transform.position != destinationPos)
            {
                yield return null;
                step += speed * Time.deltaTime;
                this.transform.position = Vector2.MoveTowards(currentPos, destinationPos, step);
                CameraManager.Instance.UpdateCameraPos(this.transform.position);
            }
        }

        endNode = null;
    }
}
