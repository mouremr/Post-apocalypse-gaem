using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class SetworldBounds : MonoBehaviour
{
    private void Awake()
    {
        Tilemap tilemap = GetComponent<Tilemap>();
        tilemap.CompressBounds();
        
        
        Vector3 worldMin = tilemap.CellToWorld(tilemap.cellBounds.min);
        Vector3 worldMax = tilemap.CellToWorld(tilemap.cellBounds.max);

        // BoundsInt cellBounds = tilemap.cellBounds;

        // Vector3 worldMin = tilemap.CellToWorld(cellBounds.min);
        // Vector3 worldMax = tilemap.CellToWorld(cellBounds.max);

        Globals.WorldBounds = new Bounds(
            (worldMin + worldMax) / 2 ,
            worldMax
        );
        //Debug.Log($"World bounds min: {Globals.WorldBounds.min}, max: {Globals.WorldBounds.max}");
    }
}
