using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAxe : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float swingCooldown;
    [SerializeField] private float swingTime;

    [SerializeField] private bool canSwing = true;
    private Collider swingArea;

    private void Start()
    {
        swingArea = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        canSwing = false;
        StartCoroutine(SwingTime());
        yield return new WaitForSeconds(swingCooldown);
        canSwing = true;
    }

    private IEnumerator SwingTime()
    {
        swingArea.enabled = true;
        yield return new WaitForSeconds(swingTime);
        swingArea.enabled = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<TreeScript>() != null && !canSwing)
        {
            TreeScript tree = collider.GetComponent<TreeScript>();
            tree.Chop(damage);
        }
    }
}
