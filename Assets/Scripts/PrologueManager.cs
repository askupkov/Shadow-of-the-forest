using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrologueManager : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI descriptionText;
    public Sprite[] sprites;

    private int currentSpriteIndex = 0;
    private float fadeDuration = 1f;

    private void Start()
    {
        StartCoroutine(ShowPrologue());
    }

    private IEnumerator ShowPrologue()
    {
        foreach (var sprite in sprites)
        {
            yield return StartCoroutine(FadeInSprite(sprite));

            descriptionText.text = $"Описание кадра {currentSpriteIndex + 1}";

            yield return new WaitForSeconds(3f);

            currentSpriteIndex++;
            if (currentSpriteIndex < sprites.Length)
            {
                yield return StartCoroutine(FadeOutSprite());
            }
        }
        Debug.Log("Пролог завершен");
    }

    private IEnumerator FadeInSprite(Sprite sprite)
    {
        image.sprite = sprite;
        image.canvasRenderer.SetAlpha(0f);

        while (image.canvasRenderer.GetAlpha() < 1f)
        {
            image.canvasRenderer.SetAlpha(Mathf.MoveTowards(image.canvasRenderer.GetAlpha(), 1f, Time.deltaTime / fadeDuration));
            yield return null;
        }
    }

    private IEnumerator FadeOutSprite()
    {
        while (image.canvasRenderer.GetAlpha() > 0f)
        {
            image.canvasRenderer.SetAlpha(Mathf.MoveTowards(image.canvasRenderer.GetAlpha(), 0f, Time.deltaTime / fadeDuration));
            yield return null;
        }
    }
}
