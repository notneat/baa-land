using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float worldSize;
    [SerializeField] private float perlinScale;
    [SerializeField] private Vector2 perlinOffset;
    [SerializeField] private float minDistanceFromHouse;

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
    [SerializeField] private GameObject housePrefab;

    [Header("Feature Shelfs")]
    [SerializeField] private GameObject treeFeatureShelf;
    [SerializeField] private GameObject lakeFeatureShelf;
    [SerializeField] private GameObject tileFeatureShelf;

    private bool houseSpawned = false;

    private void Start()
    {
        GetRandomOffset();
        SpawnTiles();
    }

    private void SpawnTiles()
    {

        Vector3 offset = new Vector3(worldSize * 2 * 0.5f, 0, worldSize * 2 * 0.5f);

        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {

                Vector3 spawnPosition = new Vector3(x * 2, 0, z * 2) - offset;

                float perlinNoise = GetPerlinNoiseValue(x, z, perlinScale, perlinOffset);

                SpawnFeatures(perlinNoise, spawnPosition);

                Vector3 featureSpawnPosition = new Vector3(spawnPosition.x, 0, spawnPosition.y);

                bool canSpawnTiles = CanSpawnTile(featureSpawnPosition);

                if (canSpawnTiles)
                {
                    GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    tile.transform.SetParent(tileFeatureShelf.transform);

                    tile.name = perlinNoise.ToString();
                }
            }
        }
    }

    private void SpawnFeatures(float perlinNoise, Vector3 spawnPosition)
    { 
        Vector3 freaturePosition = new Vector3(spawnPosition.x, 0, spawnPosition.z);

        if (!houseSpawned)
        {
            Vector3 housePosition = new Vector3(0, 0, 6);

            GameObject house = Instantiate(housePrefab, housePosition, Quaternion.identity);
            house.transform.SetParent(this.transform);
            houseSpawned = true;
        }

        float distanceToHouse = Vector3.Distance(spawnPosition, GetHousePosition());

        if (distanceToHouse > minDistanceFromHouse)
        {
            float randomValue = Random.value;

            if (randomValue < treeSpawnChance)
            {
                if (perlinNoise >= minTreeThreshold && perlinNoise < maxTreeThreshold)
                {
                    GameObject tree = Instantiate(treePrefab, freaturePosition, Quaternion.identity);
                    tree.transform.SetParent(treeFeatureShelf.transform);
                }
            }

            if (perlinNoise >= minLakeThreshold && perlinNoise < maxLakeThreshold)
            {
                GameObject lake = Instantiate(lakePrefab, freaturePosition, Quaternion.identity);
                lake.transform.SetParent(lakeFeatureShelf.transform);
            }
        }
        else
        {
            Debug.LogWarning("Cant spawn features, since: " + distanceToHouse);
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

    private Vector3 GetHousePosition()
    {
        GameObject houseObject = GameObject.FindGameObjectWithTag("House");
        Vector3 housePosition = houseObject.transform.position;
        Debug.Log(houseObject);
        Debug.Log(housePosition);

        return housePosition;
    }

    private bool CanSpawnTile(Vector3 featurePosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(featurePosition, Vector3.down, out hit))
        {
            Debug.DrawRay(featurePosition, Vector3.down * hit.distance, Color.red);

            if (hit.distance < 1.0f)
            {
                return false;
            }
        }
        return true;
    }
}
