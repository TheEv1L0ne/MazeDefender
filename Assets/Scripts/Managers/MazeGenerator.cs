using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    private int[,] maze;

    public MazeGenerator()
    {
        
    }

    public void GenerateMaze(int width, int height)
    {
        maze = new int[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                maze[i, j] = 0;
            }
        }

        RecursiveDivision(maze, maze.GetLength(0), maze.GetLength(1));
        PrintMaze(maze);
    }

    private void RecursiveDivision(int [,] maze, int xMax,  int yMax, int xMin = 0, int yMin = 0)
    {
        //yield return new WaitForSeconds(1f);

        int[,] mazeT = maze;

        if (xMax < 0 && xMin < 0 && yMax < 0 && yMin < 0)
            return;

        if (xMax - xMin < 2 || yMax - yMin < 2)
        {
            return;
        }

        var isHorizontal = Random.Range(0, 2) % 2 == 0 ? true : false;

        int pIndex;
        int wIndex;

        if (isHorizontal)
        {
            wIndex = Wall(xMin + 1, xMax - 1);
            pIndex = Wall(yMin, yMax);

        }
        else
        {
            wIndex = Wall(yMin + 1, yMax - 1);
            pIndex = Wall(xMin, xMax);
        }

        mazeT = BuildWall(mazeT, isHorizontal, wIndex, pIndex, xMax, yMax, xMin, yMin);

        if (isHorizontal)
        {
            // Top and Bottom areas
            RecursiveDivision(mazeT, xMax: wIndex - 1, yMax, xMin, yMin);
            RecursiveDivision(mazeT, xMax, yMax, xMin: wIndex + 1, yMin);

        }
        else
        {
            // Left and Right areas
            RecursiveDivision(mazeT, xMax, yMax: wIndex -1, xMin, yMin);
            RecursiveDivision(mazeT, xMax, yMax, xMin, yMin: wIndex +1);
        }
    }

    private int Wall(int startIndex, int endIdex)
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
                output += $"{(maze[i, j] == 0 ? ("<color=red>" + maze[i, j] + "</color>" ) : (maze[i, j]).ToString())} ";
            }

            output += "\n";
        }

        Debug.Log(output);
    }
}
