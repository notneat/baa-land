using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layers;

    public GameObject forwardTile { get; private set; }
    public GameObject backTile { get; private set; }
    public GameObject rightTile { get; private set; }
    public GameObject leftTile { get; private set; }

    public void StartNeighborCheck()
    {
        Vector3 tilePosition = transform.position;

        DetectNeighbor(tilePosition, Vector3.forward, layers, "forward");
        DetectNeighbor(tilePosition, Vector3.back, layers, "back");
        DetectNeighbor(tilePosition, Vector3.right, layers, "right");
        DetectNeighbor(tilePosition, Vector3.left, layers, "left");
    }

    private void DetectNeighbor(Vector3 origin, Vector3 direction, LayerMask layer , string id)
    {
        Ray ray = new Ray(origin, direction);

        if(Physics.Raycast(ray, out RaycastHit hit, rayDistance, layer))
        {
            Debug.LogWarning(hit.transform.gameObject + " // " + hit.transform.position + " // " + id);
            
            if(direction == Vector3.forward)
            {
                this.forwardTile = hit.transform.gameObject;
            }
            else if(direction == Vector3.back)
            {
                this.backTile = hit.transform.gameObject;
            }
            else if (direction == Vector3.right)
            {
                this.rightTile = hit.transform.gameObject;
            }
            else if (direction == Vector3.left)
            {
                this.leftTile = hit.transform.gameObject;
            }
        }
    }
}
