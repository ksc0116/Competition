using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStat : MonoBehaviour 
{
    public float atk;
    public float def;
    public float moveSpeed;

    [SerializeField] TextMeshProUGUI atkText;
    [SerializeField] TextMeshProUGUI defText;
    [SerializeField] TextMeshProUGUI moveSpeedText;
    [SerializeField] GameObject playerStatPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (playerStatPanel.activeSelf == true)
            {
                playerStatPanel.SetActive(false);
            }
            else if (playerStatPanel.activeSelf == false)
            {
                playerStatPanel.SetActive(true);
                
            }
        }
        if (playerStatPanel.activeSelf == true)
        {
            atkText.text = atk.ToString();
            defText.text = def.ToString();
            moveSpeedText.text = moveSpeed.ToString();
        }
    }
}
