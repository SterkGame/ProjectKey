using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile[] grassTiles;
    public Tile[] roadTiles;
    public GameObject[] buildingPrefabs;
    public GameObject[] vegetationPrefabs;
    public GameObject[] grassPrefabs;

    // масиви для зимового біому
    public Tile[] winterGrassTiles;
    public GameObject[] winterVegetationPrefabs;
    public GameObject[] winterGrassPrefabs;

    public int grassDensity = 1000;
    public int mapSize = 100;
    public int numberOfBuildings = 4;
    public int vegetationDensity = 100;
    public int roadThickness = 3;
    public int buildingSpacing = 20;
    public int vegetationSpacing = 2;
    public float noiseScale = 20f;
    private float forestThreshold = 0.9f;
    private float clearingThreshold = 0.7f;

    private GameObject environmentParent;

    // випадковий вибір біому
    private bool isWinterBiome;

    void Start()
    {
        isWinterBiome = Random.value < 0.5f;
        environmentParent = new GameObject("Environment");

        GenerateMap();
        GenerateGrass();
    }

    void GenerateMap()
    {
        FillMapWithGrass();
        GenerateRoad();
        PlaceBuildings();
        PlaceVegetation();
    }

    void FillMapWithGrass()
    {
        Tile[] grassSet = isWinterBiome ? winterGrassTiles : grassTiles;

        for (int x = -mapSize / 2; x < mapSize / 2; x++)
        {
            for (int y = -mapSize / 2; y < mapSize / 2; y++)
            {
                Tile randomGrassTile = grassSet[Random.Range(0, grassSet.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), randomGrassTile);
            }
        }
    }

    void GenerateRoad()
    {
        bool isHorizontal = Random.Range(0, 2) == 0;

        if (isHorizontal)
        {
            int roadY = Random.Range(-mapSize / 2, mapSize / 2);
            for (int x = -mapSize / 2; x < mapSize / 2; x++)
            {
                for (int i = 0; i < roadThickness; i++)
                {
                    Tile randomRoadTile = roadTiles[Random.Range(0, roadTiles.Length)];
                    tilemap.SetTile(new Vector3Int(x, roadY + i, 0), randomRoadTile);
                }
            }
        }
        else
        {
            int roadX = Random.Range(-mapSize / 2, mapSize / 2);
            for (int y = -mapSize / 2; y < mapSize / 2; y++)
            {
                for (int i = 0; i < roadThickness; i++)
                {
                    Tile randomRoadTile = roadTiles[Random.Range(0, roadTiles.Length)];
                    tilemap.SetTile(new Vector3Int(roadX + i, y, 0), randomRoadTile);
                }
            }
        }
    }

    void PlaceBuildings()
    {
        for (int i = 0; i < numberOfBuildings; i++)
        {
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            if (IsPositionValidForBuilding(x, y))
            {
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                GameObject building = Instantiate(buildingPrefab, new Vector3(x, y, 0), Quaternion.identity);
                building.transform.parent = environmentParent.transform;
            }
            else
            {
                i--;
            }
        }
    }

    void PlaceVegetation()
    {
        for (int x = -mapSize / 2; x < mapSize / 2; x++)
        {
            for (int y = -mapSize / 2; y < mapSize / 2; y++)
            {
                float xCoord = (float)x / mapSize * noiseScale;
                float yCoord = (float)y / mapSize * noiseScale;
                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);

                if (noiseValue > forestThreshold)
                {
                    if (Random.Range(0, 2) == 0)
                        PlaceTree(x, y);
                }
                else if (noiseValue < clearingThreshold)
                {
                    if (Random.Range(0, 10) == 0)
                        PlaceTree(x, y);
                }
                else
                {
                    if (Random.Range(0, 5) == 0)
                        PlaceTree(x, y);
                }
            }
        }
    }

    void PlaceTree(int x, int y)
    {
        if (IsPositionValidForVegetation(x, y))
        {
            GameObject[] vegetationSet = isWinterBiome ? winterVegetationPrefabs : vegetationPrefabs;
            GameObject treePrefab = vegetationSet[Random.Range(0, vegetationSet.Length)];
            GameObject tree = Instantiate(treePrefab, new Vector3(x, y, 0), Quaternion.identity);
            tree.transform.parent = environmentParent.transform;
        }
    }

    bool IsPositionValidForBuilding(int x, int y)
    {
        for (int i = -buildingSpacing; i <= buildingSpacing; i++)
        {
            for (int j = -buildingSpacing; j <= buildingSpacing; j++)
            {
                if (IsRoadTile(x + i, y + j))
                    return false;
            }
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), buildingSpacing);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Building"))
                return false;
        }

        return true;
    }

    bool IsPositionValidForVegetation(int x, int y)
    {
        if (IsRoadTile(x, y) || IsBuildingAtPosition(x, y))
            return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), vegetationSpacing);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Vegetation"))
                return false;
        }

        return true;
    }

    bool IsBuildingAtPosition(int x, int y)
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(x, y));
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Building"))
                return true;
        }
        return false;
    }

    void GenerateGrass()
    {
        GameObject[] grassSet = isWinterBiome ? winterGrassPrefabs : grassPrefabs;

        for (int i = 0; i < grassDensity; i++)
        {
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            if (IsPositionValidForGrass(x, y))
            {
                GameObject grassPrefab = grassSet[Random.Range(0, grassSet.Length)];
                GameObject grass = Instantiate(grassPrefab, new Vector3(x, y, 0), Quaternion.identity);
                grass.transform.parent = environmentParent.transform;
            }
        }
    }

    bool IsPositionValidForGrass(int x, int y)
    {
        return !IsRoadTile(x, y);
    }

    bool IsRoadTile(int x, int y)
    {
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (tile != null && System.Array.Exists(roadTiles, roadTile => roadTile == tile))
            return true;

        return false;
    }
}
