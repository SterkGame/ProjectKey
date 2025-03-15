using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelChenger : MonoBehaviour
{
    private Animator anim;
    public int levelToLoad;
    public Slider slider;
    public GameObject loadingScreen;



    public Vector3 position; //player
    //public VectorValue playerStorage;

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }

    public void OnLoadingLevel()
    {
        StartCoroutine(LoadingScreenOnFade());
    }

    IEnumerator LoadingScreenOnFade()
    {
        // Завантажуємо сцену у фоновому режимі
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        //operation.allowSceneActivation = false; // Забороняємо активацію, поки рівень не готовий

        loadingScreen.SetActive(true);

        while (operation.progress < 0.9f) // Чекаємо, поки сцена завантажиться
        {
            Debug.Log("Крок 1");
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }

        while (!GenerationEnv.Instance || !GenerationEnv.Instance.IsGenerationComplete)
        {
            Debug.Log("Очікуємо завершення генерації...");
            yield return null;
        }

        Debug.Log("Крок 4");
        operation.allowSceneActivation = true;
        loadingScreen.SetActive(false);
    }


}
