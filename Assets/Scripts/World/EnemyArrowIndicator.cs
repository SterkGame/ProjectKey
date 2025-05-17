using UnityEngine;
using UnityEngine.UI;

public class EnemyPointer : MonoBehaviour
{
    public GameObject arrowPrefab; 
    public float arrowOffset = 2f;
    public int minEnemiesToShowArrow = 3;
    public float spriteRotationOffset = -135f;

    private GameObject arrow;
    private Transform player;
    private GameObject[] enemies;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        arrow = Instantiate(arrowPrefab, transform);
        arrow.SetActive(false);
        FindEnemies();
    }

    void Update()
    {

        FindEnemies();

        if (enemies != null && enemies.Length <= minEnemiesToShowArrow && enemies.Length > 0)
        {
            arrow.SetActive(true);

            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                Vector3 direction = (nearestEnemy.transform.position - player.position).normalized;
                arrow.transform.position = player.position + direction * arrowOffset;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + spriteRotationOffset;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle);
            }
        }
        else
        {
            arrow.SetActive(false);
        }
    }

    void FindEnemies()
    {
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
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