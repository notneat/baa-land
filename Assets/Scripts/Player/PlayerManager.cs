using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private PlayerMovement movement;

    private ItemClass selectedItem;

    private void Update()
    {
        selectedItem = inventory.selectedItem;
        Debug.Log(selectedItem);
    }
}
