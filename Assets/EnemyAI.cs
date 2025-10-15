using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 3f;
    public float pathUpdateInterval = 0.5f;

    private IPathfinding pathfinding;
    private List<Node> currentPath;
    private int currentPathIndex = 0;
    private float pathUpdateTimer;

    void Start()
    {
        // Try to find either Pathfinding (A*) or DijkstraPathfinding
        pathfinding = FindObjectOfType<Pathfinding>() as IPathfinding;

        if (pathfinding == null)
        {
            pathfinding = FindObjectOfType<DijkstraPathfinding>() as IPathfinding;
        }

        if (pathfinding == null)
        {
            Debug.LogError("No pathfinding component found! Add either Pathfinding or DijkstraPathfinding to GridManager.");
        }
        else
        {
            Debug.Log("Pathfinding found: " + pathfinding.GetType().Name);
        }

        if (player == null)
        {
            Debug.LogError("Player not assigned!");
        }

        pathUpdateTimer = pathUpdateInterval;
    }

    void Update()
    {
        pathUpdateTimer -= Time.deltaTime;

        if (pathUpdateTimer <= 0)
        {
            UpdatePath();
            pathUpdateTimer = pathUpdateInterval;
        }

        FollowPath();
    }

    void UpdatePath()
    {
        if (pathfinding == null || player == null) return;

        currentPath = pathfinding.FindPath(transform.position, player.position);
        currentPathIndex = 0;

        if (currentPath == null)
        {
            Debug.LogWarning("No path found!");
        }
        else
        {
            Debug.Log("Path found with " + currentPath.Count + " nodes");
        }
    }

    void FollowPath()
    {
        if (currentPath == null || currentPath.Count == 0)
            return;

        if (currentPathIndex >= currentPath.Count)
            return;

        Vector3 targetPosition = currentPath[currentPathIndex].worldPosition;
        targetPosition.y = transform.position.y;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            currentPathIndex++;
        }
    }
}