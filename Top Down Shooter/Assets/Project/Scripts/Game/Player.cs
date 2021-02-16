using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    public CrossHair crossHair;

    private PlayerController playerController;
    private Camera gameCamera;
    private GunController gunController;

    private void Awake()
    {
        gunController = GetComponent<GunController>();
        playerController = GetComponent<PlayerController>();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        gameCamera = Camera.main;
 
    }

    // Update is called once per frame
    void Update()
    {
        // Camera Rotation
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.up * gunController.GunHeight);
        float distance;

        if(ground.Raycast(ray, out distance))
        {
            Vector3 point = ray.GetPoint(distance);
            //Debug.DrawLine(ray.origin, point, Color.red);
            playerController.RotatePlayer(point);
            crossHair.transform.position = point;
            crossHair.TargetDetector(ray);
        }

        // Shoot
        if(Input.GetMouseButton(0))
        {
            gunController.Shoot();
        }
    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }
}
