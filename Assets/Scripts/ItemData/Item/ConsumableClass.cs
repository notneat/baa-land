using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable item", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override WeaponClass GetWeapon() { return null; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return this; }
}
