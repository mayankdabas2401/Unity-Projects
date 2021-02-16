using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    public Transform weaponHolder;

    private Vector3 originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = weaponHolder.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRecoil()
    {
        weaponHolder.position -= Vector3.forward * 0.2f;
    }

    public void StopRecoil()
    {
        weaponHolder.position = originalPosition;
    }
}
