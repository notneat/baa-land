using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5;
    Rigidbody rb;
    Vector3 directionInputs;
    Vector3 movementDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 directionInputs = new Vector3(horizontalInput, 0, verticalInput).normalized;

        Vector3 movementDirection = (Quaternion.Euler(0, 45, 0) * directionInputs).normalized;

        Vector3 velocity = movementDirection * moveSpeed;
        rb.velocity = new Vector3(velocity.x, 0, velocity.z);
    }

    private void Look()
    {
        Vector3 movementDirection = rb.velocity.normalized;

        if(movementDirection != Vector3.zero)
        {   
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            rb.rotation = Quaternion.Lerp(rb.rotation, targetRotation, 18 * Time.deltaTime);
        }
    }
}
