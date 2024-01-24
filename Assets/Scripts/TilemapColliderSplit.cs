using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColliderSplit : MonoBehaviour
{
    public GameObject grassColliderPrefab;

    void Awake()
    {
        GenerateTileColliders();
    }

    void GenerateTileColliders()
    {
        Tilemap grassTilemap = GetComponent<Tilemap>();
        if (grassTilemap == null)
        {
            Debug.LogError("Tilemap component not found on the GameObject.");
            return;
        }

        BoundsInt bounds = grassTilemap.cellBounds;

        foreach (var position in bounds.allPositionsWithin)
        {
            if (grassTilemap.HasTile(position))
            {
                Vector3 worldPosition = grassTilemap.GetCellCenterWorld(position);
                Instantiate(grassColliderPrefab, worldPosition, Quaternion.identity);
            }
        }
        Destroy(grassColliderPrefab);
    }
}
