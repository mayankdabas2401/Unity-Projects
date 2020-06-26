using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundLayer : MonoBehaviour
{
    public GameObject target;
    public GameObject tilePrefab;
    public int amount;
    public float size;
    public float maxDistance;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < amount; i++)
        {
            GameObject tile = Instantiate(tilePrefab, transform);
            tile.transform.position = new Vector3(
                    size * i,
                    0,
                    0
                );
        }
    }

    // Update is called once per frame
    void Update()
    {

        // Search for the right most tile
        // and the left most tile
        GameObject leftmostTile = transform.GetChild(0).gameObject;
        GameObject rightmostTile = transform.GetChild(0).gameObject;

        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject currentTile = transform.GetChild(i).gameObject;
            if(currentTile.transform.position.x > rightmostTile.transform.position.x)
            {
                rightmostTile = currentTile;
            }
            else if(currentTile.transform.position.x < leftmostTile.transform.position.x)
            {
                leftmostTile = currentTile;
            }
        }

        // Calculate the distance between the center layer
        // and the target
        float centerPosition = (leftmostTile.transform.position.x + rightmostTile.transform.position.x) / 2;
        float distance = target.transform.position.x - centerPosition;


        // Make the background look infinite
        if(distance > maxDistance)
        {
            leftmostTile.transform.position = new Vector3(
                    leftmostTile.transform.position.x + amount * size,
                    leftmostTile.transform.position.y,
                    leftmostTile.transform.position.z
                );
        }
        else if(distance < -maxDistance)
        {
            rightmostTile.transform.position = new Vector3(
                    rightmostTile.transform.position.x - amount * size,
                    rightmostTile.transform.position.y,
                    rightmostTile.transform.position.z
                );
        }
    }
}
