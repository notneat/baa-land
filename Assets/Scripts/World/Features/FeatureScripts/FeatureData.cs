using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Feature", menuName = "World/Common_Feature")]
public class FeatureData : ScriptableObject
{
    public int ID;
    public string featureName;
    public float minThreshold;
    public float maxThreshold;
    public float spawnChance;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;
    public GameObject[] prefab;
}
