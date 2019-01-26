using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public int gridX;
    public int gridY;

    public int gCost;
    public int hCost;

    public Node parent;

    public TileType type;

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY, TileType type)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
        this.type = type;
    }

    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}

public enum TileType
{
    WALL,
    FLOOR
}
