using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    public Tilemap tileMap;

    private Node[,] grid;
    private List<Node> globalPath;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }
    

    void CreateGrid()
    {      
        tileMap.CompressBounds();
        BoundsInt tilemapBounds = tileMap.cellBounds;
        TileBase[] allTiles = tileMap.GetTilesBlock(tilemapBounds);
        grid = new Node[tilemapBounds.size.x, tilemapBounds.size.y];
        for (int x = 0; x < tilemapBounds.size.x; x++)
        {
            for (int y = 0; y < tilemapBounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * tilemapBounds.size.x];
                bool walkable = (tile.name == "Floor");
                grid[x, y] = new Node(walkable, tileMap.GetCellCenterWorld(new Vector3Int(x, y, 0)), x, y);
            }
        }
    }

    public void ChangeNodeState(Vector3 position, bool walkable)
    {
        Node node = NodeFromWorlPoint(position);
        node.walkable = walkable;
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x ==0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < grid.GetLength(0) && checkY >= 0 && checkY < grid.GetLength(1))
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorlPoint(Vector3 worlPosition)
    {
        int x = (int)(worlPosition.x - tileMap.tileAnchor.x);
        int y = (int)(worlPosition.y - tileMap.tileAnchor.y);

        return grid[x, y];
    }

    private void OnDrawGizmos()
    {
        if(grid != null)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    Node node = grid[x, y];
                    Gizmos.color = (node.walkable) ? Color.white : Color.red;

                    if(globalPath != null)
                    {
                        if (globalPath.Contains(node))
                        {
                            Gizmos.color = Color.black;
                        }
                    }
                    Gizmos.DrawCube(node.worldPosition, Vector3.one);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void FindPath(Vector3 startPosition, Vector3 destination)
    {
        Node startNode = NodeFromWorlPoint(startPosition);
        Node endNode = NodeFromWorlPoint(destination);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);
            if (currentNode.worldPosition == endNode.worldPosition)
            {

                RetracePath(startNode, endNode);
                return;
            }

            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, endNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }

           
        }
    }

    public void RetracePath(Node startNode, Node endNode)
    {
        
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();

        globalPath = path;
    }

    public int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);

        return 14 * distX + 10 * (distY - distX);
    }

    public List<Node> GetGlobalPath()
    {
        return globalPath;
    }
}
