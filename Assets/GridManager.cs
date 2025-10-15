using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 20;
    public int gridHeight = 20;
    public float nodeSize = 1f;
    public LayerMask unwalkableMask;

    private Node[,] grid;

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridWidth, gridHeight];

        // Calculate the bottom-left corner of the grid
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWidth / 2 - Vector3.forward * gridHeight / 2;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 worldPosition = worldBottomLeft + Vector3.right * (x * nodeSize + nodeSize / 2) + Vector3.forward * (y * nodeSize + nodeSize / 2);
                worldPosition.y = 0.5f; // Set to ground level

                // Check if this position has an obstacle
                bool walkable = !Physics.CheckSphere(worldPosition, nodeSize * 0.2f, unwalkableMask);

                grid[x, y] = new Node(walkable, worldPosition, x, y);
            }
        }

        Debug.Log("Grid created with " + gridWidth + "x" + gridHeight + " nodes");
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPosition)
    {
        // Convert world position to grid coordinates
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWidth / 2 - Vector3.forward * gridHeight / 2;

        float percentX = (worldPosition.x - worldBottomLeft.x) / (gridWidth * nodeSize);
        float percentY = (worldPosition.z - worldBottomLeft.z) / (gridHeight * nodeSize);

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridWidth - 1) * percentX);
        int y = Mathf.RoundToInt((gridHeight - 1) * percentY);

        return grid[x, y];
    }

    public Node[,] GetGrid()
    {
        return grid;
    }

    // Visualize the grid in the editor (optional but helpful!)
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWidth * nodeSize, 1, gridHeight * nodeSize));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = n.walkable ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeSize - 0.1f));
            }
        }
    }
}