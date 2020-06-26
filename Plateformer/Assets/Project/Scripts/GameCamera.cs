using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public Player player;
    public Vector3 offset;
    public float minimumY;
    public float smoothingFactor;

    private bool win;
    void Start()
    {
        player.OnWin += OnWin;
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if(player != null && !player.Dead && !win)
        {
            Vector3 targetPosition = new Vector3(
                offset.x + player.transform.position.x,
                Mathf.Max(offset.y + player.transform.position.y, minimumY),
                offset.z + transform.position.z
            );
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothingFactor * Time.deltaTime);
        }
    }

    void OnWin()
    {
        win = true;
    }
}
