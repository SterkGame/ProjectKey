using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowIndicator : MonoBehaviour
{
    public Transform arrowIndicator; // ��'��� ������, ��� ������ �������� �� ������
    private List<Transform> enemies = new List<Transform>(); // ������ ������ �� ����
    private Transform targetEnemy; // ���������� �����, �� ����� ����� ������
    private float searchInterval = 1.0f; // ��� �� ���������� ������ ������
    private float nextSearchTime = 0f; // ˳������� ��� ��������� ������ ������


    
    void Update()
    {
        if (Time.time >= nextSearchTime)
        {
            FindEnemies(); // ��������� ������ ������ ����� �������
            nextSearchTime = Time.time + searchInterval;
        }

        if (enemies.Count > 0 && enemies.Count <= 10) // ���� ������ 3 ��� ����� � �������� ������
        {
            SelectClosestEnemy();
            if (targetEnemy != null)
            {
                arrowIndicator.gameObject.SetActive(true);
                PointToEnemy();
            }
        }
    }

    // ����������� ��������� ��� ������ � ��
    void FindEnemies()
    {
        enemies.Clear(); // ������� ������ ������
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // ������ ������ �� ����� "Enemy"

        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy.transform); // ������ �� � ������
        }
    }

    // ������ ����������� ������ �� ������
    void SelectClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue; // ���������� �������� ������

            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        targetEnemy = closestEnemy; // ��������� ���� ��� ������
    }

    // ������� ������ � �� ����������� ������
    void PointToEnemy()
    {
        if (targetEnemy == null) return;
        Vector3 direction = (targetEnemy.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowIndicator.rotation = Quaternion.Euler(0, 0, angle);
    }
}
