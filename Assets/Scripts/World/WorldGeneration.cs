using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Tilemap tilemap; // Tilemap ��� ������� �����
    public Tile[] grassTiles;  // ����� ��� ����/�����
    public Tile[] roadTiles;   // ����� ��� ������
    public GameObject[] buildingPrefabs; // ������� �������
    public GameObject[] vegetationPrefabs; // ������� ����������

    public GameObject[] grassPrefabs; // ������ �����
    public int grassDensity = 1000; // ������� �����
    public int mapSize = 100; // ����� ����� (�� -50 �� 50 �� ���� ����)
    public int numberOfBuildings = 4; // ʳ������ �������
    public int vegetationDensity = 100; // ������� ����������
    public int roadThickness = 3; // ������� ������
    public int buildingSpacing = 20; // ³������ �� �������� �� �������
    public int vegetationSpacing = 2; // ³������ �� ����������

    private GameObject environmentParent; // ����������� �ᒺ�� ��� ���������� �� �������

    void Start()
    {
        // ��������� ����������� �ᒺ�� ��� ���������� �� �������
        environmentParent = new GameObject("Environment");
        GenerateMap();

        // �������� �����
        GenerateGrass();
    }

    void GenerateMap()
    {
        // ���������� ��� ����� ������
        FillMapWithGrass();

        // �������� ������
        GenerateRoad();

        // �������� �����
        PlaceBuildings();

        // �������� ����������
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
        bool isHorizontal = Random.Range(0, 2) == 0; // ��������� �������� �������� ������

        if (isHorizontal)
        {
            int roadY = Random.Range(-mapSize / 2, mapSize / 2); // ��������� ������� ������ �� Y
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
            int roadX = Random.Range(-mapSize / 2, mapSize / 2); // ��������� ������� ������ �� X
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
            // �������� ���������� ��� �����
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // ����������, �� ���� �������� ��� �����
            if (IsPositionValidForBuilding(x, y))
            {
                // �������� ���������� ������ �����
                GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
                GameObject building = Instantiate(buildingPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // ������ ������ ������� �ᒺ���� Environment
                building.transform.parent = environmentParent.transform;
            }
            else
            {
                i--; // ���� ���� �� ��������, �������� �� ���
            }
        }
    }

    bool IsPositionValidForBuilding(int x, int y)
    {
        // ����������, �� ���� �� ������� �������
        for (int i = -buildingSpacing; i <= buildingSpacing; i++)
        {
            for (int j = -buildingSpacing; j <= buildingSpacing; j++)
            {
                if (IsRoadTile(x + i, y + j))
                    return false;
            }
        }

        // ����������, �� ���� �� ������� ����� �������
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
            // �������� ���������� ��� ����������
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // ����������, �� ���� �������� ��� ����������
            if (IsPositionValidForVegetation(x, y))
            {
                // �������� ���������� ������ ����������
                GameObject vegetationPrefab = vegetationPrefabs[Random.Range(0, vegetationPrefabs.Length)];
                GameObject vegetation = Instantiate(vegetationPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // ������ ���������� ������� �ᒺ���� Environment
                vegetation.transform.parent = environmentParent.transform;
            }
        }
    }

    bool IsPositionValidForVegetation(int x, int y)
    {
        // ����������, �� ���� �� ������� ������� ��� �������
        if (IsRoadTile(x, y) || IsBuildingAtPosition(x, y))
            return false;

        // ����������, �� ���� �� ������� ����� ����������
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
        // ����������, �� � ������ �� ��� �������
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
            // �������� ���������� ��� �����
            int x = Random.Range(-mapSize / 2, mapSize / 2);
            int y = Random.Range(-mapSize / 2, mapSize / 2);

            // ����������, �� ���� �������� ��� �����
            if (IsPositionValidForGrass(x, y))
            {
                // ��������� �����
                GameObject grassPrefab = grassPrefabs[Random.Range(0, grassPrefabs.Length)];
                GameObject grass = Instantiate(grassPrefab, new Vector3(x, y, 0), Quaternion.identity);

                // ������ ����� ������� �ᒺ���� Environment
                grass.transform.parent = environmentParent.transform;
            }
        }
    }

    bool IsPositionValidForGrass(int x, int y)
    {
        // ����������, �� ���� �� ������� �������
        if (IsRoadTile(x, y))
            return false;

        return true;
    }

    bool IsRoadTile(int x, int y)
    {
        // ����������, �� ���� � �������
        TileBase tile = tilemap.GetTile(new Vector3Int(x, y, 0));
        if (tile != null && System.Array.Exists(roadTiles, roadTile => roadTile == tile))
            return true;

        return false;
    }
}

//bool IsPositionValidForGrass(float x, float y)
//    {
//        // ����������, �� ���� �� ������� ������� ��� �������
//        Vector3Int tilePosition = tilemap.WorldToCell(new Vector3(x, y, 0));
//        if (tilemap.GetTile(tilePosition) == roadTiles[0] || IsBuildingAtPosition(Mathf.RoundToInt(x), Mathf.RoundToInt(y)))
//            return false;

//        return true;
//    }
//}