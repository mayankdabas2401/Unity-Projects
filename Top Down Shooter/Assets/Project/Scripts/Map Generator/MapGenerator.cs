using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Map[] maps;
    public int mapIndex;

    public Transform tilePrefab;
    public Transform navmeshFloor;
    public Vector2 maxMapSize;
    [Range(0, 1)]
    public float outlinePercent;
    public float tileSize;

    private Map currentMap;

    private Transform[,] tileMap;
    private List<Coordinates> allOpenTiles;
    private Queue<Coordinates> shuffleOpenTiles;
    // Start is called before the first frame update
    void Awake()
    {
        if(FindObjectOfType<Spawner>() != null)
            FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
        //GenerateMap();
    }

    void OnNewWave(int waveIndex)
    {
        mapIndex = waveIndex - 1;
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps[mapIndex];

        // Creating Map holder
        string holderName = "Generated Map";
        if (transform.Find(holderName)) DestroyImmediate(transform.Find(holderName).gameObject);

        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        // Box Collioder
        GetComponent<BoxCollider>().size = new Vector3(
                currentMap.mapSize.x * tileSize,
                0.05f,
                currentMap.mapSize.y * tileSize
            );

        // Random coordinates
        tileMap = new Transform[(int)currentMap.mapSize.x, (int)currentMap.mapSize.y];
        allOpenTiles = new List<Coordinates>();

        for(int x = 0; x < currentMap.mapSize.x; x++)
        {
            for(int y = 0; y < currentMap.mapSize.y; y++)
            {
                allOpenTiles.Add(new Coordinates(x, y));
            }
        }

        // Spawning tiles
        for(int x = 0; x < currentMap.mapSize.x; x++)
        {
            for(int y = 0; y < currentMap.mapSize.y; y++)
            {
                Vector3 tilePosition = CoordinateToPosition(x, y);
                Transform newTile = Instantiate(
                        tilePrefab,
                        tilePosition,
                        Quaternion.Euler(Vector3.right * 90)
                    ) as Transform;
                newTile.localScale = Vector3.one * (1 - outlinePercent) * tileSize;
                newTile.parent = mapHolder;
                tileMap[x,y] = newTile;
            }
        }

        // Removing tile that are not to be used
        for(int x = -1; x <= 1; x++)
        {
            for(int y = -1; y <= 1; y++)
            {
                int x_coordinate = currentMap.mapCenter.x + x;
                int y_coordinate = currentMap.mapCenter.y + y;

                allOpenTiles.Remove(new Coordinates(x_coordinate, y_coordinate));
            }
        }

        shuffleOpenTiles = new Queue<Coordinates>(Utility.ShuffleArray(allOpenTiles.ToArray(), currentMap.seed));

        // Creating navmesh floor
        navmeshFloor.localScale = new Vector3(
                maxMapSize.x,
                maxMapSize.y
            ) * tileSize;
    }

    Vector3 CoordinateToPosition(int x, int y)
    {
        return new Vector3(
                        -currentMap.mapSize.x / 2f + 0.5f + x,
                        0f,
                        -currentMap.mapSize.y / 2f + 0.5f + y
                    ) * tileSize;
    }

    public Transform GetRandomOpenTiles()
    {
        Coordinates randomCoordinates = shuffleOpenTiles.Dequeue();
        shuffleOpenTiles.Enqueue(randomCoordinates);

        return tileMap[randomCoordinates.x, randomCoordinates.y];
    }

    public Transform GetTilesFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize + (currentMap.mapSize.x - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (currentMap.mapSize.y - 1) / 2f);
        x = Mathf.Clamp(x, 0, tileMap.GetLength(0));
        y = Mathf.Clamp(y, 0, tileMap.GetLength(1));
        return tileMap[x, y];
    }

    [System.Serializable]
    public struct Coordinates
    {
        public int x;
        public int y;

        public Coordinates(int _x, int _y)
        {
            x = _x;
            y = _y;
        }
    }

    [System.Serializable]
    public class Map
    {
        public Coordinates mapSize;
        public int seed;

        public Coordinates mapCenter
        {
            get
            {
                return new Coordinates((int)mapSize.x / 2, (int)mapSize.y / 2);
            }
        }
    }
}
