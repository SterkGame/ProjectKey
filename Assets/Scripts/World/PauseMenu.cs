using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public LevelTask levelTask;
    public bool pauseGame;
    public GameObject pauseGameMenu;
    public GameObject gameOver;
    public bool isOver;
    public TextMeshProUGUI textOver;
    public Image imageOver;

    private void Start()
    {
        isOver = false;
        pauseGameMenu.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1f;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isOver == false)
        {
            if (pauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        pauseGame = false;

        GameInput.Instance.EnableMovement();
    }

    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;

        GameInput.Instance.DisableMovement();
    }

    public void GameOverDeath()
    {
        isOver = true;
        gameOver.SetActive(true);
        textOver.text = "Поразка";
        textOver.color = new Color(1f, 0f, 0f, 1f);
        imageOver.color = new Color(0.62f, 0.14f, 0.14f, 0.58f);
        Time.timeScale = 0f;
        pauseGame = true;

        // Нарахування винагороди тільки за вбитих ворогів
        int reward = levelTask.enemyCount * 20;

        PlayerData data = SaveSystem.LoadPlayer();
        data.coins += reward;
        data.totalKills += levelTask.enemyCount;
        data.totalDeaths += 1;
        Debug.Log("Монети: " + reward);
        SaveSystem.SavePlayer(data);
    }

    public void GameOverWin()
    {
        isOver = true;
        gameOver.SetActive(true);
        textOver.text = "Перемога";
        textOver.color = new Color(0f, 1f, 0f, 1f);
        imageOver.color = new Color(0.53f, 1f, 0.5f, 0.58f);
        Time.timeScale = 0f;
        pauseGame = true;

        // Винагорода за ворогів + бонус
        int reward = levelTask.enemyCount * 20;
        Debug.Log("Монети: " + reward);
        reward = reward + 100;
        Debug.Log("Монети: " + reward);
        PlayerData data = SaveSystem.LoadPlayer();
        data.coins += reward;
        data.totalKills += levelTask.enemyCount;
        data.totalWins += 1;
        SaveSystem.SavePlayer(data);
    }


    public void RestartScene()
    {
        Time.timeScale = 1f;
        isOver = false;
        pauseGame = false;
        gameOver.SetActive(false);

        GameInput.Instance.EnableMovement();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {


        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
