using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public int HP = 300;
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
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        anim = GetComponent<Animator>();
        StartCoroutine(IsmoveChange());
        this.target = target;
    }
    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    private void Update()
    {
/*        LookTarget();
        Pursuit();
        LookTarget();*/
    }

    private void Pursuit()
    {
        if (isMove == false) return;

        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        Vector3 moveDirection = (to - from).normalized;

        transform.position+=moveDirection*moveSpeed*Time.deltaTime;
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
        float distance =Vector3.Distance(transform.position, target.position);
        if (distance <= attackRange)
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

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            isDie = true;
        }
        StartCoroutine(ChangeColor());
    }
    private IEnumerator ChangeColor()
    {
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
}