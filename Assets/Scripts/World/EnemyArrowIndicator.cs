using System.Collections.Generic;
using UnityEngine;

public class EnemyArrowIndicator : MonoBehaviour
{
    public Transform arrowIndicator; // Об'єкт стрілки, яка показує напрямок на ворога
    private List<Transform> enemies = new List<Transform>(); // Список ворогів на сцені
    private Transform targetEnemy; // Найближчий ворог, на якого вказує стрілка
    private float searchInterval = 1.0f; // Час між оновленням списку ворогів
    private float nextSearchTime = 0f; // Лічильник для оновлення списку ворогів


    
    void Update()
    {
        if (Time.time >= nextSearchTime)
        {
            FindEnemies(); // Оновлюємо список ворогів кожну секунду
            nextSearchTime = Time.time + searchInterval;
        }

        if (enemies.Count > 0 && enemies.Count <= 10) // Якщо ворогів 3 або менше – показуємо стрілку
        {
            SelectClosestEnemy();
            if (targetEnemy != null)
            {
                arrowIndicator.gameObject.SetActive(true);
                PointToEnemy();
            }
        }
    }

    // Автоматично знаходить всіх ворогів у грі
    void FindEnemies()
    {
        enemies.Clear(); // Очищаємо старий список
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy"); // Шукаємо ворогів за тегом "Enemy"

        foreach (GameObject enemy in enemyObjects)
        {
            enemies.Add(enemy.transform); // Додаємо їх у список
        }
    }

    // Вибирає найближчого ворога до гравця
    void SelectClosestEnemy()
    {
        float minDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Transform enemy in enemies)
        {
            if (enemy == null) continue; // Пропускаємо знищених ворогів

            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        targetEnemy = closestEnemy; // Оновлюємо ціль для стрілки
    }

    // Повертає стрілку у бік найближчого ворога
    void PointToEnemy()
    {
        if (targetEnemy == null) return;
        Vector3 direction = (targetEnemy.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrowIndicator.rotation = Quaternion.Euler(0, 0, angle);
    }
}
