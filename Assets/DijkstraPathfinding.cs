using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinding : MonoBehaviour, IPathfinding
{
    public GridManager gridManager;

    void Start()
    {
        gridManager = GetComponent<GridManager>();
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = gridManager.GetNodeFromWorldPosition(startPos);
        Node targetNode = gridManager.GetNodeFromWorldPosition(targetPos);

        List<Node> unvisitedNodes = new List<Node>();
        HashSet<Node> visitedNodes = new HashSet<Node>();

        // Initialize all nodes
        Node[,] grid = gridManager.GetGrid();
        foreach (Node node in grid)
        {
            node.gCost = int.MaxValue;
            node.parent = null;
            if (node.walkable)
                unvisitedNodes.Add(node);
        }

        // Force start node to be walkable if it's not
        if (!startNode.walkable)
        {
            startNode.walkable = true;
            unvisitedNodes.Add(startNode);
        }

        // Force target node to be walkable if it's not
        if (!targetNode.walkable)
        {
            targetNode.walkable = true;
            if (!unvisitedNodes.Contains(targetNode))
                unvisitedNodes.Add(targetNode);
        }

        startNode.gCost = 0;

        while (unvisitedNodes.Count > 0)
        {
            // Find node with smallest distance
            Node currentNode = null;
            int lowestCost = int.MaxValue;

            foreach (Node node in unvisitedNodes)
            {
                if (node.gCost < lowestCost)
                {
                    lowestCost = node.gCost;
                    currentNode = node;
                }
            }

            if (currentNode == null || currentNode.gCost == int.MaxValue)
            {
                break;
            }

            unvisitedNodes.Remove(currentNode);
            visitedNodes.Add(currentNode);

            // If we reached the target, reconstruct path
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            // Check all neighbors
            foreach (Node neighbour in GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || visitedNodes.Contains(neighbour))
                    continue;

                int newDistanceToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newDistanceToNeighbour < neighbour.gCost)
                {
                    neighbour.gCost = newDistanceToNeighbour;
                    neighbour.parent = currentNode;
                }
            }
        }

        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        Node[,] grid = gridManager.GetGrid();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridManager.gridWidth &&
                    checkY >= 0 && checkY < gridManager.gridHeight)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}