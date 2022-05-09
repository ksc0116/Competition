using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    private CapsuleCollider attackCollider;
    private BoxCollider boxCollider;

    private int HP = 50;
    private int attackPower = 60;
    private bool isDie = false;

    private Transform target;

    private bool isMove = false;
    private bool isAttack = false;

    private Animator anim;

    private float moveSpeed = 2f;

    private float attackRange = 4f;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    public void Setup(Transform target)
    {
        boxCollider=GetComponent<BoxCollider>();
        attackCollider =GetComponent<CapsuleCollider>();
        isDie = false;
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(IsmoveChange());
        this.target = target;
    }
    private void Update()
    {
        LookTarget();
        Attack();
        Pursuit();
    }
    private void Pursuit()
    {
        if (isMove == false || isDie==true) return;

/*        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);*/

        /*Vector3 moveDirection = (to - from).normalized;*/

       transform.position=Vector3.MoveTowards(transform.position, target.position,moveSpeed*Time.deltaTime);
    }

    private IEnumerator IsmoveChange()
    {
        yield return new WaitForSeconds(1f);
        isMove = true;
        anim.SetBool("isMove", isMove);
    }
    private void LookTarget()
    {
        if (isAttack == true || isDie==true) return;

        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.1f);
    }

    private void Attack()
    {
        if(isDie == true) return;
        float distance =Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange && isAttack==false)
        {
            int index = Random.Range(0, 2);
            isAttack = true;
            if (index == 0) anim.SetTrigger("onAttack1");
            else if (index == 1) anim.SetTrigger("onAttack2");
        }
    }

    public void AttackStart()
    {
        isAttack = true;
    }
    public void AttackEnd()
    {
       isAttack=false;
    }
    public void OnAttackCollider()
    {
        StartCoroutine(OnOff());
    }
    private IEnumerator OnOff()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackCollider.enabled = false;
    }
    public void TakeDamage(int damage)
    {
        if(isDie==true) return;
        HP -= damage;
        if (HP <= 0)
        {
            Manager.Instance.golemSceneManager.golemCount--;
            boxCollider.enabled = false;
            isDie = true;
            StartCoroutine(Die());
        }
        StartCoroutine(ChangeColor());
    }
    private IEnumerator Die()
    {
        anim.SetTrigger("onDie");
        yield return new WaitForSeconds(2.5f);
        gameObject.SetActive(false);
    }
    private IEnumerator ChangeColor()
    {
        skinnedMeshRenderer.material.color = Color.white;
        Color color=skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = color;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            S_PlayerController logic = other.GetComponent<S_PlayerController>();
            logic.TakeDamage(attackPower);
        }
    }
}