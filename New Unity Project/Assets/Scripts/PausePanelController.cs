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

    [Header("[옵션 버튼]")]
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

    // 옵션 버튼
    public void OptionButton()
    {
        isOption = true;
        pauseMenuPanel.SetActive(false);
        optionPanel.SetActive(true);
    }
    //=======================================================
    // 옵션 창 나가기
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
    // 재개 버튼
    public void ResumeButton()
    {
        isOption = false;
        player.isPause = false;
        pauseMenuPanel.SetActive(false);
    }
    //=======================================================
    // 타이틀 가기 버튼
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
