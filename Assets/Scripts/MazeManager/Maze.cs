using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze 
{
    public MazeNode[,] mazeMatrix;
	public int MazeSizeX { get; private set; }
    public int MazeSizeY { get; private set; }

	public void InitMaze(int mazeSizeX, int mazeSizeY)
    {
		mazeMatrix = new MazeNode[mazeSizeX, mazeSizeY];
		MazeSizeX = mazeSizeX;
		MazeSizeY = mazeSizeY;

		for (int i = 0; i < MazeSizeX; i++)
		{
			for (int j = 0; j < MazeSizeY; j++)
			{
				mazeMatrix[i, j] = new MazeNode(i, j);
			}
		}
	}

	public List<MazeNode> GetNeighbours(MazeNode node)
	{
		List<MazeNode> neighbours = new List<MazeNode>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				if (x == 0 && y == 0)
					continue;

				int checkX = node.NodeX + x;
				int checkY = node.NodeY + y;

				if (checkX >= 0 && checkX < MazeSizeX && checkY >= 0 && checkY < MazeSizeY)
				{
					neighbours.Add(mazeMatrix[checkX, checkY]);
				}
			}
		}

		return neighbours;
	}

	public void GenerateRandomMaze()
    {
		MazeGenerator generator = new MazeGenerator();
		mazeMatrix = generator.GenerateMaze(this, 5);
	}
}
