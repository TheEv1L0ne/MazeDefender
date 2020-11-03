using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
        MazeManager.Instance.GenerateMaze();
        Maze maze = MazeManager.Instance.Maze;
        SpawnPlayer(maze);

        StartCoroutine(GameLoop());
    }

    private IEnumerator GameLoop()
    {
        while(true)
        {
            yield return null;
        }
    }

    private void SpawnPlayer(Maze maze)
    {
        bool spawnLocGood = false;

        int x = -1;
        int y = -1;

        while (!spawnLocGood)
        {
            x = Random.Range(0, maze.mazeMatrix.GetLength(0));
            y = Random.Range(0, maze.mazeMatrix.GetLength(1));

            if (maze.mazeMatrix[x,y].Walkable)
            {
                spawnLocGood = true;
            }
        }

        MazeManager.Instance.PlayerGraphics(x, y);

    }
}
