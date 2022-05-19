using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    S_PlayerController playerController;

    Image HpBar;


    float tempHP;
    private void Awake()
    {
        HpBar = GetComponent<Image>();
    }

    private void Update()
    {
        UpdateHP();
    }

    private void UpdateHP()
    {
        tempHP = playerController.HP/playerController.maxHP;
        HpBar.fillAmount = tempHP;
    }
}
