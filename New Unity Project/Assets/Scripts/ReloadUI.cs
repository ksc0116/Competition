using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReloadUI : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TextMeshProUGUI curShotCountText;
    [SerializeField]
    private Image reloadBar;
    [SerializeField]
    private GameObject reloadBarObj;


    private float curTime;
    private float reloadTime = 1f;

    private void Update()
    {
        CurShotCountUpdate();
        Reloadbar();
    }


    private void CurShotCountUpdate()
    {
        curShotCountText.text = player.CurShotCount.ToString();
    }
    private void Reloadbar()
    {
        if (player.state == PlayerState.Reload)
        {
            reloadBarObj.SetActive(true);
            curTime += Time.deltaTime;
            if (curTime < reloadTime)
            {
                reloadBar.fillAmount = Mathf.Lerp(1, 0, curTime);
            }
        }
        else
        {
            reloadBarObj.SetActive(false);
            curTime = 0;
        }
    }
}
