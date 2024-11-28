using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(LineRenderer))]

// Uses main tile map along with tile collision data to determine optimal path
public class PathFindingAlgorithm : MonoBehaviour
{
    // Method to transmit next point the gameObject is expected to go
    public delegate void MoveTowards(Vector2 value);
    MoveTowards moveTowardsDelegate = null;

    TilemapToMatrix tilemapToMatrix;  // Reference to the TilemapToMatrix script

    Transform target;
    public float pathUpdateIntervalSeconds = 0.1f;  // How often to check for path updates (in seconds)
    
    public bool displayPathLine = true;
    // Threshold for enemy density; if the number of enemies in a given area is higher than this, we try to surround the player
    public int maxEnemiesInPath = 3;

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

    // Modify the UpdatePath method to consider enemy density
    void UpdatePath(Vector2 targetPos)
    {
        Vector2 enemyPosition = tilemapToMatrix.WalkableMatrixToMatrix(transform.position);
        Vector2 targetPosition = tilemapToMatrix.WalkableMatrixToMatrix(targetPos);
        targetPosition = new Vector2(targetPosition.x, targetPosition.y);
        targetPosition = findClosestWalkable(targetPosition);

        // Check enemy density along the path
        int enemiesInPath = CountEnemiesInPath(enemyPosition, targetPosition);

        if (enemiesInPath > maxEnemiesInPath)
        {
            // If too many enemies, try to find an alternative route that surrounds the player
            Vector2 alternativeTarget = FindSurroundingTarget(targetPosition);
            Vector2[] newPath = CalculatePath(enemyPosition, alternativeTarget);

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
                // Fallback: If no path found, continue with the original path
                Vector2[] newPathFallback = CalculatePath(enemyPosition, targetPosition);
                path = newPathFallback;
                currentTargetIndex = 0;
                if (displayPathLine)
                {
                    DisplayPathLine();
                }
            }
        }
        else
        {
            // Proceed with the original path if there are no issues
            Vector2[] newPath = CalculatePath(enemyPosition, targetPosition);
            if (newPath != null && newPath.Length > 0)
            {
                path = newPath;
                currentTargetIndex = 0;
                if (displayPathLine)
                {
                    DisplayPathLine();
                }
            }
            else
            {
                lineRenderer.positionCount = 0;
            }
        }
    }

    // Count the number of enemies in a given path range
    int CountEnemiesInPath(Vector2 start, Vector2 end)
    {
        int count = 0;
        List<Vector2> pathPositions = GetPathPositionsBetween(start, end);

        foreach (Vector2 pos in pathPositions)
        {
            // Check if there's an enemy in this position (assuming you have a way to check for enemies on the map)
            if (IsEnemyAtPosition(pos))
            {
                count++;
            }
        }
        return count;
    }

    // Example of how to get path positions between two points
    List<Vector2> GetPathPositionsBetween(Vector2 start, Vector2 end)
    {
        List<Vector2> positions = new List<Vector2>();
        Vector2 current = start;
        while (current != end)
        {
            positions.Add(current);
            // You may need to calculate the next position based on your pathfinding algorithm
            // (e.g., using the same logic as BFS/A* to move step-by-step)
            current = GetNextStepTowardsEnd(current, end);  // This would be a method to move step by step
        }
        return positions;
    }

    // Find an alternative destination to surround the player
    Vector2 FindSurroundingTarget(Vector2 originalTarget)
    {
        // Find neighboring tiles around the target position
        List<Vector2> surroundingTiles = new List<Vector2>
    {
        new Vector2(originalTarget.x + 1, originalTarget.y),  // Right
        new Vector2(originalTarget.x - 1, originalTarget.y),  // Left
        new Vector2(originalTarget.x, originalTarget.y + 1),  // Up
        new Vector2(originalTarget.x, originalTarget.y - 1)   // Down
    };

        // Filter out non-walkable or blocked tiles
        surroundingTiles = surroundingTiles.Where(pos => IsWalkable(pos)).ToList();

        // Choose a random surrounding tile (or implement a more strategic surrounding logic)
        if (surroundingTiles.Count > 0)
        {
            return surroundingTiles[UnityEngine.Random.Range(0, surroundingTiles.Count)];
        }

        return originalTarget;  // Fallback to original target if no valid surrounding positions
    }

    // Example of how to get the next step towards the end in a pathfinding algorithm
    Vector2 GetNextStepTowardsEnd(Vector2 current, Vector2 goal)
    {
        // The next step can be obtained from your pathfinding algorithm
        // For now, we can return the next node in the path calculated previously.

        // Use the CalculatePath function to determine the path from current to goal
        // This should already give us a series of positions along the path.
        List<Vector2> path = new List<Vector2>(CalculatePath(current, goal));

        // If the path has more than 1 step, return the second step (the next step after the current position)
        if (path.Count > 1)
        {
            return path[1];  // The next position in the path
        }

        // If no valid path is found, return the current position to avoid moving to an invalid position
        return current;
    }


    // Method to check if there's an enemy at a given position (you need to implement enemy detection logic)
    bool IsEnemyAtPosition(Vector2 position)
    {
        // Check if there is an enemy at the given position
        // This assumes you have a way to detect enemies on the tilemap
        return false;  // Implement your own logic to detect enemies
    }

    //bredth find algorithm to find the closest walkable tile. This fix the infinite loop if the player body 
    //is located in a non-walkable place
    Vector2 findClosestWalkable(Vector2 actualPosition)
    {
        // Queue for BFS
        Queue<Vector2> positions = new Queue<Vector2>();
        positions.Enqueue(actualPosition);
        // Set to track visited nodes
        HashSet<Vector2> visited = new HashSet<Vector2>();

        while (positions.Count > 0)
        {
            Vector2 current = positions.Dequeue();

            // If the current position is walkable, return it
            if (IsWalkable(current))
            {
                return current;
            }

            // Mark the current position as visited
            if (!visited.Contains(current))
            {
                visited.Add(current);

                // Add adjacent positions to the queue
                foreach (Vector2 adjacent in GetAdjacentPositions(current))
                {
                    if (!visited.Contains(adjacent))
                    {
                        positions.Enqueue(adjacent);
                    }
                }
            }
        }

        // If no walkable position is found, return a default value (or handle appropriately)
        throw new Exception("No walkable position found.");
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
                     // Skip diagonal movements if the corresponding non-diagonal paths are non-walkable
                    // if (Mathf.Abs(dx) == Mathf.Abs(dy)) // Check for diagonal movement
                    // {
                    //     if (!IsWalkable(current + new Vector2(dx, 0)) || !IsWalkable(current + new Vector2(0, dy)))
                    //         continue;
                    // }

                    // if (Mathf.Abs(dx) == Mathf.Abs(dy)) continue; // Skip diagonal movements

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
