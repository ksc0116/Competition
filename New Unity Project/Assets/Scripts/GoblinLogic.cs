using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinLogic : MonoBehaviour
{
    [Header("HP바")]
    [SerializeField] Transform HpBar;
    Camera cam;
    Slider hpSlider;

    [SerializeField]
    private BoxCollider boxCollider;
    [SerializeField]
    private float HP;
    float maxHP = 100f;

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
        hpSlider=GetComponentInChildren<Slider>();  
        HpBar.gameObject.SetActive(false);
        HP = maxHP;
        cam = Camera.main;
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
        Quaternion q_hp = Quaternion.LookRotation(HpBar.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(HpBar.rotation, q_hp, 1000).eulerAngles;
        HpBar.rotation = Quaternion.Euler(0, hp_angle.y, 0);
        hpSlider.value = HP / maxHP;
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
            attackRate = Random.Range(4, 8);
            lastAttackTime = Time.time;

            anim.SetTrigger("onAttack");
        }
    }

    // 애니메이션 이벤트에서 호출
    public void Shot()
    {
        pool.SpawnArrow(firePos.position,target);
    }

    public void TakeDamage(float damage)
    {
        HpBar.gameObject.SetActive(true);
        HP -= damage;
        if (HP <= 0)
        {
            HpBar.gameObject.SetActive(false);
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
