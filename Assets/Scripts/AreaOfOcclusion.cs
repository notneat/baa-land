using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaOfOcclusion : MonoBehaviour
{
    public List<GameObject> objectsToCull = new List<GameObject>();

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("House"))
        {
            objectsToCull.Add(collision.gameObject);
            Debug.Log("Added " + collision.gameObject + " to objectsToCull list");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        DeCullObjects();
        if (collision.gameObject.CompareTag("Tree") || collision.gameObject.CompareTag("House"))
        {   
            objectsToCull.Remove(collision.gameObject);
            Debug.Log("Removed " + collision.gameObject + " from objectsToCull list");
        }
    }

    private void FixedUpdate()
    {
        CullObjects();
    }

    private void CullObjects()
    {
        Debug.Log("Culling Started");
        foreach (GameObject obj in objectsToCull)
        {
            Renderer[] objectRenderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer objectRenderer in objectRenderers)
            {
                objectRenderer.enabled = false;
            }
        }
    }

    private void DeCullObjects()
    {
        Debug.Log("DeCulling Started");
        foreach (GameObject obj in objectsToCull)
        {
            Renderer[] objectRenderers = obj.GetComponentsInChildren<Renderer>();

            foreach (Renderer objectRenderer in objectRenderers)
            {
                objectRenderer.enabled = true;
            }
        }
    }
}
