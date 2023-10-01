using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tool", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]
    public ToolType toolType;

    public enum ToolType
    {
        weapon,
        axe
    }

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return this; }
    public override WeaponClass GetWeapon() { return null; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return null; }
}
