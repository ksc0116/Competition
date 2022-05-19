using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class S_PlayerDownSkill : MonoBehaviour
{
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    GameObject smallExplosionPrefab;
    [SerializeField]
    GameObject playerCirecle;

    [SerializeField]
    CheckGround checkGround;

    [SerializeField]
    Image coolTimeImage;
    [SerializeField]
    TextMeshProUGUI coolTimeText;

    Rigidbody rigid;


    float coolTime=3f;
    float currentCoolTime;

    bool isInputAble=true;

    public bool isDownSkill;
    private void Awake()
    {
        coolTimeImage.fillAmount = 0;
        Color color = Color.white;
        color.a = 0;
        coolTimeText.color = color;
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        PlayerCircle();
        PressDownSkill();
        CheckDownCollider();
    }

    private void PressDownSkill()
    {
        if (Input.GetKeyDown(KeyCode.F) && isInputAble==true && checkGround.isGround==false)
        {
            isInputAble = false;

            isDownSkill = true;

            rigid.isKinematic = true;
            rigid.isKinematic = false;
            rigid.AddForce(Vector3.down * 100f, ForceMode.Impulse);
            StartCoroutine(CoolTime());
            StartCoroutine(CoolTimeText());
        }
    }

    private void CheckDownCollider()
    {
        if (isDownSkill == true)
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position+new Vector3(0,0.5f,0), Vector3.down*0.6f, Color.green);
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), Vector3.down, out hit, 0.6f,layerMask))
            {
                isDownSkill = false;
                Instantiate(smallExplosionPrefab,hit.point,Quaternion.identity);
            }
        }
    }

    private void PlayerCircle()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), Vector3.down * 100f, Color.red);
        if(Physics.Raycast(transform.position+new Vector3(0,0.5f,0),Vector3.down, out hit, 100f, layerMask))
        {
            playerCirecle.transform.position = hit.point+new Vector3(0,0.01f,0);
        }
    }

    private IEnumerator CoolTime()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            Debug.Log("½ÇÇà");
            currentTime += Time.deltaTime;
            percent = currentTime / coolTime;

            coolTimeImage.fillAmount = Mathf.Lerp(1, 0, percent);

            yield return null;
        }
        isInputAble = true;
    }
    private IEnumerator CoolTimeText()
    {
        Color color = Color.white;
        color.a = 1f;
        coolTimeText.color = color;
        currentCoolTime = coolTime;
        coolTimeText.text = currentCoolTime.ToString();
        while (currentCoolTime != 0)
        {
            yield return new WaitForSeconds(1.0f);
            currentCoolTime -= 1;
            coolTimeText.text = currentCoolTime.ToString();
        }
        color.a = 0;
        coolTimeText.color = color;
    }
}
