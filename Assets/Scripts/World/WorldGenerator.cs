using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float worldSize;
    [SerializeField] private float perlinScale;
    [SerializeField] private Vector2 perlinOffset;
    [SerializeField] private float minDistanceFromHouse;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask waterMask;
    private bool worldGenerationComplete = false;

    [Header("Thresholds")]

    [SerializeField] private float minTreeThreshold;
    [SerializeField] private float maxTreeThreshold;
    [SerializeField] private float treeSpawnChance;
    [Space(20)]
    [SerializeField] private float minLakeThreshold;
    [SerializeField] private float maxLakeThreshold;

    [Header("Prefabs")]

    [SerializeField] private List<TileData> tiles = new List<TileData>();
    [SerializeField] private List<Tile> tileInstances = new List<Tile>();

    [SerializeField] private List<FeatureData> features = new List<FeatureData>();
    [SerializeField] private List<Feature> featureInstances = new List<Feature>();

    [SerializeField] private GameObject lakePrefab;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject housePrefab;

    [Header("Events")]
    public GameEvent onGenerationComplete;

    [Header("Shelfs")]
    [SerializeField] private GameObject featureSelf;
    [SerializeField] private GameObject tileShelf;

    private bool houseSpawned = false;
    private Vector3 housePosition;

    private float expectedTilesAmount;
    private int tilesSpawnedCount;
    private int featuresSpawnedCount;
    private int featuresThatCantSpawnCound;
    private int tilesThatCantSpawnCount;

    private void Start()
    {
        InitializeTiles();
        expectedTilesAmount = worldSize * worldSize;
        GetRandomOffset();
        SpawnTiles();
    }

    private void InitializeTiles()
    {
        foreach (TileData tileData in tiles)
        {
            Tile tileInstance = new Tile(tileData, tileData.ID, tileData.minThreshold, tileData.maxThreshold, tileData.tileName);
            tileInstances.Add(tileInstance);
        }
    }
    
    private void SpawnTiles()
    {
        Vector3 offset = new Vector3(worldSize * 2 * 0.5f, 0, worldSize * 2 * 0.5f);
        
        SpawnHouse();

        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                Vector3 spawnPosition = new Vector3(x * 2, 0, z * 2) - offset;
                float perlinNoise = GetPerlinNoiseValue(x, z, perlinScale, perlinOffset); //TODO: Manipular perlin noise
                bool canSpawnTiles = CanSpawnTile(spawnPosition);

                if (canSpawnTiles)
                {
                    float distanceToHouse = GetDistanceFromHouse(spawnPosition);

                    // Check if the distance to the house is greater than the minimum distance

                    foreach (Tile tileInstance in tileInstances)
                    {
                        if (distanceToHouse > minDistanceFromHouse)
                        {
                            if (tileInstance.GetTileName() == "water")
                            {
                                if (perlinNoise >= tileInstance.GetMinThreshold() && perlinNoise <= tileInstance.GetMaxThreshold())
                                {
                                    Vector3 waterYOffset = new Vector3(spawnPosition.x, spawnPosition.y - 0.45f, spawnPosition.z);

                                    GameObject tilePrefab = tileInstance.GetTile().prefab;
                                    GameObject tile = Instantiate(tilePrefab, waterYOffset, Quaternion.identity);
                                    tile.transform.SetParent(tileShelf.transform);
                                    tile.name = tileInstance.GetTileName();
                                    tilesSpawnedCount++;

                                    if ((float)expectedTilesAmount < tilesSpawnedCount + featuresSpawnedCount)
                                    {
                                        WorldFinishedGenerating();
                                    }

                                    break; // Exit the loop after finding the matching tile.
                                }
                            }
                        }
                        if (tileInstance.GetTileName() == "ground")
                        {
                            if (perlinNoise >= tileInstance.GetMinThreshold() && perlinNoise <= tileInstance.GetMaxThreshold())
                            {
                                // Generate ground tile if it's within the threshold
                                GameObject tilePrefab = tileInstance.GetTile().prefab;
                                GameObject tile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                                tile.transform.SetParent(tileShelf.transform);
                                tile.name = tileInstance.GetTileName();
                                tilesSpawnedCount++;

                                if ((float)expectedTilesAmount < tilesSpawnedCount + featuresSpawnedCount)
                                {
                                    WorldFinishedGenerating();
                                }

                                break; // Exit the loop after finding the matching tile.
                            }
                        }
                    }
                }
                SpawnFeatures(perlinNoise, spawnPosition);
            }
        }
    }

    private void SpawnFeatures(float perlinNoise, Vector3 spawnPosition)
    {
        Vector3 featurePosition = new Vector3(spawnPosition.x, 0, spawnPosition.z);

        float distanceToHouse = GetDistanceFromHouse(featurePosition);

        if (distanceToHouse > minDistanceFromHouse)
        {
            foreach (FeatureData featureData in features)
            {
                if (perlinNoise >= featureData.minThreshold && perlinNoise < featureData.maxThreshold)
                {
                    float randomValue = Random.value;

                    if (randomValue < featureData.spawnChance)
                    {
                        GameObject featurePrefab = featureData.prefab[Random.Range(0, featureData.prefab.Length)];

                        if (featurePrefab != null)
                        {
                            Vector3 featureSpawnPosition = new Vector3(featurePosition.x, featurePosition.y, featurePosition.z);

                            float zOffset = 0f;

                            if (featureData.ID == 0 || featureData.featureName == "tree")
                            {
                                featureSpawnPosition.y += 5;
                                zOffset = Random.Range(-0.1f, 0.1f);
                            }

                            featureSpawnPosition.z += zOffset;

                            GameObject feature = Instantiate(featurePrefab, featureSpawnPosition, featureData.rotationOffset);
                            feature.transform.SetParent(featureSelf.transform);
                            featuresSpawnedCount++;
                        }
                    }

                    break;
                }
            }
        }
        else
        {
            featuresThatCantSpawnCound++;
        }

        if ((float)expectedTilesAmount < tilesSpawnedCount + featuresSpawnedCount)
        {
            WorldFinishedGenerating();
        }
    }

    private void SpawnHouse()
    {
        if (!houseSpawned)
        {
            housePosition = new Vector3(0, 0, 6);

            GameObject house = Instantiate(housePrefab, housePosition, Quaternion.identity);
            house.transform.SetParent(this.transform);
            houseSpawned = true;
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

    private float GetDistanceFromHouse(Vector3 position)
    {
        return Vector3.Distance(housePosition, position);
    }

    private bool CanSpawnTile(Vector3 featurePosition)
    {
        Vector3 rayPosition = new Vector3(featurePosition.x, featurePosition.y + 5, featurePosition.z);

        Ray ray = new Ray(rayPosition, Vector3.down);

        // Check if the raycast doesn't hit anything.
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, waterMask))
        {
            return true;
        }
        tilesThatCantSpawnCount++;
        return false;
    }

    private void WorldFinishedGenerating()
    {
        if(!worldGenerationComplete)
        {
            Debug.Log("Amount of tiles spawned: " + tilesSpawnedCount);
            Debug.Log("Amount of features spawned: " + featuresSpawnedCount);
            int totalAmountOfObjects = tilesSpawnedCount + featuresSpawnedCount;
            Debug.Log("Amount of total objects spawned: " + totalAmountOfObjects);

            Debug.Log("Amount of tiles that couldn't spawn: " + tilesThatCantSpawnCount);
            Debug.Log("Amount of features that couldn't spawn: " + featuresThatCantSpawnCound);
            Debug.Log("World Generated");
            worldGenerationComplete = true;
            onGenerationComplete.Raise(this);
        }
    }
}
