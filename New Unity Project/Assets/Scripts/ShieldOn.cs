using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShieldOn : MonoBehaviour
{
    [SerializeField]
    Image durationImage;
    [SerializeField]
    GameObject shieldObject;
    [SerializeField]
    Image coolTimeImage;
    [SerializeField]
    TextMeshProUGUI coollTimeText;

    float coolTime = 5f;

    float onTime = 0.5f;

    Vector3 tempScale = new Vector3(1, 1, 1);


    bool isCool;

    float currentCoolTime;

    private void Awake()
    {
        durationImage.fillAmount= 0f;
        Color color=Color.white;
        color.a = 0f;
        coollTimeText.color = color;
        coollTimeText.text=coolTime.ToString();
        coolTimeImage.fillAmount = 0f;
        shieldObject.transform.localScale = Vector3.zero;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isCool==false)
        {
            StartCoroutine(ConductShield());
            StartCoroutine(DurationImage());
        }
    }
    private IEnumerator DurationImage()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currentTime+=Time.deltaTime;
            percent = currentTime / coolTime;

            durationImage.fillAmount = Mathf.Lerp(1, 0, percent);

            yield return null;
        }
    }
    private IEnumerator ConductShield()
    {
        isCool=true;
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1f)
        {
            currentTime+=Time.deltaTime;
            percent = currentTime / onTime;

            shieldObject.transform.localScale=Vector3.Lerp(shieldObject.transform.localScale, tempScale, percent);

            yield return null;
        }
        yield return new WaitForSeconds(4.5f);
        StartCoroutine(DeConductShield());
    }
    private IEnumerator DeConductShield()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / onTime;

            shieldObject.transform.localScale = Vector3.Lerp(shieldObject.transform.localScale, Vector3.zero, percent);

            yield return null;
        }
        StartCoroutine(CoolTime());
        StartCoroutine(CoolTimeText());
    }
    private IEnumerator CoolTime()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while(percent < 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / coolTime;

            coolTimeImage.fillAmount = Mathf.Lerp(1, 0, percent);

            yield return null;
        }
        isCool = false;
    }
    private IEnumerator CoolTimeText()
    {
        currentCoolTime = coolTime;
        Color color = Color.white;
        color.a = 1f;
        coollTimeText.color = color;
        coollTimeText.text = currentCoolTime.ToString();
        while (currentCoolTime != 0)
        {
            yield return new WaitForSeconds(1f);
            currentCoolTime -= 1;
            coollTimeText.text = currentCoolTime.ToString();
        }
        isCool = false;
        color.a = 0f;
        coollTimeText.color = color;
    }
}
