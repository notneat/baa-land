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
    public int magSize;
    public int maxAmmoInMag;
    public int pellets;
    public float spread;
    public float fireRate;
    public float timeBetweenShots;
    public float critChance;
    public float reloadTime;

    public enum WeaponType
    {
        Rifle,
        Shotgun
    }

    public enum FiringMode
    {
        Single,
        Automatic,
        Burst
    }

    public override WeaponClass GetWeapon() { return this; }
}
