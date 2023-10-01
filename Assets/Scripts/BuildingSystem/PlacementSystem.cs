using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDataBase database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;

    private bool isBuilding;

    private GridData floorData, furnitureData;

    private List<GameObject> placeGameObjects = new List<GameObject>();

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        furnitureData = new();
  
    }

    public void StartStopPlacement(int ID)
    {
        if (!isBuilding)
        {
            StartPlacement(ID);
            isBuilding = true;
        }
        else
        {
            StopPlacement();
            isBuilding = false;
        }

    }

    private void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found: {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        preview.StartShowingPlacementPreview(database.objectsData[selectedObjectIndex].Prefab, database.objectsData[selectedObjectIndex].Size);
        inputManager.OnClick += PlaceStructure;
        inputManager.OnExit += StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        if(placementValidity == false)
        {
            return;
        }

        GameObject newObject = Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
        placeGameObjects.Add(newObject);
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        selectedData.AddObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size, database.objectsData[selectedObjectIndex].ID, placeGameObjects.Count -1);
        preview.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectsData[selectedObjectIndex].Size);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        preview.StopShowingPreview();
        inputManager.OnClick -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
    }

    private void Update()
    {
        if (selectedObjectIndex < 0)
            return;
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        if(lastDetectedPosition != gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

            mouseIndicator.transform.position = mousePosition;
            preview.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
            lastDetectedPosition = gridPosition;
        }
        
    }
}