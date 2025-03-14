using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // ������ ������
    public int minEnemies = 5; // ̳������� ������� ������
    public int maxEnemies = 10; // ����������� ������� ������
    public float spawnRadius = 50f; // ����� ����� (100x100, ���� ����� = 50)
    public float minDistanceFromCenter = 10f; // ̳������� ������� �� ������

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        // ��������� ������� ������
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            // �������� ��������� �������
            Vector2 spawnPosition = GetRandomSpawnPosition();

            // ��������� ������ �� ��� �������
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        Vector2 spawnPosition;
        float distanceFromCenter;

        do
        {
            // �������� �������� ���������� � ����� �����
            float x = Random.Range(-spawnRadius, spawnRadius);
            float y = Random.Range(-spawnRadius, spawnRadius);
            spawnPosition = new Vector2(x, y);

            // ���������� ������� �� ������
            distanceFromCenter = Vector2.Distance(Vector2.zero, spawnPosition);

        } while (distanceFromCenter < minDistanceFromCenter); // ����������, ���� ������� ������� ������� �� ������

        return spawnPosition;
    }
}