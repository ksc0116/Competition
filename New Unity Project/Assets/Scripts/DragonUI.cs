using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DragonUI : MonoBehaviour
{
    [SerializeField] Dragon dragonLogic;
    [SerializeField] RectTransform[] fillArea;
    [SerializeField] TextMeshProUGUI hpText;
    [SerializeField] TextMeshProUGUI maxHpText;
    [SerializeField] TextMeshProUGUI totalNumberText;

    float sumDamage = 0;
    int fillAreaIndex = 0;
    private void OnEnable()
    {
        hpText.text = dragonLogic.HP.ToString();
        maxHpText.text = "/ " + dragonLogic.MaxHP.ToString();
        totalNumberText.text = "x" + ((int)dragonLogic.HP / dragonLogic.MaxHP).ToString();
    }
    private void Update()
    {
        hpText.text = dragonLogic.HP.ToString();
        totalNumberText.text = "x" + ((int)(dragonLogic.HP / 100)).ToString();
    }

    public void ChangeFillArea(float damage)
    {
        sumDamage += damage;
        if (sumDamage >= 100)
        {
            if (dragonLogic.HP>0)
            {
                fillArea[fillAreaIndex].gameObject.SetActive(false);
                sumDamage = sumDamage - 100;
                if (fillAreaIndex < fillArea.Length - 1)
                {
                    fillAreaIndex++;
                }
                else
                {
                    fillAreaIndex = 0;
                }
            }
        }
         fillArea[fillAreaIndex].localScale =new Vector3(1 - (sumDamage / 100),1,1);
         Debug.Log(1-(sumDamage/100));   
    }

}
