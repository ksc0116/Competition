using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DragonPathEndDoor : MonoBehaviour
{
    [SerializeField]
    GameObject dangerPanel;
    [SerializeField]
    Image dangerBackGround;
    [SerializeField]
    TextMeshProUGUI dangerText;
    [SerializeField]
    private Transform doorOriginPos;
    [SerializeField]
    private Transform doorClosePos;
    [SerializeField]
    private Dragon dragon;

    float pingpongTime = 1f;
    private IEnumerator CloseDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 150.0f;

            doorOriginPos.position=Vector3.Lerp(doorOriginPos.position,doorClosePos.position,percent);

            yield return null;
        }
    }
    private IEnumerator PingPongUI()
    {
        while (true)
        {
            Color colorImage=dangerBackGround.color;
            colorImage.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time*pingpongTime,1));
            dangerBackGround.color= colorImage;

            Color colorText=dangerText.color;
            colorText.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * pingpongTime, 1));
            dangerText.color= colorText;

            yield return null;
        }
    }
    private IEnumerator AutoUIDisable()
    {
        yield return new WaitForSeconds(2.5f);
        dangerPanel.gameObject.SetActive(false);
        dangerBackGround.gameObject.SetActive(false);
        dangerText.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CloseDoor());
            StartCoroutine( dragon.ScreamAni());
            StartCoroutine ( PingPongUI());
            StartCoroutine(AutoUIDisable());
            dangerPanel.gameObject.SetActive(true);
            dangerBackGround.gameObject.SetActive(true);
            dangerText.gameObject.SetActive(true);
        }
    }
}
