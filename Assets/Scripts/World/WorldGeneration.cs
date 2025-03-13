using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap для основної карти
    public Tile[] grassTiles;  // Тайли для землі/трави
    public Tile[] roadTiles;   // Тайли для дороги
    public GameObject[] buildingPrefabs; // Префаби будівель
    public GameObject[] vegetationPrefabs; // Префаби рослинності

    public GameObject[] grassPrefabs; // Префаб трави
    public int grassDensity = 1000; // Густота трави
    public int mapSize = 100; // Розмір карти (від -50 до 50 по обох осях)
    public int numberOfBuildings = 4; // Кількість будівель
    public int vegetationDensity = 100; // Густота рослинності
    public int roadThickness = 3; // Товщина дороги
    public int buildingSpacing = 20; // Відстань між будівлями та дорогою
    public int vegetationSpacing = 2; // Відстань між рослинністю

    private GameObject environmentParent; // Батьківський об’єкт для рослинності та будівель

    void Start()
    {
        // Створюємо батьківський об’єкт для рослинності та будівель
        environmentParent = new GameObject("Environment");
        GenerateMap();

        // Генеруємо траву
        GenerateGrass();
    }

    void GenerateMap()
    {
        // Заповнюємо всю карту травою
        FillMapWithGrass();

        // Генеруємо дорогу
        GenerateRoad();

        // Розміщуємо будівлі
        PlaceBuildings();

        // Розміщуємо рослинність
        PlaceVegetation();
    }

    void FillMapWithGrass()
    {
        for (int x = -mapSize / 2; x < mapSize / 2; x++)
        {
            for (int y = -mapSize / 2; y < mapSize / 2; y++)
            {
                Tile randomGrassTile = grassTiles[Random.Range(0, grassTiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), randomGrassTile);
            }
        }
    }

    void GenerateRoad()
    {
        bool isHorizontal = Random.Range(0, 2) == 0; // Випадково вибираємо напрямок дороги

        if (isHorizontal)
        {
            int roadY = Random.Range(-mapSize / 2, mapSize / 2); // Випадкова позиція дороги по Y
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
            int roadX = Random.Range(-mapSize / 2, mapSize / 2); // Випадкова позиція дороги по X
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
            // Випадкові координати для будівлі
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // Перевіряємо, чи місце підходить для будівлі
            if (IsPositionValidForBuilding(x, y))
            {
                // Вибираємо випадковий префаб будівлі
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                GameObject building = Instantiate(buildingPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // Робимо будівлю дочірнім об’єктом Environment
                building.transform.parent = environmentParent.transform;
            }
            else
            {
                i--; // Якщо місце не підходить, спробуємо ще раз
            }
        }
    }

    bool IsPositionValidForBuilding(int x, int y)
    {
        // Перевіряємо, чи місце не зайняте дорогою
        for (int i = -buildingSpacing; i <= buildingSpacing; i++)
        {
            for (int j = -buildingSpacing; j <= buildingSpacing; j++)
            {
                if (IsRoadTile(x + i, y + j))
                    return false;
            }
        }

        // Перевіряємо, чи місце не зайняте іншою будівлею
        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(x, y), buildingSpacing);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Building"))
                return false;
        }

        return true;
    }

    void PlaceVegetation()
    {
        for (int i = 0; i < vegetationDensity; i++)
        {
            // Випадкові координати для рослинності
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // Перевіряємо, чи місце підходить для рослинності
            if (IsPositionValidForVegetation(x, y))
            {
                // Вибираємо випадковий префаб рослинності
                GameObject vegetationPrefab = vegetationPrefabs[Random.Range(0, vegetationPrefabs.Length)];
                GameObject vegetation = Instantiate(vegetationPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // Робимо рослинність дочірнім об’єктом Environment
                vegetation.transform.parent = environmentParent.transform;
            }
        }
    }

    bool IsPositionValidForVegetation(int x, int y)
    {
        // Перевіряємо, чи місце не зайняте дорогою або будівлею
        if (IsRoadTile(x, y) || IsBuildingAtPosition(x, y))
            return false;

        // Перевіряємо, чи місце не зайняте іншою рослинністю
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
        // Перевіряємо, чи є будівля на цій позиції
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
        for (int i = 0; i < grassDensity; i++)
        {
            // Випадкові координати для трави
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // Перевіряємо, чи місце підходить для трави
            if (IsPositionValidForGrass(x, y))
            {
                // Створюємо траву
                GameObject grassPrefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
                GameObject grass = Instantiate(grassPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // Робимо траву дочірнім об’єктом Environment
                grass.transform.parent = environmentParent.transform;
            }
        }
    }

    bool IsPositionValidForGrass(int x, int y)
    {
        // Перевіряємо, чи місце не зайняте дорогою
        if (IsRoadTile(x, y))
            return false;

        return true;
    }

    bool IsRoadTile(int x, int y)
    {
        // Перевіряємо, чи тайл є дорогою
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (tile != null && System.Array.Exists(roadTiles, roadTile => roadTile == tile))
            return true;

        return false;
    }
}

//bool IsPositionValidForGrass(float x, float y)
//    {
//        // Перевіряємо, чи місце не зайняте дорогою або будівлею
//        Vector3Int tilePosition = tilemap.WorldToCell(new Vector3(x, y, 0));
//        if (tilemap.GetTile(tilePosition) == roadTiles[0] || IsBuildingAtPosition(Mathf.RoundToInt(x), Mathf.RoundToInt(y)))
//            return false;

//        return true;
//    }
//}