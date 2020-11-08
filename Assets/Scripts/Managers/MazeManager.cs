using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeManager : Singleton<MazeManager>
{
    private Maze _maze;
    public Maze Maze { get => _maze;}
    public Transform MazeGraphicsHolder { get => _mazeGraphicsHolder;}

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

        _maze.GeneratePassages();

        MazeGraphicsGenerator mazeGraphics = new MazeGraphicsGenerator();
        mazeGraphics.GenerateMazeGraphics(_maze, _mazeGraphicsHolder);
    }

    public void InitBase((int,int) atIndex)
    {
        _mazeGraphicsHolder.GetChild(atIndex.Item1 * _maze.MazeSizeY + atIndex.Item2).GetComponent<SpriteRenderer>().sprite = SpriteManager.Instance.Base;
        _maze.mazeMatrix[atIndex.Item1, atIndex.Item2].Type = MazeNode.TileType.City;

    }

    public (int, int) GetEmptyNodeIndex()
    {
        bool spawnLocGood = false;

        int x = -1;
        int y = -1;

        while (!spawnLocGood)
        {
            x = Random.Range(0, _maze.MazeSizeX);
            y = Random.Range(0, _maze.MazeSizeY);

            if (_maze.mazeMatrix[x, y].Type == MazeNode.TileType.Ground)
            {
                spawnLocGood = true;
            }
        }

        return (x,y);
    }

    public (int, int) GetEmptyCloseTo((int, int) index)
    {
        bool spawnLocGood = false;

        int x = -1;
        int y = -1;

        int distanceFromTile = 3;

        int xMin = Mathf.Clamp(index.Item1 - distanceFromTile, 0, index.Item1 - distanceFromTile);
        int xMax = Mathf.Clamp(index.Item1 + distanceFromTile, index.Item1 + distanceFromTile, _maze.MazeSizeX);

        int yMin = Mathf.Clamp(index.Item2 - distanceFromTile, 0, index.Item2 - distanceFromTile);
        int yMax = Mathf.Clamp(index.Item2 + distanceFromTile, index.Item2 + distanceFromTile, _maze.MazeSizeY);

        while (!spawnLocGood)
        {
            x = Random.Range(xMin, xMax);
            y = Random.Range(yMin, yMax);

            if (_maze.mazeMatrix[x, y].Type == MazeNode.TileType.Ground)
            {
                spawnLocGood = true;
            }
        }

        return (x, y);
    }    

    public MazeNode GetNode(int atX, int atY)
    {
        return _maze.mazeMatrix[atX, atY];
    }

    public void PlayerGraphics(int x, int y)
    {
        Debug.Log($"_mazeGraphicsHolder count = {_mazeGraphicsHolder.childCount}");
        Debug.Log($"{x * _maze.mazeMatrix.GetLength(1) + y}");
        Transform gameObject = _mazeGraphicsHolder.GetChild(x * _maze.mazeMatrix.GetLength(1) + y);
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }

    public void ClearMaze()
    {
        for (int i = _mazeGraphicsHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(_mazeGraphicsHolder.GetChild(i).gameObject);
        }
    }

}
