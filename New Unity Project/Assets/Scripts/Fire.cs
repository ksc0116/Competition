using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public Transform camera;
    public GameObject bulletPrefab;
    public GameObject aimBulletPrefab;
    public Transform bulletFirePos;
    private float curAtkTime;
    private float maxAtkTime = 0.3f;

    private PlayerController player;

    private Vector3 targetPoint;

    private PlayerAnimatorController playerAnimator;

    private void Awake()
    {
        player = GetComponent<PlayerController>();
        playerAnimator=GetComponentInChildren<PlayerAnimatorController>();
    }
    void Update()
    {
        curAtkTime += Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && curAtkTime > maxAtkTime && (player.state==PlayerState.Battle || player.state==PlayerState.AimMode))
        {
            curAtkTime = 0;
            RaycastHit hit;
            Ray ray=Camera.main.ViewportPointToRay(Vector2.one*0.5f);
            if(Physics.Raycast(ray,out hit,Mathf.Infinity))
            {
                targetPoint = hit.point;
                Vector3 dir=targetPoint - bulletFirePos.position;
                Attack(dir.normalized);
            }
            else
            {
                Attack(ray.direction);
            }
        }
    }

    private void Attack(Vector3 dir)
    {
        if (player.CurShotCount == 0) return;

        if (player.state == PlayerState.AimMode)
        {
            CameraShake.Instance.OnShakeCamera(0.1f, 0.01f,true);
            Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.shot2);
        }
        else if (player.state == PlayerState.Battle)
        {
            CameraShake.Instance.OnShakeCamera(0.1f, 0.01f, false);
            Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.shot1);
        }

        playerAnimator.AttackAni();


        if (player.state == PlayerState.Battle)
        {
            GameObject instanceBullet = Instantiate(bulletPrefab, bulletFirePos.position, Quaternion.identity);
            instanceBullet.transform.position = bulletFirePos.position;
            Rigidbody rigid = instanceBullet.GetComponent<Rigidbody>();
            rigid.AddForce(dir * 100f, ForceMode.VelocityChange);
        }
        else if(player.state == PlayerState.AimMode)
        {
            GameObject instanceBullet = Instantiate(aimBulletPrefab, bulletFirePos.position, Quaternion.identity);
            instanceBullet.transform.position = bulletFirePos.position;
            Rigidbody rigid = instanceBullet.GetComponent<Rigidbody>();
            rigid.AddForce(dir * 250f, ForceMode.VelocityChange);
        }
        player.CurShotCount--;
    }
}