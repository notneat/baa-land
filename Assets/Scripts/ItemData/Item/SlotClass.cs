using System.Collections;
using UnityEngine;
using System;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    public SlotClass(ItemClass slotItem, int slotQuantity)
    {
        item = slotItem;
        quantity = slotQuantity;
    }

    public SlotClass(SlotClass slot)
    {
        this.item = slot.GetItem();
        this.quantity = slot.GetQuantity();
    }

    public SlotClass()
    {
        item = null;
        quantity = 0;
    }

    public void Clear()
    {
        this.item = null;
        this.quantity = 0;
    }

    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void AddQuantity(int slotQuantity) { quantity += slotQuantity; }
    public void SubQuantity(int slotQuantity) 
    {
        quantity -= slotQuantity;
        if(quantity <= 0)
        {
            Clear();
        }
    }
    public void AddItem(ItemClass item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
