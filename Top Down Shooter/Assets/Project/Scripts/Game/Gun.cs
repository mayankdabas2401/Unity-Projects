using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public Transform shell;
    public Transform shellSpawnOrigin;
    public Projectile projectile;
    public float bulletSpeed = 35f;
    public float fireRate = 100f;
    public AudioClip shootAudio;

    private float nextShootTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Recoil Animation
        
    }

    public void Shoot()
    {
        if(Time.time > nextShootTime)
        {
            nextShootTime = Time.time + fireRate / 1000;

            Projectile newProjectile = Instantiate(projectile, muzzle.position, muzzle.rotation) as Projectile;
            //newProjectile.transform.position = muzzle.position;
            newProjectile.Speed = bulletSpeed;

            Instantiate(shell, shellSpawnOrigin.position, shellSpawnOrigin.rotation);
            AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }
}
