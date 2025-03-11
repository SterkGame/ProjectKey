using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTask : MonoBehaviour
{
    TextMeshProUGUI text;
    public static int enemyCount;
    public static int allEnemyCount;
    public EnemyAI[] enemyAIs;

    void Start()
    {

        text = GetComponent<TextMeshProUGUI>();
        enemyAIs = FindObjectsOfType<EnemyAI>();
        enemyCount = 0;
    }

    void Update()
    {
        
        allEnemyCount = enemyAIs.Length;
        text.text = "Enemies killed: " + enemyCount + "/" + allEnemyCount;

        //if (allEnemyCount == enemyCount)
        //{

        //    Debug.Log("Перемога");
        //}
    }

    public void EnemyKilled()
    {
        enemyCount++;
    }
}
