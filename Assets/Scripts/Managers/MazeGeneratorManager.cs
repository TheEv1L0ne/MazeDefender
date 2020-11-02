using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorManager : Singleton<MazeGeneratorManager>
{
    private Maze _maze;

    public Maze Maze { get => _maze;}

    protected override void OnAwake()
    {
        base.OnAwake();

        Debug.Log($"--->> AWAKE");
    }

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        _maze = generator.GenerateMaze(20 , 40);
    }
}
