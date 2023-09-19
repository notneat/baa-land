using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "World/Common_Tile")]
public class TileData : ScriptableObject
{
    public int ID;
    public string tileName;
    public float minThreshold;
    public float maxThreshold;
    public GameObject prefab;
}
