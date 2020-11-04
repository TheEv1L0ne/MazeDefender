using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PathFinding : IPathFinding
{
    protected Maze _maze;
    protected MazeNode startNode;
    protected MazeNode targetNode;

    protected List<MazeNode> path;

    public void InitPathfinder(Maze maze, MazeNode startNode, MazeNode targetNode)
    {
        _maze = maze;
        this.startNode = startNode;
        this.targetNode = targetNode;
    }

    public virtual void FindPath()
    {

    }
    public virtual void RetracePath(MazeNode startNode, MazeNode endNode)
    {

    }

    public virtual List<MazeNode> GetPath()
    {
        return null;
    }
}
