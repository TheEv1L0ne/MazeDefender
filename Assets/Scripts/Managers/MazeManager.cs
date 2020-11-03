using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : Singleton<MazeManager>
{
    private Maze _maze;
    public Maze Maze { get => _maze;}

    [SerializeField] private Transform mazeGraphicsHolder;

    protected override void OnAwake()
    {
        base.OnAwake();

        Debug.Log($"--->> AWAKE");
    }

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        _maze = generator.GenerateMaze(20 , 40);

        MazeGraphicsGenerator mazeGraphics = new MazeGraphicsGenerator();
        mazeGraphics.GenerateMazeGraphics(_maze, mazeGraphicsHolder);
    }
}
