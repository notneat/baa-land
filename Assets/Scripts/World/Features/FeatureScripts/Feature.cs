using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Feature
{
    [SerializeField] private FeatureData feature;
    [SerializeField] private int ID;
    [SerializeField] private string featureName;
    [SerializeField] private float minThreshold;
    [SerializeField] private float maxThreshold;

    public Feature(FeatureData featureObject, int featureID, float featureMinThreshold, float featureMaxThreshold, string _featureName)
    {
        feature = featureObject;
        ID = featureID;
        featureName = _featureName.ToLower();
        minThreshold = featureMinThreshold;
        maxThreshold = featureMaxThreshold;
    }

    public FeatureData GetFeature() { return feature; }
    public int GetFeatureID() { return ID; }
    public float GetMinThreshold() { return minThreshold; }
    public float GetMaxThreshold() { return maxThreshold; }
    public string GetFeatureName() {  return featureName; }
}
