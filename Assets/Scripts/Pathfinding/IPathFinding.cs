using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPathFinding
{
    void FindPath();
    void RetracePath(MazeNode startNode, MazeNode endNode);
    List<MazeNode> GetPath();
}
