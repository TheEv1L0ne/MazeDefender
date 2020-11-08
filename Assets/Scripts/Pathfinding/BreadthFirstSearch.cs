using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadthFirstSearch : PathFinding
{

    int ROW = 9;
    int COL = 10;

    // These arrays are used to get row and column
    // numbers of 4 neighbours of a given cell
    static int[] rowNum = { -1, 0, 0, 1, -1, -1, 1, 1 };
    static int[] colNum = { 0, -1, 1, 0, -1, 1, -1, 1 };

    public QueueNode endPoint;

    public override void FindPath()
    {
        Debug.Log("FIND PATH RECURSIVE");

        ROW = _maze.MazeSizeX;
        COL = _maze.MazeSizeY;

        int dist = BFS(startNode, targetNode);
        if(dist != -1)
        {
            RetracePath(startNode, targetNode);
        }
    }

    public override void RetracePath(MazeNode startNode, MazeNode endNode)
    {
        List<MazeNode> path = new List<MazeNode>();
        QueueNode currentNode = endPoint;

        while (currentNode.parentNode != null)
        {
            path.Add(currentNode.mazeNode);

            currentNode = currentNode.parentNode;
        }

        path.Add(currentNode.mazeNode);
        path.Reverse();

        this.path = path;
    }

    public override List<MazeNode> GetPath()
    {
        return path;
    }

    public class QueueNode
    {
        public MazeNode mazeNode;

        public int dist;

        public QueueNode parentNode;

        public QueueNode(MazeNode mazeNode, int dist)
        {
            this.mazeNode = mazeNode;
            this.dist = dist;
        }
    };


    bool isValid(int row, int col)
    {
        return (row >= 0) && (row < ROW) &&
               (col >= 0) && (col < COL);
    }

    public int BFS(MazeNode src, MazeNode dest)
    {
        if (_maze.mazeMatrix[src.NodeX, src.NodeY].Type == MazeNode.TileType.Wall
            || _maze.mazeMatrix[dest.NodeX, dest.NodeY].Type == MazeNode.TileType.Wall)
            return -1;

        bool[,] visited = new bool[ROW, COL];

        visited[src.NodeX, src.NodeY] = true;

        Queue<QueueNode> q = new Queue<QueueNode>();

        QueueNode s = new QueueNode(src, 0)
        {
            parentNode = null
        };

        q.Enqueue(s);

        while (q.Count != 0)
        {
            QueueNode curr = q.Peek();
            MazeNode mazeNode = curr.mazeNode;

            if (mazeNode.NodeX == dest.NodeX && mazeNode.NodeY == dest.NodeY)
            {
                endPoint = curr;
                return curr.dist;
            }

            q.Dequeue();

            for (int i = 0; i < rowNum.Length; i++)
            {
                int row = mazeNode.NodeX + rowNum[i];
                int col = mazeNode.NodeY + colNum[i];

                if (isValid(row, col) 
                    && (_maze.mazeMatrix[row, col].Type == MazeNode.TileType.Ground
                        || _maze.mazeMatrix[row, col].Type == MazeNode.TileType.Passage
                        || _maze.mazeMatrix[row, col].Type == MazeNode.TileType.City) 
                    &&!visited[row, col])
                {
                    visited[row, col] = true;
                    QueueNode Adjcell = new QueueNode(_maze.mazeMatrix[row, col], curr.dist + 1)
                    {
                        parentNode = curr
                    };
                    q.Enqueue(Adjcell);
                }
            }
        }

        return -1;
    }

}
