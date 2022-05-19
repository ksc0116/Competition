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

    ArrowMemoryPool pool;

    public void SetUp(Transform target)
    {
        this.target = target;
    }

    private void Awake()
    {
        pool=GetComponent<ArrowMemoryPool>();
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

        // 바로 돌기
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
    // 애니메이션 이벤트에서 호출
    public void Shot()
    {
        pool.SpawnArrow(firePos.position);
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
