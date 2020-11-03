using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : Singleton<MazeManager>
{
    private Maze _maze;
    public Maze Maze { get => _maze;}

    [SerializeField] private Transform _mazeGraphicsHolder;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    public void GenerateMaze()
    {     
        _maze = new Maze();
        _maze.InitMaze(50, 40);
        _maze.GenerateRandomMaze();

        MazeGraphicsGenerator mazeGraphics = new MazeGraphicsGenerator();
        mazeGraphics.GenerateMazeGraphics(_maze, _mazeGraphicsHolder);

        MazeNode startNode = GetEmptyTile();
        MazeNode endNode = GetEmptyTile();

        AStarPathfinding aStar = new AStarPathfinding();
        aStar.InitPathfinder(_maze, startNode, endNode);
        aStar.FindPath();
        aStar.GenerateMazeGraphics(_mazeGraphicsHolder);
    }

    private MazeNode GetEmptyTile()
    {
        bool spawnLocGood = false;

        int x = -1;
        int y = -1;

        while (!spawnLocGood)
        {
            x = Random.Range(0, _maze.mazeMatrix.GetLength(0));
            y = Random.Range(0, _maze.mazeMatrix.GetLength(1));

            if (_maze.mazeMatrix[x, y].Walkable)
            {
                spawnLocGood = true;
                return _maze.mazeMatrix[x, y];

            }
        }

        return null;

    }

    public void PlayerGraphics(int x, int y)
    {
        Debug.Log($"_mazeGraphicsHolder count = {_mazeGraphicsHolder.childCount}");
        Debug.Log($"{x * _maze.mazeMatrix.GetLength(1) + y}");
        Transform gameObject = _mazeGraphicsHolder.GetChild(x * _maze.mazeMatrix.GetLength(1) + y);
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
