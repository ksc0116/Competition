using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Golem : MonoBehaviour
{
    [Header("HP바")]
    [SerializeField] Transform HpBar;
    Camera cam;
    [SerializeField] Slider hpSlider;

    private CapsuleCollider attackCollider;
    private BoxCollider boxCollider;

    private float HP;
    float maxHP = 100f;
    private int attackPower = 60;
    float attackRate=2.5f;
    float lastAttackTime;
    private bool isDie = false;

    private Transform target;

    private bool isMove = false;
    private bool isAttack = false;

    private Animator anim;

    public float moveSpeed = 2f;

    private float attackRange = 4f;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        HpBar.gameObject.SetActive(false);
        cam = Camera.main;
        HP = maxHP;
    }
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
        Quaternion q_hp = Quaternion.LookRotation(HpBar.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(HpBar.rotation, q_hp, 1000).eulerAngles;
        HpBar.rotation = Quaternion.Euler(0, hp_angle.y, 0);
        hpSlider.value = HP / maxHP;
    }
    private void Pursuit()
    {
        if (isMove == false || isDie==true ) return;

/*        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);*/

        /*Vector3 moveDirection = (to - from).normalized;*/

        Vector3 moveDir=new Vector3(target.position.x,transform.position.y,target.position.z); 
       transform.position=Vector3.MoveTowards(transform.position, moveDir, moveSpeed*Time.deltaTime);
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
        if (distance <= attackRange && isAttack==false &&   Time.time - lastAttackTime > attackRate)
        {
            lastAttackTime = Time.time;
            int index = Random.Range(0, 2);
            isAttack = true;
            if (index == 0) anim.SetTrigger("onAttack1");
            else if (index == 1) anim.SetTrigger("onAttack2");
        }
    }

    public void AttackStart()
    {
        isAttack = true;
        moveSpeed = 0;
    }
    public void AttackEnd()
    {
       isAttack=false;
        moveSpeed = 2f;
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
    public void TakeDamage(float damage)
    {
        if(isDie==true) return;
        HpBar.gameObject.SetActive(true);
        HP -= damage;
        if (HP <= 0)
        {
            HpBar.gameObject.SetActive(false);
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
            Debug.Log("플레이어 공격");
            S_PlayerController logic = other.GetComponent<S_PlayerController>();
            logic.TakeDamage(attackPower);
        }
    }
}