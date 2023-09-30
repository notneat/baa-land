using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layers;

    public GameObject forwardTile;
    public GameObject backTile;
    public GameObject rightTile;
    public GameObject leftTile;
    public GameObject upTile;

    [SerializeField] private GameObject wallForward;
    [SerializeField] private GameObject wallBack;
    [SerializeField] private GameObject wallRight;
    [SerializeField] private GameObject wallLeft;

    private void Start()
    {
        CheckNeighbor();
    }

    private void CheckNeighbor()
    {
        CheckNeighbor(Vector3.forward,forwardTile, wallForward);
        CheckNeighbor(Vector3.back, backTile, wallBack);
        CheckNeighbor(Vector3.right, forwardTile, wallRight);
        CheckNeighbor(Vector3.left, forwardTile, wallLeft);
    }

    private void CheckNeighbor(Vector3 direction, GameObject tile, GameObject wall)
    {
        Vector3 tilePosition = transform.position;
        Ray ray = new Ray(tilePosition, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance, layers))
        {
            //Debug.LogWarning(hit.transform.gameObject + " // " + hit.transform.position + " // " + direction);

            tile = hit.transform.gameObject;
            HideWalls(wall, tile);
        }
    }

    private void HideWalls(GameObject wallObject, GameObject neighborObject)
    {
        if (neighborObject.CompareTag("Water"))
        {
            wallObject.SetActive(false);
        }
        else
        {
            wallObject.SetActive(true);
        }
    }
}
