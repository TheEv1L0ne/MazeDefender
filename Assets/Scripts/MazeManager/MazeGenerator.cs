using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class MazeGenerator
{
    private Maze _maze;
    private int _mazeDensity;
    public MazeGenerator()
    {

    }

    public MazeNode[,] GenerateMaze(Maze maze, int mazeDensity = 5)
    {
        _mazeDensity = mazeDensity;
        _maze = maze;
        RecursiveDivision(_maze.MazeSizeX, _maze.MazeSizeY);
        //PrintMaze(_maze.mazeMatrix);

        return _maze.mazeMatrix;
    }

    private void RecursiveDivision(int xMax, int yMax, int xMin = 0, int yMin = 0, bool isHorizontal = true)
    {
        MazeNode[,] mazeTmp = _maze.mazeMatrix;


        if (xMax - xMin < _mazeDensity || yMax - yMin < _mazeDensity)
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

    private MazeNode[,] BuildWall(MazeNode[,] maze, bool isHorizontal, int wallindex, int passageIndex,
        int xMax, int yMax, int xMin, int yMin)
    {
        MazeNode[,] mazeT = maze;

        for (int i = (isHorizontal ? yMin : xMin); i < (isHorizontal ? yMax : xMax); i++)
        {
            if (i == passageIndex)
                continue;

            if (isHorizontal)
                maze[wallindex, i].Walkable = false;
            else
                maze[i, wallindex].Walkable = false;
        }

        return mazeT;
    }

    private void PrintMaze(MazeNode[,] maze)
    {

        string output = "";

        for (int i = 0; i < _maze.MazeSizeX; i++)
        {
            for (int j = 0; j < _maze.MazeSizeY; j++)
            {
                output += $"{(maze[i, j].Walkable ? ("<color=red>" + maze[i, j] + "</color>") : (maze[i, j]).ToString())} ";
            }

            output += "\n";
        }

        Debug.Log(output);
    }
}
