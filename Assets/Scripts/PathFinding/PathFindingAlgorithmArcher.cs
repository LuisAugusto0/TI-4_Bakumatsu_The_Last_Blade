using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(LineRenderer))]

// Uses main tile map along with tile collision data to determine optimal path
public class PathFindingAlgorithmArcher : MonoBehaviour
{
    public int rangeX = 10; 
    public int rangeY = 10; 

    // Method to transmit next point the gameObject is expected to go
    public delegate void MoveTowards(Vector2 value);
    MoveTowards moveTowardsDelegate = null;

    TilemapToMatrix tilemapToMatrix;  // Reference to the TilemapToMatrix script

    Transform target;
    public float pathUpdateIntervalSeconds = 0.1f;  // How often to check for path updates (in seconds)
    
    public bool displayPathLine = true;

    Vector2[] path = null;  // Private array of positions to move along (matrix coordinates)
    int currentTargetIndex = 0;  // Index of the current target position
    LineRenderer lineRenderer;  // LineRenderer component for visualizing the path

    
    Coroutine updatePathCoroutine = null;
    Coroutine followPathCoroutine = null;

    void Awake()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        tilemapToMatrix = GameObject.FindGameObjectWithTag("Scenario").GetComponent<TilemapToMatrix>();

        // Get the LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();


    }


    public void SetMoveTowardsDelegate(MoveTowards func)
    {
        moveTowardsDelegate = func;
    }

    public void StopSeeking()
    {
        if (followPathCoroutine != null)
        {
            StopCoroutine(followPathCoroutine);
            StopCoroutine(updatePathCoroutine);
            followPathCoroutine = null;
            updatePathCoroutine = null;
            ClearPathLine();
        }
    }

    public void StartSeekingTarget(Transform transform)
    {
        target = transform;
        if (followPathCoroutine == null)
        {
            followPathCoroutine = StartCoroutine(UpdatePathForMovingTarget());
            updatePathCoroutine = StartCoroutine(MoveAlongPath());
        }
    }


    IEnumerator UpdatePathForMovingTarget()
    {
        while (true)
        {
            UpdatePath(target.position);
            // Wait for the next fixed update to prevent busy waiting
            yield return new WaitForSeconds(pathUpdateIntervalSeconds);
        }
    }

    // Get Path with Search Algorithm
    void UpdatePath(Vector2 targetPos)
    {
        Vector2 enemyPosition = tilemapToMatrix.WalkableMatrixToMatrix(transform.position);
        Vector2 targetPosition = tilemapToMatrix.WalkableMatrixToMatrix(targetPos);
        Vector2[] newPath = CalculatePath(enemyPosition, targetPosition);

        // Check if a valid new path is found
        if (newPath != null && newPath.Length > 0)
        {
            path = newPath;
            currentTargetIndex = 0;  // Reset target index when the path is updated
            
            if (displayPathLine) 
            {
                DisplayPathLine();  
            }
        }
        else
        {
            // Clear the line if no valid path
            lineRenderer.positionCount = 0;
        }
        
    }

    // Helper method to get adjacent positions in 4 directions (up, down, left, right)
    IEnumerable<Vector2> GetAdjacentPositions(Vector2 position)
    {
        return new List<Vector2>
        {
            new Vector2(position.x, position.y - 1),   // Down
            new Vector2(position.x + 1, position.y),  // Right
            new Vector2(position.x - 1, position.y),  // Left
            new Vector2(position.x, position.y + 1)  // Up            
        };
    }

    Vector2[] CalculatePath(Vector2 start, Vector2 goal)
    {
        List<Vector2> queue = new List<Vector2>();  // List used as a priority queue
        HashSet<Vector2> visited = new HashSet<Vector2>();
        //Dictionary<Vector2, int> acummulated = new Dictionary<Vector2, int>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

        // Define the two main goals
        Vector2 goal1 = new Vector2(goal.x, rangeY);  // goal with y = rangeY
        Vector2 goal2 = new Vector2(rangeX, goal.y);  // goal with x = rangeX

        queue.Add(start);
        visited.Add(start);


        while (queue.Count > 0)
        {
            queue.Sort((a, b) =>
                Mathf.Min(Vector2.Distance(a, goal1), Vector2.Distance(a, goal2))
                .CompareTo(Mathf.Min(Vector2.Distance(b, goal1), Vector2.Distance(b, goal2)))
            );

            Vector2 current = queue[0];  // Take the closest position to either goal
            queue.RemoveAt(0);  // Remove the position from the queue

            // Check if we reached either of the goals
            if (current == goal1 || current == goal2)
            {
                return ReconstructPath(cameFrom, current);
            }

            // Explore neighbors
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
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


    bool IsWalkable(Vector2 position)
    {
        if (position.x < 0 || position.x >= tilemapToMatrix.WalkableMatrix.GetLength(0) ||
            position.y < 0 || position.y >= tilemapToMatrix.WalkableMatrix.GetLength(1))
        {
            return false;  // Out of bounds
        }

        return tilemapToMatrix.WalkableMatrix[(int)position.x, (int)position.y].isWalkable;
    }

    

    // Debug info
    void DisplayPathLine()
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


    void ClearPathLine()
    {
        lineRenderer.positionCount = 0;  // Set position count to 0 to clear the line
    }



    // Get result
    Vector2[] ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
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




    // Follow along created path

    IEnumerator MoveAlongPath()
    {
        while (true)
        {
            if (path == null || path.Length == 0)
            {
                yield return new WaitForFixedUpdate();  // Wait for the path to be calculated
            }
            else
            {
                // Move towards points of the graph
                while (currentTargetIndex < path.Length)
                {
                    // Get the current target position from the path
                    Vector2 targetMatrixPos = path[currentTargetIndex];
                    Vector3 pathPointWorldPos = tilemapToMatrix.WalkableMatrix[(int)targetMatrixPos.x, (int)targetMatrixPos.y].worldPosition;

                    // Move towards the target position
                    while (Vector2.Distance(transform.position, pathPointWorldPos) > 0.1f)
                    {
                        if (moveTowardsDelegate != null) moveTowardsDelegate.Invoke(pathPointWorldPos);

                        yield return new WaitForFixedUpdate();  // Wait for the next frame
                    }

                    // Move to the next target position
                    currentTargetIndex++;
                }

                // Reset target index to start over for a new path
                currentTargetIndex = 0;
                path = null;  // Clear the path until recalculated
            }

            yield return new WaitForFixedUpdate();  // Wait until next frame if no path is available
        }
    }
}
