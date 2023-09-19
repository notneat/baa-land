using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Tile
{
    [SerializeField] private TileData tile;
    [SerializeField] private int ID;
    [SerializeField] private string tileName;
    [SerializeField] private float minThreshold;
    [SerializeField] private float maxThreshold;

    public Tile(TileData tileObject, int tileID, float tileMinThreshold, float TilemaxThreshold, string _tileName)
    {
        tile = tileObject;
        ID = tileID;
        tileName = _tileName.ToLower();
        minThreshold = tileMinThreshold;
        maxThreshold = TilemaxThreshold;
    }

    public TileData GetTile() { return tile; }
    public int GetTileID() { return ID; }
    public float GetMinThreshold() { return minThreshold; }
    public float GetMaxThreshold() { return maxThreshold; }
    public string GetTileName() {  return tileName; }

}
