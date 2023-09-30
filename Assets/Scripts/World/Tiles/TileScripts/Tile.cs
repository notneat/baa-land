using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Tile
{
    [SerializeField] private TileData tile;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int ID;
    [SerializeField] private string tileName;
    [SerializeField] private float minThreshold;
    [SerializeField] private float maxThreshold;

    public Tile(TileData tileData, int tileID, float tileMinThreshold, float TilemaxThreshold, string _tileName, GameObject tileObject)
    {
        tile = tileData;
        ID = tileID;
        tileName = _tileName.ToLower();
        minThreshold = tileMinThreshold;
        maxThreshold = TilemaxThreshold;
        tilePrefab = tileObject;
    }

    public TileData GetTile() { return tile; }
    public GameObject GetTilePrefab() { return tilePrefab; }
    public int GetTileID() { return ID; }
    public float GetMinThreshold() { return minThreshold; }
    public float GetMaxThreshold() { return maxThreshold; }
    public string GetTileName() {  return tileName; }
    public void OverrideWith(TileData tileData)
    {
        if (tileData != null)
        {
            tile = tileData;
            ID = tileData.ID;
            tileName = tileData.tileName.ToLower();
            minThreshold = tileData.minThreshold;
            maxThreshold = tileData.maxThreshold;
            tilePrefab = tileData.prefab;
        }
    }
}
