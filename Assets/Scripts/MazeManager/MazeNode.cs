using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeNode
{
    public bool Walkable { get; set; }
    public Vector3 NodePosition { get; set; }
    public int NodeX { get; set; }
    public int NodeY { get; set; }
    public MazeNode ParentNode { get; set; }
    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get => GCost + HCost; }
    public TileType Type { get; set; }

    public MazeNode(int x, int y)
    {
        Type = TileType.Ground;
        NodeX = x;
        NodeY = y;
    }

    public enum TileType
    {
        City,
        Wall,
        Ground,
        Passage
    }
}
