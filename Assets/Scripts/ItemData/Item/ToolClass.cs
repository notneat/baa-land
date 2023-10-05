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

    public override ToolClass GetTool() { return this; }
}
