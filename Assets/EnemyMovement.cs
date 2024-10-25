using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private TilemapToMatrix tilemapToMatrix;  // Reference to the TilemapToMatrix script
    //private Damageable damageable;  // Reference to the Damageable script
    private Transform player;
    public float moveSpeed = 2f;  // Speed of the sprite movement
    public float pathUpdateIntervalSeconds = 0.1f;  // How often to check for path updates (in seconds)
    public bool drawPath = true;

    private Vector2[] path;  // Private array of positions to move along (matrix coordinates)
    private int currentTargetIndex = 0;  // Index of the current target position
    private LineRenderer lineRenderer;  // LineRenderer component for visualizing the path

    public Damageable damageable;  // Referência ao script Damageable
    private bool isDead = false;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        tilemapToMatrix = GameObject.FindGameObjectWithTag("Scenario").GetComponent<TilemapToMatrix>();
        damageable.onDeath.AddListener(HandleDeath);

        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component is missing!");
            return;
        }

        // Start the movement coroutine
        StartCoroutine(MoveAlongPath());
        // Start the path update coroutine
        StartCoroutine(UpdatePath());
    }

    void HandleDeath(Damageable damageable)
    {
        isDead = true;
        //Debug.Log("O inimigo morreu!");
    }

    private System.Collections.IEnumerator UpdatePath()
    {
        while (true)
        {
            Vector2 enemyPosition = tilemapToMatrix.WalkableMatrixToMatrix(transform.position);
            Vector2 playerPosition = tilemapToMatrix.WalkableMatrixToMatrix(player.position);
            Vector2[] newPath = CalculatePath(enemyPosition, playerPosition);

            // Check if a valid new path is found
            if (newPath != null && newPath.Length > 0 && !isDead)
            {
                path = newPath;
                currentTargetIndex = 0;  // Reset target index when the path is updated
                if (drawPath) DrawPath();  // Visualize the path
            }
            else
            {
                // Clear the line if no valid path
                lineRenderer.positionCount = 0;
            }

            yield return new WaitForSeconds(pathUpdateIntervalSeconds);  // Wait before checking again
        }
    }

    private void DrawPath()
    {
        if (path != null && path.Length > 0)
        {
            lineRenderer.positionCount = path.Length;  // Set the number of points in the line

            // Set the positions of the points in world space
            for (int i = 0; i < path.Length; i++)
            {
                Vector3 worldPos = tilemapToMatrix.WalkableMatrix[(int)path[i].x, (int)path[i].y].worldPosition;
                lineRenderer.SetPosition(i, worldPos);
            }
        }
    }

    private Vector2[] CalculatePath(Vector2 start, Vector2 goal)
    {
        List<Vector2> queue = new List<Vector2>();  // List used as a priority queue
        HashSet<Vector2> visited = new HashSet<Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        queue.Add(start);
        visited.Add(start);

        while (queue.Count > 0)
        {
            // Sort the queue based on distance to the player
            queue.Sort((a, b) => Vector2.Distance(a, goal).CompareTo(Vector2.Distance(b, goal)));

            Vector2 current = queue[0];  // Take the closest position to the player
            queue.RemoveAt(0);  // Remove the position from the queue

            // Check if we reached the goal
            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            // Explore neighbors
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    //if (Mathf.Abs(dx) == Mathf.Abs(dy)) continue; // Skip diagonal movements

                    Vector2 neighbor = current + new Vector2(dx, dy);

                    // Check if neighbor is walkable and has not been visited
                    if (IsWalkable(neighbor) && !visited.Contains(neighbor))
                    {
                        queue.Add(neighbor);
                        visited.Add(neighbor);
                        cameFrom[neighbor] = current;
                    }
                }
            }
        }

        return null;  // No path found
    }

    private bool IsWalkable(Vector2 position)
    {
        if (position.x < 0 || position.x >= tilemapToMatrix.WalkableMatrix.GetLength(0) ||
            position.y < 0 || position.y >= tilemapToMatrix.WalkableMatrix.GetLength(1))
        {
            return false;  // Out of bounds
        }

        return tilemapToMatrix.WalkableMatrix[(int)position.x, (int)position.y].isWalkable;
    }

    private Vector2[] ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
    {
        List<Vector2> totalPath = new List<Vector2> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Add(current);
        }
        totalPath.Reverse();
        return totalPath.ToArray();
    }

    private System.Collections.IEnumerator MoveAlongPath()
    {
        while (true && !isDead)
        {
            if (path == null || path.Length == 0)
            {
                yield return null;  // Wait for the path to be calculated
            }
            else
            {
                while (currentTargetIndex < path.Length && !isDead)
                {
                    // Get the current target position from the path
                    Vector2 targetMatrixPos = path[currentTargetIndex];
                    Vector3 targetWorldPos = tilemapToMatrix.WalkableMatrix[(int)targetMatrixPos.x, (int)targetMatrixPos.y].worldPosition;

                    // Move towards the target position
                    while (Vector2.Distance(transform.position, targetWorldPos) > 0.1f && !isDead)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
                        yield return null;  // Wait for the next frame
                    }

                    // Move to the next target position
                    currentTargetIndex++;
                }

                // Reset target index to start over for a new path
                currentTargetIndex = 0;
                path = null;  // Clear the path until recalculated
            }

            yield return null;  // Wait until next frame if no path is available
        }
    }
}
