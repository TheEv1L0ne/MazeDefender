using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    private Maze _maze;

    public MazeGenerator()
    {

    }

    public Maze GenerateMaze(int width = 0, int height = 0)
    {
        _maze = new Maze
        {
            mazeMatrix = new int[width, height]
        };

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                _maze.mazeMatrix[i, j] = 0;
            }
        }

        RecursiveDivision(_maze.mazeMatrix.GetLength(0), _maze.mazeMatrix.GetLength(1));
        //PrintMaze(_maze.mazeMatrix);

        return _maze;
    }

    private void RecursiveDivision(int xMax, int yMax, int xMin = 0, int yMin = 0, bool isHorizontal = true)
    {
        int[,] mazeTmp = _maze.mazeMatrix;

        //if (xMax < 0 && xMin < 0 && yMax < 0 && yMin < 0)
        //    return;

        if (xMax - xMin < 2 || yMax - yMin < 2)
        {
            return;
        }

        // 0 -> Horizontal
        // 1 -> Vertial
        //var isHorizontal = Random.Range(0, 2) == 0;

        int wallIndex = isHorizontal ? GenerateIndex(xMin + 1, xMax - 1) : GenerateIndex(yMin + 1, yMax - 1);
        int passageIndex = isHorizontal ? GenerateIndex(yMin, yMax) : GenerateIndex(xMin, xMax);

        _maze.mazeMatrix = BuildWall(mazeTmp, isHorizontal, wallIndex, passageIndex, xMax, yMax, xMin, yMin);

        if (isHorizontal)
        {
            // Top and Bottom areas
            RecursiveDivision(xMax: wallIndex - 1, yMax, xMin, yMin, !isHorizontal);
            RecursiveDivision(xMax, yMax, xMin: wallIndex + 1, yMin ,!isHorizontal);

        }
        else
        {
            // Left and Right areas
            RecursiveDivision(xMax, yMax: wallIndex - 1, xMin, yMin, !isHorizontal);
            RecursiveDivision(xMax, yMax, xMin, yMin: wallIndex + 1, !isHorizontal);
        }
    }

    private int GenerateIndex(int startIndex, int endIdex)
    {
        return Random.Range(startIndex, endIdex);
    }

    private int[,] BuildWall(int[,] maze, bool isHorizontal, int wallindex, int passageIndex,
        int xMax, int yMax, int xMin, int yMin)
    {
        int[,] mazeT = maze;

        for (int i = (isHorizontal ? yMin : xMin); i < (isHorizontal ? yMax : xMax); i++)
        {
            if (i == passageIndex)
                continue;

            if (isHorizontal)
                maze[wallindex, i] = 7;
            else
                maze[i, wallindex] = 7;
        }

        return mazeT;
    }

    private void PrintMaze(int[,] maze)
    {

        string output = "";

        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                output += $"{(maze[i, j] == 0 ? ("<color=red>" + maze[i, j] + "</color>") : (maze[i, j]).ToString())} ";
            }

            output += "\n";
        }

        Debug.Log(output);
    }
}
