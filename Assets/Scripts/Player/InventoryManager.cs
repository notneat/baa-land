using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private GameObject slotHolder;

    [SerializeField] private SlotClass[] startingItems;

    private SlotClass[] items;

    private GameObject[] slots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    private bool isMovingItem;

    private void Start()
    {
        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;

        if (isMovingItem)
        {
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if(isMovingItem)
            {
                EndItemMove();
            }
            else
            {
                BeginItemMove();
            }
        }
        else if(Input.GetMouseButtonDown(1))
        {
            if (isMovingItem)
            {
                EndItemMoveSingle();
            }
            else
            {
                BeginItemMoveHalf();
            }
        }
    }

    #region Inventory Utils

    public bool AddItem(ItemClass item, int quantity)
    {
        SlotClass slot = Contains(item);

        if(slot != null && slot.GetItem().isStackable)
        {
            slot.AddQuantity(quantity);
        }
        else
        {
            for(int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i] = new SlotClass(item, quantity);
                    break;
                }
            }
        }
        RefreshUI();
        return true;
    }

    public bool RemoveItem(ItemClass item)
    {

        SlotClass temp = Contains(item);

        if (temp != null)
        {
            if(temp.GetQuantity() > 1)
            {
                temp.SubQuantity(1);
            }
            else
            {
                int slotToRemoveIndex = 0;

                SlotClass slotToRemove = new SlotClass();

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public SlotClass Contains(ItemClass item)
    {
        foreach(SlotClass slot in items)
        {
            if(slot.GetItem() == item) { return slot; }
        }
            return null;
    }

    public void RefreshUI()
    {
        for(int i = 0;i < slots.Length;i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetQuantity() > 1)
                {
                    slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = items[i].GetQuantity().ToString();
                }
                else
                {
                    slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
                }
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TMP_Text>().text = "";
            }
        }
    }

    #endregion Inventory Utils

    #region Move Stuff

    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();

        if(originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
     
    }

    private bool BeginItemMoveHalf()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null)
        {
            return false;
        }

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        
        if(originalSlot.GetQuantity() == 0)
        {
            originalSlot.Clear();
        }

        isMovingItem = true;
        RefreshUI();
        return true;

    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {

            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());

                    RefreshUI();
                    return true;
                }

            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMoveSingle()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            return false;
        }

        if (originalSlot.GetItem() != null && !originalSlot.GetItem().isStackable)
        {
            return false;
        }

        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
        {
            return false;
        }
        else
        {
            if (originalSlot.GetItem() != null && originalSlot.GetItem() == originalSlot.GetItem())
            {
                originalSlot.AddQuantity(1);
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), 1);
            }

            movingSlot.SubQuantity(1);

            if (movingSlot.GetQuantity() < 1)
            {
                isMovingItem = false;
                movingSlot.Clear();
            }
            else
            {
                isMovingItem = true;
            }
            RefreshUI();
            return true;
        }
    }

    private SlotClass GetClosestSlot()
    {
        for(int i = 0; i < slots.Length;i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
            {
                return items[i]; 
            }
        }

        return null;
    }

    #endregion Move Stuff
}
