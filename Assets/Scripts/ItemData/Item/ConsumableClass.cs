using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable item", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    public override ConsumableClass GetConsumable() { return this; }
}
