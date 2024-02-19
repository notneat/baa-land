using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float despawnTime;

    private void Start()
    {
        StartCoroutine(DespawnBullet());        
    }

    private IEnumerator DespawnBullet()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
