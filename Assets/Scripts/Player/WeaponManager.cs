using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Weapon properties")]
    private InventoryManager inventory;
    [SerializeField] private Mesh mesh;
    [SerializeField] private FiringMode firingMode;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private int damage;
    [SerializeField] private int magSize;
    [SerializeField] private int maxAmmoInMag;
    [SerializeField] private int pellets;
    [SerializeField] private float spread;
    [SerializeField] private float fireRate;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float critChance;
    [SerializeField] private float reloadTime;

    [Header("System values")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject weaponHolder;
    [SerializeField] private Transform shootingPosition;
    [SerializeField] private int remainingBullets;
    [SerializeField] private int storedAmmo;
    [SerializeField] private int bulletsShot;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private LineRenderer lineRenderer;

    private bool canShoot = true;
    private bool shooting = false;
    private bool reloading = false;
    private bool weaponEquipped = false;

    private enum WeaponType
    {
        Rifle,
        Shotgun
    }

    private enum FiringMode
    {
        Single,
        Automatic,
        Burst
    }

    private void Start()
    {
        inventory = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        RefreshWeaponData();
        ListenInputs();
    }

    private void RefreshWeaponData()
    {
        if(inventory.selectedItem is WeaponClass)
        {
            weaponEquipped = true;
            WeaponClass selectedWeapon = inventory.selectedItem.GetWeapon();

            mesh = selectedWeapon.weaponMesh;
            weaponHolder.GetComponent<MeshFilter>().mesh = mesh;
            firingMode = (FiringMode)selectedWeapon.firingMode;
            weaponType = (WeaponType)selectedWeapon.weaponType;
            damage = selectedWeapon.damage;
            magSize = selectedWeapon.magSize;
            maxAmmoInMag = selectedWeapon.maxAmmoInMag;
            pellets = selectedWeapon.pellets;
            spread = selectedWeapon.spread;
            fireRate = selectedWeapon.fireRate;
            timeBetweenShots = selectedWeapon.timeBetweenShots;
            critChance = selectedWeapon.critChance;
            reloadTime = selectedWeapon.reloadTime;
        }
        else
        {
            weaponEquipped = false;
        }
    }

    private void ListenInputs()
    {
        if (weaponEquipped)
        {
            if (firingMode == FiringMode.Single)
            {
                shooting = Input.GetMouseButtonDown(0);

                if (shooting == true && remainingBullets > 0 && !reloading && canShoot)
                {
                    bulletsShot = pellets;
                    Shoot();
                }
            }
            else if (firingMode == FiringMode.Automatic)
            {
                shooting = Input.GetMouseButton(0);

                if (shooting == true && remainingBullets > 0 && !reloading && canShoot)
                {
                    Shoot();
                }
            }
            
            if(remainingBullets < maxAmmoInMag && !shooting)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Reload();
                }
            }

            if(remainingBullets <= 0)
            {
                canShoot = false;
            }
            else
            {
                canShoot = true;
            }
        }
        else
        {
            if(reloading)
            {
                reloading = false;
            }
        }
    }

    private void Shoot()
    {
        for(int i = 0; i < Mathf.Max(1, pellets); i++)
        {
            Ray ray = new Ray(shootingPosition.position, shootingPosition.forward);
            RaycastHit hit;

            Vector3 targetPoint;
            if(Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;

                //lineRenderer.SetPosition(0, shootingPosition.position);
                //lineRenderer.SetPosition(1, hit.point);
                //lineRenderer.enabled = true;
            }
            else
            {
                targetPoint = ray.GetPoint(128);
                //lineRenderer.enabled = false;
            }

            Vector3 directionNoSpread = targetPoint - shootingPosition.position;

            float xSpread = Random.Range(-spread, spread);
            float ySpread = Random.Range(-spread, spread);

            Vector3 directionWithSpread = directionNoSpread + new Vector3(xSpread, ySpread, 0);

            GameObject bullet = Instantiate(bulletPrefab, shootingPosition.position, Quaternion.identity);
            bullet.transform.forward = directionWithSpread.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * bulletSpeed, ForceMode.Impulse);
        }
        canShoot = false;
        remainingBullets--;
        Invoke("ResetShot", fireRate);
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        if (reloading)
        {
            remainingBullets = maxAmmoInMag;
            storedAmmo -= maxAmmoInMag;
            Debug.Log("Reloading");
            reloading = false;
        }
    }

    private void ResetShot()
    {
        canShoot = true;
    }
}
