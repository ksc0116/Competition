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
    private PlayerController playerController;

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

    private bool isFirst=true;

    [SerializeField]
    private GameObject groundEnemy;
    [SerializeField]
    private GameObject flyEnemy;

    private bool isEnemyFirst=true;

    public bool isTutorial=true;

    [SerializeField]
    private GameObject building;
    [SerializeField]
    private GameObject grapCube;

    
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

    // ó�� ������ ���� �Ǿ� �� �� �ý����� ���� �� �� �ؽ�Ʈ, �ڽ� Ȱ��ȭ
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
    // Ʃ�丮�� ���� �� ������ ��
    private IEnumerator LastDialouge()
    {
        systemDialouge[12].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[12].SetActive(false);

        systemDialouge[13].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[13].SetActive(false);

        systemDialouge[14].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[14].SetActive(false);

        systemDialouge[15].SetActive(true);
        yield return new WaitForSeconds(2f);
        systemDialouge[15].SetActive(false);
    }
/*    // �� �� �ؽ�Ʈ
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

    // ���� �����Ȳ�� �°� ��� ������Ʈ ���ִ� �Լ�
    private void CheckTheTutorialProgress()
    {
        if (popUpIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) return;
            // ��Ʋ ��� ����
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if(popUpIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E)|| Input.GetKeyDown(KeyCode.R)|| Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab) || Input.GetMouseButtonDown(0)) return;
            // ���� ��� ����
            if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 2)
        {
            if (isFirst == true)
            {
                playerController.CurShotCount = 1;
                playerController.state = PlayerState.Battle;
            }
            isFirst = false;
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab)|| Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)) return;
            // ������
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) return;
            // �뽬
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex==4)
        {
            if(isFirst == true)
            {
                playerController.CurShotCount = 1;
                playerController.HP = 10;
                playerController.state = PlayerState.Battle;
            }
            isFirst=false;
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) return;
            // �ð� �ǵ�����
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 5)
        {
            if (isFirst == true)
            {
                building.SetActive(true);
                grapCube.SetActive(true);
            }
            isFirst = false;
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) return;
            // �׷��ø� ��
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                building.SetActive(false);
                grapCube.SetActive(false);
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 6)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) return;
            // �Ϲ� ����
            if (Input.GetMouseButtonDown(0) && playerController.state == PlayerState.Battle)
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if (popUpIndex == 7)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.R)) return;
            // ���� ����
            if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0))
            {
                StartCoroutine(UpdateSystemdialouge());
            }
        }
        else if(popUpIndex == 8)
        {
            // �ɾ�ٴϴ� �� �����ϱ�
            if (isFirst == true)
            {
                groundEnemy.SetActive(true);
            }
            isFirst = false;
            if (groundEnemy.activeSelf == false)
            {
                if (isEnemyFirst == true)
                {
                    StartCoroutine(UpdateSystemdialouge());
                }
                isEnemyFirst = false;
            }
        }
        else if (popUpIndex==9)
        {
            // ���� �ٴϴ� �� �����ϱ�
            if(isFirst == true)
            {
                flyEnemy.SetActive(true);
            }
            isFirst = false;
            if (flyEnemy.activeSelf == false)
            {
                if(isEnemyFirst == true)
                {
                    StartCoroutine(UpdateSystemdialouge());
                }
                isEnemyFirst= false;
            }
        }
    }

    private IEnumerator UpdateSystemdialouge()
    {
        // �÷��̾ �־��� �ൿ�� �ϸ� �� �� �ؽ�Ʈ�� �ؽ�Ʈ �ڽ� ��Ȱ��ȭ �ϸ鼭 Great! �ؽ�Ʈ ���
        popUpText[popUpIndex].SetActive(false);
        popUpTextBox.SetActive(false);
        systemRepeat.SetActive(true);

        // 1�� �ڿ� Great! �ؽ�Ʈ ��Ȱ��ȭ
        yield return new WaitForSeconds(1f);
        systemRepeat.SetActive(false);

        if (popUpIndex == popUpText.Length-1)
        {
            tutorialPortal.SetActive(true);
            isTutorial = false;
            StartCoroutine(LastDialouge());
            yield break;
        }

        // 1�� �ڿ� ���� �ɷ¿� ���� ������ �ý����� ����
        yield return new WaitForSeconds(1f);
        systemDialouge[systemDialougeIndex].SetActive(true);

        // 2�� �ڿ� ���� �����
        yield return new WaitForSeconds(2f);
        systemDialouge[systemDialougeIndex].SetActive(false);

        // �̸� ������ ������ ���� �ε����� ������Ŵ
        systemDialougeIndex++;

        // �� �� �ؽ�Ʈ �ڽ��� �ٽ� Ȱ��ȭ �ϸ鼭 �ɷ��� �����ϴ� ����� ���� �ؽ�Ʈ ���
        popUpTextBox.SetActive(true);
        popUpIndex++;
        popUpText[popUpIndex].SetActive(true);
        isFirst = true;
        isEnemyFirst=true;
    }
}