using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField]
    Image coolTimeImage;
    [SerializeField]
    TextMeshProUGUI coolTimeText;

    float coolTime = 2f;

    float currentCoolTime;


    public Transform characterTransform;

    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsgrappleable;
    public Transform grapPos;
    public Transform player;
    private float maxDistance = 50f;

    bool hooked=false;
    bool isCool = false;

    Vector3 tempForward=Vector3.zero;
    Vector3 tempUp= Vector3.zero;

    private void Awake()
    {
        coolTimeImage.fillAmount = 0;
        Color color = Color.white;
        color.a = 0;
        coolTimeText.color = color;
        lr = GetComponent<LineRenderer>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftShift) && isCool==false)
        {
            StartGrapple();
        }
/*        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopGrapple();
        }*/
        MovePlayer();
    }
    private void LateUpdate()
    {
        DrawRope();
    }
    // 아래가 진짜 코드
    /*    private void StartGrapple()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
            if (Physics.SphereCast(ray, cognitionRange, out hit, maxDistance,whatIsgrappleable))
            {
                grapplePoint = hit.point;
                joint=player.gameObject.AddComponent<SpringJoint>();
                joint.autoConfigureConnectedAnchor = false;
                joint.connectedAnchor = grapplePoint;

                float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

                joint.maxDistance = distanceFromPoint*0.001f;
                joint.minDistance = distanceFromPoint * 0.0005f;

                joint.spring = 7f;
                joint.damper = 7f;
                joint.massScale = 4.5f;

                lr.positionCount = 2;
            }
        }*/
    private void StartGrapple()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(Vector2.one * 0.5f);
        if (Physics.Raycast(ray,  out hit, maxDistance, whatIsgrappleable))
        {
            tempForward = characterTransform.transform.forward;
            tempUp = characterTransform.transform.up;
            hooked = true;
            grapplePoint = hit.point;
            
            lr.positionCount = 2;
            StartCoroutine(CoolTime());
            StartCoroutine(CoolTimeText());
        }
        else
        {
            hooked=false;
        }
    }
    private void MovePlayer()
    {
        if (hooked == true)
        {
            player.GetComponent<Rigidbody>().useGravity = false;
            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);
            player.position = Vector3.MoveTowards(player.position, grapplePoint, 20f * Time.deltaTime);
            if(distanceFromPoint < 1)
            {
                player.Translate(tempForward * Time.deltaTime * 50f);
                player.Translate( tempUp* Time.deltaTime * 60f);
                StopGrapple();
            }
        }
    }
    void DrawRope()
    {
        if (hooked==false) return;
        lr.SetPosition(0, grapPos.position);
        lr.SetPosition(1, grapplePoint);
    }

    private void StopGrapple()
    {
        player.GetComponent<Rigidbody>().useGravity = true;
        hooked = false;
        lr.positionCount = 0;
    }
    private IEnumerator CoolTime()
    {
        isCool = true;
        float currentTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
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
        color.a = 1;
        coolTimeText.color = color;
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