using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject target;
    public int amount;
    public float size;
    public float maximumPosition;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject tile = GameObject.Instantiate(tilePrefab, transform);
            tile.transform.localPosition = new Vector3(
                    i * size,
                    transform.position.y,
                    transform.position.x
                );
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject leftmostTile = transform.GetChild(0).gameObject;
        GameObject rightmostTile = transform.GetChild(0).gameObject;

        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject currentTile = transform.GetChild(i).gameObject;

            if(currentTile.transform.position.x > rightmostTile.transform.position.x)
            {
                rightmostTile = currentTile;
            }
            else if (currentTile.transform.position.x < leftmostTile.transform.position.x)
            {
                leftmostTile = currentTile;
            }
        }

        float centerPosition = (rightmostTile.transform.position.x + leftmostTile.transform.position.x) / 2;
        float distance = target.transform.position.x - centerPosition;

        if(distance > maximumPosition)
        {
            leftmostTile.transform.position = new Vector3(
                    leftmostTile.transform.position.x + size * amount,
                    leftmostTile.transform.position.y,
                    leftmostTile.transform.position.z
                );
        }
        else if (distance < -maximumPosition)
        {
            rightmostTile.transform.position = new Vector3(
                    rightmostTile.transform.position.x - size * amount,
                    rightmostTile.transform.position.y,
                    rightmostTile.transform.position.z
                );
        }
    }
}
