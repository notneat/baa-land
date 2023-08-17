using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{

    [SerializeField] private Transform playerPos;
    [SerializeField] private float lerpSpeed = 3;

    void FixedUpdate()
    {
        transform.position = playerPos.position;
    }
}
