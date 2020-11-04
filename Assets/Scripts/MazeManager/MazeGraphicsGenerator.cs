using System.Collections;
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

                float offset = 0.5f;

                tile.transform.position = new Vector3(i + offset, -j - offset, 0f);
                maze.mazeMatrix[i, j].NodePosition = tile.transform.position;

                if (maze.mazeMatrix[i,j].Walkable == false)
                {
                    tile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.Clif;
                }
                else
                {
                    tile.GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.Dirt;
                }
            }
        }
    }
}
