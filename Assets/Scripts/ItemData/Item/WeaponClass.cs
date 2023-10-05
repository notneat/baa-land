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
        Shotgun,
        Rifle
    }

    public enum FiringMode
    {
        Single,
        Automatic
    }

    public FiringMode GetFiringMode()
    {
        return firingMode;
    }

    public override WeaponClass GetWeapon() { return this; }
}
