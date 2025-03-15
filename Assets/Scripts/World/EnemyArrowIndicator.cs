using UnityEngine;
using UnityEngine.UI;

public class EnemyPointer : MonoBehaviour
{
    public GameObject arrowPrefab; // Префаб стрілки
    public float arrowOffset = 2f; // Відстань стрілки від гравця
    public int minEnemiesToShowArrow = 3; // Мінімальна кількість ворогів для показу стрілки
    public float spriteRotationOffset = -135f; // Додатковий поворот спрайту (-135 градусів)

    private GameObject arrow; // Екземпляр стрілки
    private Transform player; // Трансформ гравця
    private GameObject[] enemies; // Масив ворогів

    void Start()
    {
        // Знаходимо гравця за тегом
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Створюємо стрілку
        arrow = Instantiate(arrowPrefab, transform);
        arrow.SetActive(false); // Спочатку стрілка прихована

        // Починаємо пошук ворогів
        FindEnemies();
    }

    void Update()
    {
        // Оновлюємо список ворогів
        FindEnemies();

        // Якщо ворогів менше, ніж minEnemiesToShowArrow, показуємо стрілку
        if (enemies != null && enemies.Length < minEnemiesToShowArrow && enemies.Length > 0)
        {
            arrow.SetActive(true);

            // Знаходимо найближчого ворога
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                // Напрямок до ворога
                Vector3 direction = (nearestEnemy.transform.position - player.position).normalized;

                // Позиція стрілки
                arrow.transform.position = player.position + direction * arrowOffset;

                // Поворот стрілки в бік ворога з урахуванням додаткового повороту спрайту
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + spriteRotationOffset;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            // Приховуємо стрілку, якщо ворогів достатньо
            arrow.SetActive(false);
        }
    }

    void FindEnemies()
    {
        // Шукаємо всі об'єкти з тегом "Enemy"
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        // Відфільтровуємо мертвих ворогів
        enemies = System.Array.FindAll(allEnemies, enemy =>
        {
            EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();
            return enemyAI != null && enemyAI._currentState != EnemyAI.State.Death;
        });
    }

    GameObject FindNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        // Проходимо по всіх ворогах і знаходимо найближчого
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}