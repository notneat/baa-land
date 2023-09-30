using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{

    [SerializeField] private Transform playerPos;
    [SerializeField] private float lerpSpeed = 3;

    [SerializeField] private float minCameraSize = 3;
    [SerializeField] private float maxCameraSize = 9;
    [SerializeField] private float zoomSensitivity = 1;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        ChangeCameraSize();
    }

    void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        Vector3 pos = playerPos.position;
        Vector3 smoothCamera = Vector3.Lerp(transform.position, pos, lerpSpeed * Time.fixedDeltaTime);
        transform.position = smoothCamera;
    }

    private void ChangeCameraSize()
    {
        if (_camera != null)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            float newSize = _camera.orthographicSize * Mathf.Pow(2.0f, -scrollInput * zoomSensitivity);

            newSize = Mathf.Clamp(newSize, minCameraSize, maxCameraSize);

            _camera.orthographicSize = newSize;
        }
    }
}
