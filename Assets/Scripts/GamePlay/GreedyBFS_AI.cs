using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GreedyBFS_AI : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    public GameObject player;
    public Tilemap tilemap;
    public Grid grid;
    public Tilemap unwalkableTilemap;
    public Vector3Int startPos;
    public Vector3Int goalPos;
    public Vector3Int[] Finalpath;
    public float zPosition = 0;
    public float speed = 10f;
    public float smoothTime = 0.3f;
    private Vector3 targetPos;
    private Vector3 velocity = Vector3.zero;
    private bool isMoving = false;



    void Update()
    {


        Vector3 mp = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPosition + 2));
        startPos = tilemap.WorldToCell(player.transform.position);
        goalPos= tilemap.WorldToCell(mp);
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine("MovePlayerAlongPath");
            velocity = Vector3.zero;
            isMoving = false;
            Finalpath = greedyBFS(startPos, goalPos, tilemap, unwalkableTilemap, grid);
            string arrayString = string.Join(", ", Finalpath);
            Debug.Log(arrayString);
            // Start the coroutine to move the player along the path

            if (Finalpath != null && Finalpath.Length > 0)
            {
                isMoving = true;
                StartCoroutine(MovePlayerAlongPath(Finalpath));
            }
            else
            {
                Finalpath = new Vector3Int[0];
            }
        }

    }



  Vector3Int[] greedyBFS(Vector3Int start, Vector3Int goal, Tilemap tilemap, Tilemap unwalkableTilemap, Grid grid)
{
    // Check if the start and goal positions are walkable
    if (!isWalkable(start, tilemap, unwalkableTilemap) || !isWalkable(goal, tilemap, unwalkableTilemap))
    {
        return null;
    }

    List<Vector3Int> fringe = new List<Vector3Int> {start};
    List<Vector3Int> visited = new List<Vector3Int>();
    Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();

    // While there are nodes in the fringe
    while (fringe.Count > 0)
    {
        // Get the node in the fringe with the lowest estimated cost to the goal
        Vector3Int current = getLowestCostNode(fringe, goal);
        if (current == goal)
        {
            // If we have reached the goal, we can reconstruct the path and return it
            List<Vector3Int> path = reconstructPath(cameFrom, current,tilemap,unwalkableTilemap);
            return path.ToArray();
        }

        // Remove the current node from the fringe
        fringe.Remove(current);
        // Add current node to visited list
        visited.Add(current);
        // Get the neighbors of the current node
        Vector3Int[] neighbors = getNeighbors(current, grid);
            // Loop through the neighbors
            foreach (Vector3Int neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && !fringe.Contains(neighbor) && isWalkable(neighbor, tilemap, unwalkableTilemap))
                {
                // Add the neighbor to the fringe
                fringe.Add(neighbor);
                // Save the current node as the neighbor's predecessor in the path
                cameFrom[neighbor] = current;
                }
            }
    }

    return null;
}


    List<Vector3Int> reconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current, Tilemap tilemap, Tilemap unwalkableTilemap)
    {
        List<Vector3Int> path = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            if (isWalkable(current, tilemap, unwalkableTilemap))
            {
                path.Insert(0, current);
            }
            else
            {
                // If the tile is not walkable, return an empty path
                return new List<Vector3Int>();
            }
        }
        return path;
    }

    Vector3Int getLowestCostNode(List<Vector3Int> fringe, Vector3Int goal)
    {
        // Initialize variables to store the lowest cost and the corresponding node
        float lowestCost = float.MaxValue;
        Vector3Int lowestCostNode = new Vector3Int();
        // Loop through the nodes in the fringe
        foreach (Vector3Int node in fringe)
        {
            // Calculate the f1
            float f = f1(node) + f2(node, goal);



            // If this cost is lower than the current lowest cost, update the lowest cost and the corresponding node
            if (f < lowestCost)
            {
                lowestCost = f;
                lowestCostNode = node;
            }
        }

        // Return the node with the lowest cost
        return lowestCostNode;
    }


float f2(Vector3Int node, Vector3Int goal)
{
    // Estimate the cost to the goal as the Euclidean distance between the node and the goal
    float dx = node.x - goal.x;
    float dy = node.y - goal.y;
    return (float)Math.Sqrt(dx * dx + dy * dy);
}

float f1(Vector3Int node)
{
    // Assume that the cost of reaching any node from the start is 1
    return 1;
}

Vector3Int[] getNeighbors(Vector3Int current, Grid grid)
{
    List<Vector3Int> neighbors = new List<Vector3Int>();
    Vector3Int neighbor = new Vector3Int();
    // Check the neighbor to the right
    neighbor = new Vector3Int(current.x + 1, current.y, current.z);
    if (isWalkable(neighbor,tilemap,unwalkableTilemap))
        neighbors.Add(neighbor);
    // Check the neighbor to the left
    neighbor = new Vector3Int(current.x - 1, current.y, current.z);
    if (isWalkable(neighbor,tilemap,unwalkableTilemap))
        neighbors.Add(neighbor);
    // Check the neighbor above
    neighbor = new Vector3Int(current.x, current.y + 1, current.z);
    if (isWalkable(neighbor,tilemap,unwalkableTilemap))
        neighbors.Add(neighbor);
    // Check the neighbor below
    neighbor = new Vector3Int(current.x, current.y - 1, current.z);
    if (isWalkable(neighbor,tilemap,unwalkableTilemap))
        neighbors.Add(neighbor);

    return neighbors.ToArray();
}



    bool isWalkable(Vector3Int pos, Tilemap tilemap, Tilemap unwalkableTilemap)
    {
        // Check if the tile at the given position exists in the tilemap
        if (tilemap.HasTile(pos))
        {
            return true;
        }
        // Check if the tile at the given position exists in the unwalkable tilemap
        else if(unwalkableTilemap.HasTile(pos))
        {
            return false;
        }
        else
        {
            return false;
        }
    }



    IEnumerator MovePlayerAlongPath(Vector3Int[] path)
    {
        for (int i = 0; i < path.Length; i++)
        {
            targetPos = grid.CellToWorld(path[i]) + new Vector3(0.5f, 0.5f, 0);

            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
                yield return null;
            }
            if(i == path.Length - 1)
            {
            isMoving = false;
            }
        }
    }



}