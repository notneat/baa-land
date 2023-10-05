using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Misc item", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    public override MiscClass GetMisc() { return this; }
}
