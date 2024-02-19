using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;

public class World : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float worldSize;
    [SerializeField] private float perlinScale;
    [SerializeField] private Vector2 perlinOffset;
    [SerializeField] private LayerMask waterMask;
    [SerializeField] private int minWaterTiles;
    [SerializeField] private float worldBuildSpeed; //Apenas para debug
    public bool restartWorld = false;
    private bool worldGenerationComplete = false;
    public int[,] worldGrid;
    private List<GameObject> worldObjects = new List<GameObject>();

    [Header("Prefabs")]

    [SerializeField] private List<TileData> tiles = new List<TileData>();
    private List<Tile> tileInstances = new List<Tile>();

    [SerializeField] private List<FeatureData> features = new List<FeatureData>();
    private List<Feature> featureInstances = new List<Feature>();

    [Header("Events")]
    public GameEvent onGenerationComplete;

    [Header("Shelfs")]
    [SerializeField] private GameObject featureSelf;
    [SerializeField] private GameObject tileShelf;

    public float expectedTilesAmount;
    public int tilesSpawnedCount;
    public int featuresSpawnedCount;
    public int numOfWaterTiles;

    private void Start()
    {
        restartWorld = false;
        InitializeWorld();
    }

    private void Update()
    {
        if(restartWorld)
        {
            restartWorld = false;
            RestartWorld();
            Debug.LogWarning("Restarting World");
        }
    }

    private void InitializeWorld()
    {
        expectedTilesAmount = (worldSize * worldSize) - 1;
        GetRandomOffset();

        worldGrid = new int[(int)worldSize, (int)worldSize];
        for (int x = 0; x < worldSize; x++)
        {
            for (int z = 0; z < worldSize; z++)
            {
                worldGrid[x, z] = -1;
            }
        }

        foreach (TileData tileData in tiles)
        {
            Tile tileInstance = new Tile(tileData, tileData.ID, tileData.minThreshold, tileData.maxThreshold, tileData.tileName, tileData.prefab);
            tileInstances.Add(tileInstance);
        }

        foreach(FeatureData featureData in features)
        {
            Feature featureInstance = new Feature(featureData, featureData.ID, featureData.minThreshold, featureData.maxThreshold, featureData.featureName, featureData.prefab);
            featureInstances.Add(featureInstance);
        }

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
                bool canSpawnTiles = CanSpawnTile(spawnPosition);

                if (canSpawnTiles)
                {
                    foreach (Tile tileInstance in tileInstances)
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
                                numOfWaterTiles++;
                                worldObjects.Add(tile);
                                worldGrid[x, z] = 0;

                                if (tilesSpawnedCount == expectedTilesAmount)
                                {
                                    Debug.Log("tilesCount == expectedTiles");
                                    WorldFinishedGenerating();
                                    break;
                                }

                                break; // Exit the loop after finding the matching tile.
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
                                worldObjects.Add(tile);
                                worldGrid[x, z] = 1;

                                if (tilesSpawnedCount == expectedTilesAmount)
                                {
                                    Debug.Log("tilesCount == expectedTiles");
                                    WorldFinishedGenerating();
                                    break;
                                }

                                break; // Exit the loop after finding the matching tile.
                            }
                        }
                        worldGrid[x, z] = (tileInstance.GetTileName() == "ground") ? 1 : 0;
                    }
                }
                SpawnFeatures(perlinNoise, spawnPosition);
            }
        }
    }

    private void SpawnFeatures(float perlinNoise, Vector3 spawnPosition)
    {
        Vector3 featurePosition = new Vector3(spawnPosition.x, 0, spawnPosition.z);
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
                        worldObjects.Add(feature);
                    }
                }

                break;
            }
        }
    }

    private void RestartWorld()
    {
        foreach (GameObject tile in worldObjects)
        {
            Destroy(tile);
        }

        worldObjects.Clear();
        worldGenerationComplete = false;
        tilesSpawnedCount = 0;
        numOfWaterTiles = 0;
        featuresSpawnedCount = 0;
        GetRandomOffset();
        InitializeWorld();
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

    private bool CanSpawnTile(Vector3 featurePosition)
    {
        Vector3 rayPosition = new Vector3(featurePosition.x, featurePosition.y + 5, featurePosition.z);

        Ray ray = new Ray(rayPosition, Vector3.down);

        // Check if the raycast doesn't hit anything.
        if (!Physics.Raycast(ray, out RaycastHit hit, 100f, waterMask))
        {
            return true;
        }
        return false;
    }

    private void WorldFinishedGenerating()
    {
        if (numOfWaterTiles < minWaterTiles && tilesSpawnedCount < expectedTilesAmount)
        {
            RestartWorld();
        }
        else if (!worldGenerationComplete)
        {
            Debug.Log("Amount of tiles spawned: " + tilesSpawnedCount);
            Debug.Log("Amount of features spawned: " + featuresSpawnedCount);
            int totalAmountOfObjects = tilesSpawnedCount + featuresSpawnedCount;
            Debug.Log("Amount of total objects spawned: " + totalAmountOfObjects);
            Debug.Log("World Generated");
            worldGenerationComplete = true;
            onGenerationComplete.Raise(this);
        }
    }
}
