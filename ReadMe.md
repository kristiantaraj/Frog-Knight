# GreedyBFS_AI

GreedyBFS_AI is a Unity script that implements the Greedy Best-First Search algorithm to find a path from a starting position to a goal position on a tile-based map. The script uses a Tilemap to represent the walkable and unwalkable tiles and a Grid component for cell-to-world position conversions.

## Features

- Greedy Best-First Search algorithm implementation.
- Dynamic pathfinding from a starting position to a goal position.
- Smooth movement of a player object along the computed path.
- Click-based input for setting the goal position.

## Getting Started

To use the GreedyBFS_AI script in your Unity project, follow these steps:

1. Attach the script to a GameObject in your scene.
2. Assign the required references in the script inspector:
   - `mainCamera`: The camera used to convert mouse positions to world positions.
   - `player`: The GameObject representing the player character.
   - `tilemap`: The Tilemap component representing the walkable tiles.
   - `unwalkableTilemap`: The Tilemap component representing the unwalkable tiles.
   - `grid`: The Grid component used for cell-to-world position conversions.
3. Make sure you have defined walkable and unwalkable tiles in your tilemap.
4. Run the scene and click on the tilemap to set the goal position. The script will calculate the path and move the player along it.

## How It Works

1. The `Update` method checks for a mouse click and triggers the pathfinding process.
2. The `greedyBFS` method implements the Greedy Best-First Search algorithm:
   - It maintains a fringe of nodes to visit and a list of visited nodes.
   - It selects the node with the lowest estimated cost to the goal from the fringe.
   - It expands the selected node by adding its neighbors to the fringe.
   - It repeats these steps until the goal is reached or no more nodes are available.
   - It returns the computed path as an array of positions.
3. The `reconstructPath` method backtracks from the goal node to the start node using the `cameFrom` dictionary to reconstruct the path.
4. The `getLowestCostNode` method calculates the lowest estimated cost node based on the sum of `f1` (cost to reach the current node from the start) and `f2` (estimated cost to the goal) functions.
5. The `getNeighbors` method returns the neighboring nodes of a given position on the grid.
6. The `isWalkable` method checks if a position is walkable by verifying its presence in the walkable tilemap or absence in the unwalkable tilemap.
7. The `MovePlayerAlongPath` coroutine moves the player object smoothly along the computed path.

## License

This project is licensed under the [MIT License](LICENSE).

## Acknowledgments

- The Greedy Best-First Search algorithm is a well-known pathfinding algorithm.
- This script was developed for educational purposes as an example of pathfinding implementation in Unity.

Feel free to customize and use the script in your own Unity projects!

**Note:** This README assumes that you have a basic understanding of Unity and C# programming. If you need further assistance, refer to the Unity documentation or seek additional resources.
