using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [SerializeField]
    GameObject dashEffect;
    [SerializeField]
    AudioSource sfxSource;
    public Image[] dashImages;
    public S_PlayerController playerController;
    public Transform camTransform;
    public int uses;
    public float coolDown, distance, speed, destinationMultiplier;
    public LayerMask layerMask;
    public bool isDash;

    int maxUses;
    float coolDownTimer;

    bool isBorder;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        maxUses = uses;
        coolDownTimer = 0;
    }
    private void Update()
    {
        CheckCollider();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        if (uses < maxUses)
        {
            if (coolDownTimer <coolDown)
            {
                coolDownTimer += Time.deltaTime;
                dashImages[uses].fillAmount = coolDownTimer / coolDown;
            }
            else
            {
                uses += 1;
                coolDownTimer = 0;
                dashImages[uses-1].fillAmount = 1;
            }
        }
    }

    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && uses>0)
        {
            StartCoroutine("DashStart");
        }
    }

    private IEnumerator DashStart()
    {
        uses -= 1;

        Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.dashSound, sfxSource.volume * 0.5f); 

        if (uses < maxUses-1)
        {
            dashImages[uses+1].fillAmount = 0;
        }

        isDash = true;
        rigid.useGravity = false;
        dashEffect.SetActive(true);
        playerController.moveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        dashEffect.SetActive(false);
        playerController.moveSpeed = 5f;
        yield return new WaitForSeconds(0.1f);
        rigid.useGravity = true;
        isDash = false;
    }

    private void CheckCollider()
    {
        if (isDash == false) return;
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(hAxis, 0, vAxis);
        Vector3 position = transform.position + new Vector3(0, 0.5f, 0);
        RaycastHit hit;

        Debug.DrawRay(position, moveDirection.normalized * 1f, Color.green);
        if (Physics.Raycast(position, moveDirection.normalized, out hit, 1f, layerMask))
        {
            isBorder = true;
            /*Debug.Log(hit.transform.name);*/
            playerController.moveSpeed = 0f;
        }
        else
        {
            isBorder = false;
            if (isDash == false)
            {
                playerController.moveSpeed = 5f;
            }
        }
    }

    /*    private void StartBlink()
        {
            if (uses > 0)
            {
                float hAxis = Input.GetAxis("Horizontal");
                float vAxis = Input.GetAxis("Vertical");

                Vector2 moveInput = new Vector2(hAxis, vAxis);

                Vector3 lookForward = new Vector3(camTransform.forward.x, 0, camTransform.forward.z).normalized;
                Vector3 lookRight = new Vector3(camTransform.right.x, 0, camTransform.right.z).normalized;
                Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
                uses -= 1;

                RaycastHit hit;
                Vector3 rayStartposition=transform.position+new Vector3(0,0.5f,0);

                if(Physics.Raycast(rayStartposition, moveDir, out hit, distance, layerMask))
                {
                    destination = hit.point*destinationMultiplier;
                }
                else
                {
                    destination = (transform.position + moveDir * distance) * destinationMultiplier;
                    Vector3 tempDir=transform.position-destination;
                    Debug.DrawRay(transform.position, tempDir,Color.green,2);
                }
                isBlink = true;
            }
        }*/
}