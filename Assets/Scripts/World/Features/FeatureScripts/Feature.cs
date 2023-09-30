using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Feature
{
    [SerializeField] private FeatureData feature;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int ID;
    [SerializeField] private string featureName;
    [SerializeField] private float minThreshold;
    [SerializeField] private float maxThreshold;

    public Feature(FeatureData featureObject, int featureID, float featureMinThreshold, float featureMaxThreshold, string _featureName, GameObject[] featurePrefabs)
    {
        feature = featureObject;
        ID = featureID;
        featureName = _featureName.ToLower();
        minThreshold = featureMinThreshold;
        maxThreshold = featureMaxThreshold;
        prefabs = featurePrefabs;
    }

    public FeatureData GetFeature() { return feature; }
    public int GetFeatureID() { return ID; }
    public GameObject GetFeaturePrefab(int index) { return prefabs[index]; }
    public float GetMinThreshold() { return minThreshold; }
    public float GetMaxThreshold() { return maxThreshold; }
    public string GetFeatureName() {  return featureName; }
}
