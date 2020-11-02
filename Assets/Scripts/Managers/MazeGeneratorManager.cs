using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGeneratorManager : Singleton<MazeGeneratorManager>
{

    private MazeGenerator generator;

    protected override void OnAwake()
    {
        base.OnAwake();

        Debug.Log($"--->> AWAKE");
    }

    private void Start()
    {
        generator = new MazeGenerator();
        generator.GenerateMaze(20 , 40);
    }
}
