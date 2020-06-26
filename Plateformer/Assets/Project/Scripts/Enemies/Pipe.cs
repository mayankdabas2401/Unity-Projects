using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    public GameObject enemy;
    public float visibleHeight;
    public float raisingDuration;
    public float raisingSpeed;

    private float raisingTimer;
    private bool isHidden;
    // Start is called before the first frame update
    void Start()
    {
        raisingTimer = raisingDuration;
        isHidden = true;
    }

    // Update is called once per frame
    void Update()
    {
        raisingTimer -= Time.deltaTime;

        if(raisingTimer <= 0.0f)
        {
            raisingTimer = raisingDuration;
            isHidden = !isHidden;
        }

        Vector3 targetPosition = new Vector3(
                enemy.transform.localPosition.x,
                isHidden ? 0.0f : visibleHeight,
                enemy.transform.localPosition.z
            );
        enemy.transform.localPosition = Vector3.Lerp(
                enemy.transform.localPosition, 
                targetPosition,
                raisingSpeed * Time.deltaTime
            );
    }
}
