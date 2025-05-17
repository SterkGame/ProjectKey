using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTask : MonoBehaviour
{
    TextMeshProUGUI text;
    public int enemyCount;
    public static int allEnemyCount;
    public EnemyAI[] enemyAIs;
    public PauseMenu pauseMenu;

    void Start()
    {

        text = GetComponent<TextMeshProUGUI>();
        //enemyAIs = FindObjectsOfType<EnemyAI>();
        enemyCount = 0;
    }

    void Update()
    {
        enemyAIs = FindObjectsOfType<EnemyAI>();
        allEnemyCount = enemyAIs.Length;
        text.text = "Ворогів вбито: " + enemyCount + "/" + allEnemyCount;

        if (allEnemyCount == enemyCount && allEnemyCount != 0 && pauseMenu.isOver == false)
        {
            //Debug.Log("Перемога");
            pauseMenu.GameOverWin();
        }
    }

    public void EnemyKilled()
    {
        enemyCount++;
    }
}
