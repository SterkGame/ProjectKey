using System.Collections.Generic;
using UnityEngine;

public class GenerationEnv : MonoBehaviour
{
    public int numberObject; // Кількість об'єктів
    private int generatedObject = 0;
    public float minRange, maxRange; // Розмір території
    public float minDistance = 2.0f; // Мінімальна відстань між об'єктами
    public GameObject[] objects; // Масив об'єктів
    private List<Vector3> spawnedPositions = new List<Vector3>(); // Список позицій згенерованих об'єктів
    private NavMeshUpdater navMeshUpdater;

    public static GenerationEnv Instance;
    public bool IsGenerationComplete { get; private set; } = false;

    private void Awake()
    {
        Debug.Log("GenerationEnv Awake() викликано!");
        Instance = this;
    }

    private void Start()
    {
        navMeshUpdater = FindObjectOfType<NavMeshUpdater>();
        navMeshUpdater.UpdateNavMesh();
        
    }
    void Update()
    {
        if (generatedObject < numberObject)
        {
            Generate();
            generatedObject++;
            Debug.Log("Генерація працює");
        }
        else if (!IsGenerationComplete)
        {
            IsGenerationComplete = true;
            Debug.Log("Генерація завершена");
        }

    }

    // Генерація об'єктів з перевіркою відстані
    public void Generate()
    {
        int rand = Random.Range(0, objects.Length);
        Vector3 newPosition;
        int maxAttempts = 50; // Ліміт спроб пошуку місця
        int attempts = 0;

        do
        {
            newPosition = new Vector3(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange), transform.position.z);
            attempts++;
        }
        while (!IsValidPosition(newPosition) && attempts < maxAttempts);

        // Якщо вдалося знайти місце, створюємо об'єкт
        if (attempts < maxAttempts)
        {
            var cell = Instantiate(objects[rand], newPosition, Quaternion.identity);
            spawnedPositions.Add(newPosition);
            navMeshUpdater.UpdateNavMesh(); // Оновлюємо нав меш
            IsGenerationComplete = true;

        }
        else
        {
            Debug.LogWarning("Не вдалося знайти місце для нового об'єкта.");
            IsGenerationComplete = true;
        }
    }

    // Перевірка, чи нова позиція не надто близько до інших
    private bool IsValidPosition(Vector3 position)
    {
        foreach (Vector3 existingPosition in spawnedPositions)
        {
            if (Vector3.Distance(position, existingPosition) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
}