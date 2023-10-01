using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Item/Weapon")]
public class WeaponClass : ItemClass
{
    [Header("Weapon")]
    public WeaponType weaponType;
    public FiringMode firingMode;
    public Mesh weaponMesh;
    public int damage;
    public int clipSize;
    public float fireRate;
    public float critChance;

    public enum WeaponType
    {
        shotgun,
        rifle
    }

    public enum FiringMode
    {
        single,
        automatic
    }

    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override WeaponClass GetWeapon() { return this; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return null; }
}
