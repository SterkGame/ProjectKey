using UnityEngine;
using System.Collections;

public class TransparencyController : MonoBehaviour
{
    public Transform player; // ��������� �� ������
    public LayerMask obstacleLayer; // ��� ��'����, �� ������ ��������� ������
    public float fadeSpeed = 5f; // �������� ���� ���������
    public float transparentAlpha = 0.3f; // ��������� ��� ����������

    private SpriteRenderer lastHitRenderer; // ������� ���������� ��'���
    private Coroutine fadeCoroutine; // ������� ��� �������� ����������
    private Color originalColor; // ���������� ���� ��'����

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
                // ³��������� ��������� ������������ ��'����
                if (lastHitRenderer != null)
                {
                    if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                    fadeCoroutine = StartCoroutine(FadeTo(lastHitRenderer, originalColor.a));
                }

                // �������� ���������� ���� ������ ��'����
                originalColor = sr.color;

                // ���������� ����� ��'���
                lastHitRenderer = sr;
                if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
                fadeCoroutine = StartCoroutine(FadeTo(sr, transparentAlpha));
            }
        }
        else if (lastHitRenderer != null)
        {
            // ���� ����� ����� �� ������� ������, ���������� ���������
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