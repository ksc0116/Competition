using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCallBack : MonoBehaviour
{
    private S_PlayerController playerController;
    [SerializeField]
    private GameObject callBackFlashPrefab;
    private float checkTime = 5f;
    public float curTime=0f;

    private Vector3 previousPosition;

    private float callbackTime = 0.45f;
    [SerializeField]
    private GameObject playerBody;

    private void Awake()
    {
        playerController=GetComponent<S_PlayerController>();
    }

    private void Update()
    {
        StorePosition();
        CallBackPosition();
    }
    private void StorePosition()
    {
        curTime += Time.deltaTime;
        if(curTime > checkTime)
        {
            previousPosition = transform.position;
            curTime = 0;
        }
    }

    private void CallBackPosition()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("CallBackPositioning");
        }
    }

    private IEnumerator CallBackPositioning()
    {
        playerController.isCallBack = true;
        playerController.HP = playerController.maxHP;
        float currentTime = 0.0f;
        float percent = 0.0f;
        playerBody.SetActive(false);
        GameObject cloneFlash= Instantiate(callBackFlashPrefab,transform.position,Quaternion.identity);
        cloneFlash.SetActive(false);
        cloneFlash.transform.localScale = new Vector3(5f, 5f, 5f);
        cloneFlash.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(cloneFlash);
        yield return new WaitForSeconds(0.3f);
        while (percent <= 1f)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / callbackTime;

            transform.position = Vector3.Lerp(transform.position,previousPosition,percent);

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        GameObject cloneFlash2 = Instantiate(callBackFlashPrefab, transform.position, Quaternion.identity);
        cloneFlash2.SetActive(false);
        cloneFlash2.transform.localScale = new Vector3(5f, 5f, 5f);
        cloneFlash2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Destroy(cloneFlash2);
        playerBody.SetActive(true);
        playerController.isCallBack = false;
    }
}
