using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [SerializeField] private float worldSize = 25;
    [SerializeField] private float perlinScale;
    [SerializeField] private Vector2 perlinOffset;

    [Header("Thresholds")]

    [SerializeField] private float minTreeThreshold;
    [SerializeField] private float maxTreeThreshold;
    [SerializeField] private float treeSpawnChance;
    [Space(20)]
    [SerializeField] private float minLakeThreshold;
    [SerializeField] private float maxLakeThreshold;

    [Header("Prefabs")]
    [SerializeField] private GameObject lakePrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject treePrefab;


    private void Start()
    {
        GetRandomOffset();
        SpawnTiles();
    }

    private void SpawnTiles()
    {
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                Vector3 spawnPosition = new Vector3(x * 2, -0.1f, z * 2);

                float perlinNoise = GetPerlinNoiseValue(x, z, perlinScale, perlinOffset);
                Debug.Log(perlinNoise);

                SpawnFeatures(perlinNoise, spawnPosition);

                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                tile.transform.SetParent(this.transform);

                MeshRenderer tileRenderer = tile.GetComponent<MeshRenderer>();
                Color tileColor = Color.Lerp(Color.black, Color.white, perlinNoise);

                tileRenderer.material.color = tileColor;
            }
        }
    }

    private void SpawnFeatures(float perlinNoise, Vector3 spawnPosition)
    {

        Vector3 featurePosition = new Vector3(spawnPosition.x, + 8, spawnPosition.z);

        float randomValue = Random.value;

        if(randomValue < treeSpawnChance)
        {
            if (perlinNoise >= minTreeThreshold && perlinNoise < maxTreeThreshold)
            {
                GameObject tree = Instantiate(treePrefab, featurePosition, Quaternion.identity);
                tree.transform.SetParent(this.transform);
            }
        }
        
        else if (perlinNoise >= minLakeThreshold && perlinNoise < maxLakeThreshold)
        {
            GameObject lake = Instantiate(lakePrefab, featurePosition, Quaternion.identity);
            lake.transform.SetParent(this.transform);
        }
        
    }


    private float GetPerlinNoiseValue(int x, int z, float noiseScale, Vector2 noiseOffset)
    {
        float perlinX = (x + noiseOffset.x) * noiseScale;
        float perlinZ = (z + noiseOffset.y) * noiseScale;

        return Mathf.PerlinNoise(perlinX, perlinZ);
    }

    private Vector2 GetRandomOffset()
    {
        perlinOffset = new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000));

        return perlinOffset;
    }
}
