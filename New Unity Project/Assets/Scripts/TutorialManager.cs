using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    private float fadeTime = 1f;

    [SerializeField]
    private GameObject tutorialPortal;

    [SerializeField]
    private S_PlayerController playerController;

    [SerializeField]
    private GameObject popUpTextBox;
    [SerializeField]
    private GameObject[] popUpText;
    public int popUpIndex=0;

    [SerializeField]
    private GameObject[] systemDialouge;

    private int systemDialougeIndex=3;

    [SerializeField]
    private GameObject systemRepeat;


    public bool isTutorial=true;
    [SerializeField]
    private GameObject grapCube;


    public bool tutorialChange = false;
    private void Awake()
    {
        if (isTutorial == false) return;
        fadeImage.gameObject.SetActive(true);
        StartCoroutine( Fade(1, 0));
        isTutorial = true;
        StartCoroutine(Start());
    }
    private IEnumerator Fade(float start,float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent<1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start, end, percent);
            fadeImage.color = color;

            yield return null;
        }
    }
    private IEnumerator Start()
    {
        if (isTutorial == false) yield break;
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(FirstDialouge());
    }
    private void Update()
    {
        if (isTutorial == false)
        {
            tutorialPortal.SetActive(true);
            return;
        }
        if (isTutorial == true)
        {
            CheckTheTutorialProgress();
        }
    }

    // 처음 게임이 실행 되었 을 때 시스템의 말과 위 쪽 텍스트, 박스 활성화
    private IEnumerator FirstDialouge()
    {
        systemDialouge[0].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[0].SetActive(false);

        systemDialouge[1].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[1].SetActive(false);

        systemDialouge[2].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[2].SetActive(false);

        popUpTextBox.SetActive(true);
        popUpText[0].SetActive(true);
    }
    // 튜토리얼 끝날 때 나오는 말
    private IEnumerator LastDialouge()
    {
        systemDialouge[7].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[7].SetActive(false);

        systemDialouge[8].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[8].SetActive(false);

        systemDialouge[9].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[9].SetActive(false);

        systemDialouge[10].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[10].SetActive(false);
    }
/*    // 위 쪽 텍스트
    private void UpdateExplanation()
    {
        for (int i = 0; i < popUpText.Length; i++)
        {
            if (i == popUpIndex)
            {
                popUpText[i].SetActive(true);
            }
            else
            {
                popUpText[i].SetActive(false);
            }
        }
    }*/

    // 현재 진행상황에 맞게 계속 업데이트 해주는 함수
    private void CheckTheTutorialProgress()
    {
        if (popUpIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift)  || Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Tab) ) return;
            // 대쉬 사용하기
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if(popUpIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.F)|| Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab) ) return;
            // 쉴드 사용하기
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Tab)|| Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R) ) return;
            // 내려찍기
            if (Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)) return;
            // 그래플링
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.F)  || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.LeftShift)) return;
            // 각성모드
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                tutorialChange = true;
                StartCoroutine(UpdateSystemdialouge());
            }
        }
    }

    private IEnumerator UpdateSystemdialouge()
    {
        // 플레이어가 주어진 행동을 하면 위 쪽 텍스트와 텍스트 박스 비활성화 하면서 Great! 텍스트 출력
        popUpText[popUpIndex].SetActive(false);
        popUpTextBox.SetActive(false);
        systemRepeat.SetActive(true);

        // 1초 뒤에 Great! 텍스트 비활성화
        yield return new WaitForSeconds(1f);
        systemRepeat.SetActive(false);

        if (popUpIndex == popUpText.Length-1)
        {
            tutorialPortal.SetActive(true);
            /*isTutorial = false;*/
            StartCoroutine(LastDialouge());
            yield break;
        }

        // 1초 뒤에 다음 능력에 대한 설명을 시스템이 해줌
        yield return new WaitForSeconds(1f);
        systemDialouge[systemDialougeIndex].SetActive(true);

        // 2초 뒤에 설명 사라짐
        yield return new WaitForSeconds(2f);
        systemDialouge[systemDialougeIndex].SetActive(false);

        // 미리 다음에 보여줄 설명 인덱스를 증가시킴
        systemDialougeIndex++;

        // 위 쪽 텍스트 박스를 다시 활성화 하면서 능력을 실행하는 방법에 대한 텍스트 출력
        popUpTextBox.SetActive(true);
        popUpIndex++;
        popUpText[popUpIndex].SetActive(true);
    }
}