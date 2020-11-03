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
    }

    public void PlayerGraphics(int x, int y)
    {
        Debug.Log($"_mazeGraphicsHolder count = {_mazeGraphicsHolder.childCount}");
        Debug.Log($"{x * _maze.mazeMatrix.GetLength(1) + y}");
        Transform gameObject = _mazeGraphicsHolder.GetChild(x * _maze.mazeMatrix.GetLength(1) + y);
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
}
