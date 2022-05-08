using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarUI : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Slider hpBar;
    private void Awake()
    {
        hpBar.value = (float)player.HP / player.HPMax;
    }


    private void Update()
    {
        HandleHP();
    }
    private void HandleHP()
    {
        hpBar.value = (float)player.HP / player.HPMax;
    }
}
