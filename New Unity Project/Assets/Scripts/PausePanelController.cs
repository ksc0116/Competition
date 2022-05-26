using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
    [Header("[Fade]")]
    public Image targetImage;
    public float fadeTime = 1f;

    [Header("[�ɼ� ��ư]")]
    public GameObject pauseMenuPanel;
    public GameObject optionPanel;
    public bool isOption = false;

    [SerializeField]
    S_PlayerController player;
    private void Update()
    {
        if (isOption == true && Input.GetKeyDown(KeyCode.Escape))
        {
            OptionExit();
        }
    }

    // �ɼ� ��ư
    public void OptionButton()
    {
        isOption = true;
        pauseMenuPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    //=======================================================
    // �ɼ� â ������
    public void OptionExitButton()
    {
        isOption = false;
        OptionExit();   
    }
    private void OptionExit()
    {
        isOption = false;
        pauseMenuPanel.SetActive(true);
        optionPanel.SetActive(false);
    }
    //=======================================================
    // �簳 ��ư
    public void ResumeButton()
    {
        isOption = false;
        player.isPause = false;
        pauseMenuPanel.SetActive(false);
    }
    //=======================================================
    // Ÿ��Ʋ ���� ��ư
    public void ToTitleButton()
    {
        StartCoroutine(FadeAndToTheTitle(0,1));
    }
    private IEnumerator FadeAndToTheTitle(float start, float end)
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
        SceneManager.LoadScene("StartScreen");
    }
    //=======================================================
}
