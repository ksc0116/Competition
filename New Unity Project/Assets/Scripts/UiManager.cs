using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UiManager : MonoBehaviour
{
    public GameObject[] UI;
    private Vector3[] originUIScale;

    [SerializeField]
    private PlayerController player;

    [Header("Reloadbar")]
    [SerializeField]
    private Image reloadImage;
    [SerializeField]
    private Image reloadBar;
    [SerializeField]
    private TextMeshProUGUI curShotCountText;
    [SerializeField]
    private TextMeshProUGUI RText;

    [Header("HPBar")]
    [SerializeField]
    private Image hpBarBackGround;
    [SerializeField]
    private Image fillImage;
    [SerializeField]
    private TextMeshProUGUI hpText;
    private void Awake()
    {
        originUIScale = new Vector3[UI.Length];
        for(int i = 0; i < originUIScale.Length; i++)
        {
            originUIScale[i]=UI[i].GetComponent<RectTransform>().localScale;
        }
    }

    private void Update()
    {
        StateOfUI();
        AimUI();
    }

    private void StateOfUI()
    {
        if (player.state == PlayerState.NonBattle)
        {
            Color color = reloadImage.color;
            color.a = 0f;
            reloadImage.color = color;

            color = curShotCountText.color;
            color.a = 0f;
            curShotCountText.color = color;

            color = reloadBar.color;
            color.a = 0f;
            reloadBar.color = color;

            color = RText.color;
            color.a = 0f;
            RText.color = color;

            color = hpBarBackGround.color;
            color.a = 0f;
            hpBarBackGround.color = color;

            color = fillImage.color;
            color.a = 0f;
            fillImage.color = color;

            color = hpText.color;
            color.a = 0f;
            hpText.color = color;
        }
        else
        {
            Color color = reloadImage.color;
            color.a = 1f;
            reloadImage.color = color;

            color = curShotCountText.color;
            color.a = 1f;
            curShotCountText.color = color;

            color = reloadBar.color;
            color.a = 1f;
            reloadBar.color = color;

            color = RText.color;
            color.a = 1f;
            RText.color = color;

            color = hpBarBackGround.color;
            color.a = 1f;
            hpBarBackGround.color = color;

            color = fillImage.color;
            color.a = 1f;
            fillImage.color = color;

            color = hpText.color;
            color.a = 1f;
            hpText.color = color;
        }
    }

    private void AimUI()
    {
        if (player.state == PlayerState.AimMode)
        {
            for(int i =0;i<UI.Length;i++)
            {
                if (i < 4)
                {
                    UI[i].GetComponent<RectTransform>().localScale = Vector3.one;
                }
                else
                {
                    UI[i].GetComponent<RectTransform>().localScale =new Vector3(1.1f, 1.1f, 1.1f);
                }
            }
        }
        else
        {
            for(int i = 0; i < UI.Length; i++)
            {
                UI[i].GetComponent<RectTransform>().localScale = originUIScale[i];
            }
        }
    }
}