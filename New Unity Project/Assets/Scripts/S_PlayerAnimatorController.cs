using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerAnimatorController : MonoBehaviour
{
    [SerializeField]
    private int attackPower = 50;
    [SerializeField]
    private BoxCollider attackCollider;

    public S_PlayerController playerController;
    private Animator anim;
    public bool isAttack = false;

    private void Awake()
    {
        anim=GetComponent<Animator>();
    }

    public void MoveAni(bool isMove,float hAxis,float vAxis)
    {
        anim.SetBool("isMove",isMove);
        anim.SetFloat("horizontal", hAxis);
        anim.SetFloat("vertical", vAxis);
    }
    
    public void AttackAni()
    {
        anim.SetTrigger("onAttack");
    }

    public void AttackStartMoveSpeed(int index)
    {
        isAttack=true;
        playerController.moveSpeed = 0.5f;
    }
    public void AttackEndMoveSpeed()
    {
        isAttack = false;
        if (Input.GetAxis("Vertical") >= 0)
        {
            playerController.moveSpeed = 5f;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            playerController.moveSpeed = 3f;
        }
    }
    
    public void AttackCollider()
    {
        StartCoroutine(OnOff(attackCollider));
    }
    private IEnumerator OnOff(BoxCollider collider)
    {
        collider.enabled = true;

        yield return new WaitForSeconds(0.1f);

        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Orc")
        {
            OrcLogic logic =other.transform.GetComponent<OrcLogic>();
            logic.TakeDamage(attackPower);
        }
        else if(other.transform.tag == "HunterGoblin")
        {
            GoblinLogic logic=other.transform.GetComponent<GoblinLogic>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "GroundGoblin")
        {
            Debug.Log("성공");
            GoblinFSM logic = other.transform.GetComponent<GoblinFSM>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "Golem")
        {
            Debug.Log("성공");
            Golem logic = other.transform.GetComponent<Golem>();
            logic.TakeDamage(attackPower);
        }
        else if (other.transform.tag == "Boss")
        {
            Debug.Log("성공");
            Dragon logic = other.transform.GetComponent<Dragon>();
            logic.TakeDamage(attackPower);
        }
    }
}
