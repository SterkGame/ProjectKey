using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    public bool pauseGame;
    public GameObject pauseGameMenu;
    public GameObject gameOver;
    private bool isOver;
    public TextMeshProUGUI textOver;
    public Image imageOver;

    private void Start()
    {
        isOver = false;
        pauseGameMenu.SetActive(false);
        gameOver.SetActive(false);
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
        textOver.text = "Game Over";
        textOver.color = new Color(1f, 0f, 0f, 1f);
        imageOver.color = new Color(0.62f, 0.14f, 0.14f, 0.58f);
        Time.timeScale = 0f;
        pauseGame = true; 
    }
    public void GameOverWin()
    {
        isOver = true;
        gameOver.SetActive(true);
        textOver.text = "You Win";
        textOver.color = new Color(0f, 1f, 0f, 1f);
        imageOver.color = new Color(0.53f, 1f, 0.5f, 0.58f);
        Time.timeScale = 0f;
        pauseGame = true;
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
