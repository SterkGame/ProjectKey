using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelChenger : MonoBehaviour
{
    private Animator anim;
    public int levelToLoad;

    public Vector3 position; //player
    public VectorValue playerStorage;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    //затемнення при переході рівня
    public void FadeToLevel()
    {
        anim.SetTrigger("fade");
    }

    //Переключення сцени і позиції гравця
    public void OnFadeComplete()
    {
        playerStorage.inatialValue = position;
        SceneManager.LoadScene(levelToLoad);
    }
}
