﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGraphicsGenerator
{
    public void GenerateMazeGraphics(Maze maze, Transform mazeHolder)
    {
        for (int i = 0; i < maze.mazeMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < maze.mazeMatrix.GetLength(1); j++)
            {
                GameObject tile = TilePoolManager.Instance.GetTileFromPool();
                tile.transform.parent = mazeHolder;

                tile.transform.position = new Vector3(i, j, 0f);

                if(maze.mazeMatrix[i,j] == 7)
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }
    }
}
