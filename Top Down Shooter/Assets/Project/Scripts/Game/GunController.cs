using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Transform weaponHolder;
    public Gun startingGun;

    private Gun equippedGun;

    // Start method
    private void Start()
    {
        if (startingGun != null) EquipGun(startingGun);
    }

    // EquipGun
    public void EquipGun(Gun gun)
    {
        if (equippedGun != null) Destroy(equippedGun.gameObject);

        equippedGun = Instantiate(
                gun,
                weaponHolder.position,
                weaponHolder.rotation
            ) as Gun;
        equippedGun.transform.parent = weaponHolder;
    }

    public void Shoot()
    {
        if (equippedGun != null) equippedGun.Shoot();
    }

    public float GunHeight
    {
        get
        {
            return weaponHolder.position.y;
        }
    }
}
