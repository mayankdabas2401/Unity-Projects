using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Camera Movement")]
    public GameObject target;
    public float smoothness;
    [Header("Offsets")]
    public Vector3 offset;
    public float minimumY;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = target.GetComponent<Player>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if(target != null && !player.Dead)
        {
            Vector3 targetPosition = new Vector3(
              offset.x + target.transform.position.x,
              Mathf.Max(offset.y + target.transform.position.y, minimumY),
              offset.z + transform.position.z
          );
            transform.position = Vector3.Lerp(
                    transform.position,
                    targetPosition,
                    smoothness * Time.deltaTime
                );
        }
    }
}
