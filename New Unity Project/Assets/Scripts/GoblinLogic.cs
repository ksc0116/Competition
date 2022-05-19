using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinLogic : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private int HP = 100;

    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private Transform firePos;

    [SerializeField]
    private Transform target;

    private Animator anim;

    [Header("Attack")]
    [SerializeField]
    private float attackRate = 5;
    private float lastAttackTime = 0;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private bool isDie = false;

    LineRenderer lr;
    public void SetUp(Transform target)
    {
        this.target = target;
    }

    private void Awake()
    {
        lr=GetComponent<LineRenderer>();
        boxCollider.enabled = true;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isDie == false)
        {
            LookTarget();
            Attack();
        }
    }

    private void LookTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        // �ٷ� ����
        transform.rotation = Quaternion.LookRotation(to - from);
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > attackRate)
        {
            attackRate = Random.Range(2, 6);
            lastAttackTime = Time.time;

            anim.SetTrigger("onAttack");
        }
    }
    // �ִϸ��̼� �̺�Ʈ���� ȣ��
    public void Shot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePos.position, Quaternion.identity);
        arrow.transform.localScale = Vector3.one * 0.2f;
        arrow.transform.rotation = Quaternion.LookRotation(target.position - arrow.transform.position);
        Rigidbody arrowRigid = arrow.transform.GetComponent<Rigidbody>();
        Arrow arrowLogic = arrow.GetComponent<Arrow>();
        arrowLogic.Setup(lr);
        DrawLine();
        arrowRigid.AddForce((target.position - arrow.transform.position).normalized * arrowLogic.moveSpeed, ForceMode.VelocityChange);
    }

    private void DrawLine()
    {
        lr.positionCount=2;
        lr.SetPosition(0, firePos.position);
        lr.SetPosition(1, target.position+new Vector3(0,-1.0f,0));
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Manager.Instance.goblinSceneManager.hunterGoblinCount--;
            boxCollider.enabled = false;
            isDie = true;
            anim.SetTrigger("onDie");
            StartCoroutine(Destroy());
        }
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        Color goblinColor = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = goblinColor;
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
