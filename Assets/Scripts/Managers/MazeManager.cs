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

        Debug.Log($"--->> AWAKE");
    }

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        _maze = generator.GenerateMaze(100 , 50);

        MazeGraphicsGenerator mazeGraphics = new MazeGraphicsGenerator();
        mazeGraphics.GenerateMazeGraphics(_maze, _mazeGraphicsHolder);
    }
}
