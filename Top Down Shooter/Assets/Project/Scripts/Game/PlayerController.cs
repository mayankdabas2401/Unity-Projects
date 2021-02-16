using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public void RotatePlayer(Vector3 point)
    {
        Vector3 lookPoint = new Vector3(
                point.x,
                transform.position.y,
                point.z
            );
        transform.LookAt(lookPoint);
    }
}
