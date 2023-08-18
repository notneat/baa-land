using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] private Transform playerPos;
    [SerializeField] private float lerpSpeed = 3;

    void FixedUpdate()
    {
        Vector3 pos = playerPos.position;
        Vector3 smoothCamera = Vector3.Lerp(transform.position, pos, lerpSpeed * Time.fixedDeltaTime);
        transform.position = smoothCamera;
    }
}
