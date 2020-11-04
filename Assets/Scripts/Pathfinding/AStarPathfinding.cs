using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinding : PathFinding
{
    private List<MazeNode> openSet;
    private HashSet<MazeNode> closedSet;

    public override void FindPath()
    {
		openSet = new List<MazeNode>();
		closedSet = new HashSet<MazeNode>();

		openSet.Add(startNode);

		while (openSet.Count > 0)
		{
			MazeNode node = openSet[0];
			for (int i = 1; i < openSet.Count; i++)
			{
				if (openSet[i].FCost < node.FCost || openSet[i].FCost == node.FCost)
				{
					if (openSet[i].HCost < node.HCost)
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode)
			{
				RetracePath(startNode, targetNode);
				return;
			}

			foreach (MazeNode neighbour in _maze.GetNeighbours(node))
			{
				if (!neighbour.Walkable || closedSet.Contains(neighbour))
				{
					continue;
				}

				int newCostToNeighbour = node.GCost + GetDistance(node, neighbour);
				if (newCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
				{
					neighbour.GCost = newCostToNeighbour;
					neighbour.HCost = GetDistance(neighbour, targetNode);
					neighbour.ParentNode = node;

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
					else
                    {
						int index = openSet.IndexOf(neighbour);
						openSet[index] = neighbour;
                    }
				}
			}
		}
	}

	public override void RetracePath(MazeNode startNode, MazeNode endNode)
	{
		List<MazeNode> path = new List<MazeNode>();
		MazeNode currentNode = endNode;

		while (currentNode != startNode)
		{
			path.Add(currentNode);
			currentNode = currentNode.ParentNode;
		}
		path.Reverse();

		this.path = path;
	}

	public override List<MazeNode> GetPath()
    {
		return path;
    }

	private int GetDistance(MazeNode nodeA, MazeNode nodeB)
	{
		int dstX = Mathf.Abs(nodeA.NodeX - nodeB.NodeX);
		int dstY = Mathf.Abs(nodeA.NodeY - nodeB.NodeY);

		if (dstX > dstY)
			return 14 * dstY + 10 * (dstX - dstY);
		return 14 * dstX + 10 * (dstY - dstX);
	}
}
