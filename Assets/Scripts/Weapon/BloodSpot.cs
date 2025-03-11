using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpot : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float fadeDuration = 20f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}

