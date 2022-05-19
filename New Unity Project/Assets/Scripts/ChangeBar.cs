using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBar : MonoBehaviour
{
    [SerializeField]
    GameObject changeMagicPrefab;
    [SerializeField]
    GameObject arousalPrefab;
    [SerializeField]
    S_PlayerAnimatorController playerController;

    Image changeBarImage;

    float chargeTime = 60f;

    public bool isCharge = false;
    public bool isChargeStart;
    public bool isArousal = false;
    private void Awake()
    {
        changeBarImage = GetComponent<Image>();
    }

    private void Update()
    {
        Change();
        if (isCharge == false && isArousal==false)
        {
            StartCoroutine(ChargeImage());
        }
        PlayerArousal();
    }
    private void Change()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && isChargeStart==false)
        {
            isCharge = false;
            isArousal = true;
            changeBarImage.fillAmount = 0;
            changeMagicPrefab.SetActive(true);
            StartCoroutine(DechargeImage());
            StartCoroutine(ChangeState());
        }
    }

    private IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(30f);
        isArousal = false;
    }

    private void PlayerArousal()
    {
        if (isArousal == true)
        {
            playerController.AttackPower = 70;
            arousalPrefab.SetActive(true);
        }
        else if (isArousal == false)
        {
            playerController.AttackPower = 50;
            arousalPrefab.SetActive(false);
        }
    }

    private IEnumerator ChargeImage()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            isCharge = true;
            isChargeStart = true;
            currentTime += Time.deltaTime;
            percent = currentTime / chargeTime;

            changeBarImage.fillAmount = Mathf.Lerp(0, 1, percent);

            yield return null;
        }
        isChargeStart = false;
    }
    private IEnumerator DechargeImage()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 30f;

            changeBarImage.fillAmount = Mathf.Lerp(1, 0, percent);

            yield return null;
        }
    }
}
