using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Префаб ворога
    public int minEnemies = 5; // Мінімальна кількість ворогів
    public int maxEnemies = 10; // Максимальна кількість ворогів
    public float spawnRadius = 50f; // Радіус карти (100x100, тому радіус = 50)
    public float minDistanceFromCenter = 10f; // Мінімальна відстань від центру

    void Start()
    {
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        // Випадкова кількість ворогів
        int enemyCount = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < enemyCount; i++)
        {
            // Генеруємо випадкову позицію
            Vector2 spawnPosition = GetRandomSpawnPosition();

            // Створюємо ворога на цій позиції
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector2 GetRandomSpawnPosition()
    {
        Vector2 spawnPosition;
        float distanceFromCenter;

        do
        {
            // Генеруємо випадкові координати в межах карти
            float x = Random.Range(-spawnRadius, spawnRadius);
            float y = Random.Range(-spawnRadius, spawnRadius);
            spawnPosition = new Vector2(x, y);

            // Обчислюємо відстань від центру
            distanceFromCenter = Vector2.Distance(Vector2.zero, spawnPosition);

        } while (distanceFromCenter < minDistanceFromCenter); // Повторюємо, якщо позиція занадто близько до центру

        return spawnPosition;
    }
}