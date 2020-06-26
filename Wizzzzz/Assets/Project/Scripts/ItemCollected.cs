using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollected : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Destroy(gameObject, 1.0f);
    }
}
