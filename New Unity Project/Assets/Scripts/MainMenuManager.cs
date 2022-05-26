using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("[Fade]")]
    public Image targetImage;
    public float fadeTime = 1f;

    [Header("[�ɼ� ��ư]")]
    public GameObject mainMenuPanel;
    public GameObject optionPanel;
    bool isOption = false;

    private void Update()
    {
        if(isOption==true && Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(FadeAndOptionExit(0,1));   
        }
    }

    // ���� ��ư
    public void StartButton()
    {
        StartCoroutine(FadeAndLoadMainMap(0, 1));
    }
    private IEnumerator FadeAndLoadMainMap(float start, float end)
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
    //================================================================

    // �ɼ� ��ư
    public void OptionButton()
    {
        isOption = true;
        StartCoroutine(FadeAndOptionPanelOn(0, 1));
    }
    private IEnumerator FadeAndOptionPanelOn(float start,float end)
    {
        mainMenuPanel.SetActive(false);
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
        optionPanel.SetActive(true);
        targetImage.gameObject.SetActive(false);
    }
    //=======================================================
    // �ɼ� â ������
    public void OptionExitButton()
    {
        isOption = false;
        StartCoroutine(FadeAndOptionExit(0,1));
    }
    private IEnumerator FadeAndOptionExit(float start, float end)
    {
        optionPanel.SetActive(false);
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
        mainMenuPanel.SetActive(true);
        targetImage.gameObject.SetActive(false);
    }
    //=======================================================
    // ������
    public void ExitButton()
    {
        Application.Quit();
    }
    //=======================================================
}