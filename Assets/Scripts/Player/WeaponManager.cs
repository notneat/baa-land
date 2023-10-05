using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Mesh mesh;
    [SerializeField] private FiringMode firingMode;
    [SerializeField] private int damage;
    [SerializeField] private int ammo;
    [SerializeField] private int storedAmmo;
    [SerializeField] private float fireRate;
    [SerializeField] private float critChance;

    private enum FiringMode
    {
        Single,
        Automatic
    }

    private void Update()
    {
        if(inventory.selectedItem is WeaponClass)
        {
            WeaponClass selectedWeapon = inventory.selectedItem.GetWeapon();

            mesh = selectedWeapon.weaponMesh;
            firingMode = (FiringMode)selectedWeapon.firingMode;
            damage = selectedWeapon.damage;
            ammo = selectedWeapon.clipSize;
            fireRate = selectedWeapon.fireRate;
            critChance = selectedWeapon.critChance;
        }
    }
}
