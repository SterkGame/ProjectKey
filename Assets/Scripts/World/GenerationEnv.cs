using System.Collections.Generic;
using UnityEngine;

public class GenerationEnv : MonoBehaviour
{
    public int numberObject; // ʳ������ ��'����
    private int generatedObject = 0;
    public float minRange, maxRange; // ����� �������
    public float minDistance = 2.0f; // ̳������� ������� �� ��'������
    public GameObject[] objects; // ����� ��'����
    private List<Vector3> spawnedPositions = new List<Vector3>(); // ������ ������� ������������ ��'����
    private NavMeshUpdater navMeshUpdater;

    private void Start()
    {
        navMeshUpdater = FindObjectOfType<NavMeshUpdater>();
    }
    void Update()
    {
        if (generatedObject < numberObject)
        {
            Generate();
            generatedObject++;
        }
    }

    // ��������� ��'���� � ��������� ������
    public void Generate()
    {
        int rand = Random.Range(0, objects.Length);
        Vector3 newPosition;
        int maxAttempts = 50; // ˳�� ����� ������ ����
        int attempts = 0;

        do
        {
            newPosition = new Vector3(Random.Range(minRange, maxRange), Random.Range(minRange, maxRange), transform.position.z);
            attempts++;
        }
        while (!IsValidPosition(newPosition) && attempts < maxAttempts);

        // ���� ������� ������ ����, ��������� ��'���
        if (attempts < maxAttempts)
        {
            var cell = Instantiate(objects[rand], newPosition, Quaternion.identity);
            spawnedPositions.Add(newPosition);
            navMeshUpdater.UpdateNavMesh(); // ��������� ��� ���
        }
        else
        {
            Debug.LogWarning("�� ������� ������ ���� ��� ������ ��'����.");
        }
    }

    // ��������, �� ���� ������� �� ����� ������� �� �����
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