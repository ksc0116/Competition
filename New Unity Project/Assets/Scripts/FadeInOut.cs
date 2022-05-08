using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image targetImage;

    public float fadeTime = 1f;

    public void StartButton()
    {
        StartCoroutine(FadeOut(0, 1));
    }
    private IEnumerator FadeOut(float start, float end)
    {
        targetImage.gameObject.SetActive(true);
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = targetImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            targetImage.color = color;

            yield return null;
        }
        SceneManager.LoadScene("MainMap");
    }
}
