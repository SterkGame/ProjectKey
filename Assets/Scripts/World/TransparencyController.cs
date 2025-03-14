using UnityEngine;
using System.Collections;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // Посилання на гравця
    public LayerMask obstacleLayer; // Шар об'єктів, що можуть закривати гравця
    public float fadeSpeed = 5f; // Швидкість зміни прозорості
    public float transparentAlpha = 0.3f; // Прозорість для затемнення

    private SpriteRenderer lastHitRenderer; // Останній затемнений об'єкт
    private Coroutine fadeCoroutine; // Корутин для плавного затемнення
    private Color originalColor; // Початковий колір об'єкта

    void Update()
    {
        Vector2 direction = (Camera.main.transform.position - player.position).normalized;
        float distance = Vector2.Distance(player.position, Camera.main.transform.position);

        RaycastHit2D hit = Physics2D.Raycast(player.position, direction, distance, obstacleLayer);

        if (hit.collider != null)
        {
            SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();

            if (sr != null && sr != lastHitRenderer)
            {
                // Відновлюємо прозорість попереднього об'єкта
                if (lastHitRenderer != null)
                {
                    if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeTo(lastHitRenderer, originalColor.a));
                }

                // Зберігаємо початковий колір нового об'єкта
                originalColor = sr.color;

                // Затемнюємо новий об'єкт
                lastHitRenderer = sr;
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeTo(sr, transparentAlpha));
            }
        }
        else if (lastHitRenderer != null)
        {
            // Якщо більше нічого не закриває гравця, відновлюємо прозорість
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeTo(lastHitRenderer, originalColor.a));
            lastHitRenderer = null;
        }
    }

    IEnumerator FadeTo(SpriteRenderer sr, float targetAlpha)
    {
        float startAlpha = sr.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < 1f / fadeSpeed)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime * fadeSpeed);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, newAlpha);
            yield return null;
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, targetAlpha);
    }
}