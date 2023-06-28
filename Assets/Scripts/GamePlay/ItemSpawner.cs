using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] items;
    public Tilemap walkableTilemap;
    public float spawnInterval = 5f;

    void Start()
    {
        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        while (true)
        {
            // Get the bounds of the walkable tilemap
            BoundsInt bounds = walkableTilemap.cellBounds;

            // Choose a random location within the walkable tilemap
            int x = Random.Range(bounds.xMin, bounds.xMax);
            int y = Random.Range(bounds.yMin, bounds.yMax);

            // Check if the tile is walkable
            if (walkableTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
            {
                // Choose a random item to spawn
                GameObject item = items[Random.Range(0, items.Length)];

                // Spawn the item at the chosen location
                Instantiate(item, walkableTilemap.GetCellCenterWorld(new Vector3Int(x, y, 0)), Quaternion.identity);
            }

            // Wait for the specified spawn interval
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
