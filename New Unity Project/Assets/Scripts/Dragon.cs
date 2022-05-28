using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BossState { None=-1,Set1=0,Set2=1,Idle=2,Attack=3,Idle2=4}

public class Dragon : MonoBehaviour
{
    [SerializeField]
    DamageTextMemoryPool m_pool;
    [SerializeField]
    GameObject dragonUI;
    [Header("HP¹Ù")]
    [SerializeField] Transform HpBar;
    Camera cam;
    [SerializeField] Slider hpSlider;

    [SerializeField]
    AudioSource sfxAudio;

    MeteorMemoryPool meteorMemoryPool;

    private SkinnedMeshRenderer skinnedMeshRenderer;

   public float MaxHP = 300;
   public float HP = 0;

    [SerializeField]
    private Transform target;

    private float attackRate = 5f;
    private float lastAttackTime = 0;
    private bool isAttack = false;

    private float fireRange = 36f;
    private float meleeRange = 20f;

    [SerializeField]
    private CapsuleCollider attackCollider;

    public BossState bossState=BossState.None;

    [SerializeField]
    private Transform[] setPos;

    private Animator anim;

    [Header("Bool")]
    public bool isTakeOff;
    public bool isFlyIdle;
    public bool isLand;
    public bool isReady=false;
    public bool isDie = false;
    public bool isHalf = false;
    public bool isAttackAble = false;

    [SerializeField]
    private GameObject flameParticle;
    [SerializeField]
    private GameObject firePartcle;
    [SerializeField]
    private GameObject fireZone;
    [SerializeField]
    private SphereCollider fireCollider;

    private int attackPower = 15;

    [SerializeField]
    GameObject meteorPrefab;
    [SerializeField]
    Transform meteorSpanwPos;
    private void Awake()
    {
        HpBar.gameObject.SetActive(false);
        cam = Camera.main;

        meteorMemoryPool =GetComponent<MeteorMemoryPool>();
        HP = MaxHP;
        skinnedMeshRenderer=GetComponentInChildren<SkinnedMeshRenderer>();  
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        CheckHPHalf();
        if (isAttack == false && isReady==true)
        {
            LookRotationToTarget();
            ChangeState(BossState.Idle2);
        }
        if (isReady == true)
        {
            Attack();
        }
        Quaternion q_hp = Quaternion.LookRotation(HpBar.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(HpBar.rotation, q_hp, 1000).eulerAngles;
        HpBar.rotation = Quaternion.Euler(hp_angle.x, hp_angle.y, 0);
        hpSlider.value = HP / MaxHP;
    }
    private IEnumerator Idle2()
    {
        while (true)
        {
            yield return null;
        }
    }
    private void CheckHPHalf()
    {
        float tempHP= HP / MaxHP;
        if (tempHP <= 0.5f)
        {
            isHalf = true;
        }
    }
    public IEnumerator ScreamAni()
    {
        anim.SetTrigger("onScream");
        yield return new WaitForSeconds(1f);
        Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.dragonScream, sfxAudio.volume);
        yield return new WaitForSeconds(3.3f);
        ChangeState(BossState.Set1);
    }
    public void ChangeState(BossState newState)
    {
        if (bossState == newState) return;

        StopCoroutine(bossState.ToString());
        bossState = newState;
        StartCoroutine(bossState.ToString());
    }
    private IEnumerator Set1()
    {
        isTakeOff = true;
        isLand = false;
        isFlyIdle = false;
        anim.SetBool("isTakeOff", isTakeOff);
        anim.SetBool("isLand", isLand);
        anim.SetBool("isFlyIdle", isFlyIdle);
        float currenTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 300.0f;

            transform.position=Vector3.Lerp(transform.position,setPos[0].position,percent);

            if (currenTime >= 2.8f)
            {
                ChangeState(BossState.Set2);
            }

            yield return null;
        }
    }
    private IEnumerator Set2()
    {
        if(isDie==true) yield break;
        isTakeOff = false;
        isLand = false;
        isFlyIdle = true;
        anim.SetBool("isTakeOff", isTakeOff);
        anim.SetBool("isLand", isLand);
        anim.SetBool("isFlyIdle", isFlyIdle);
        float currenTime = 0.0f;
        float percent = 0.0f;
        while (percent < 1)
        {
            currenTime += Time.deltaTime;
            percent = currenTime / 150.0f;

            transform.position = Vector3.Lerp(transform.position, setPos[1].position, percent);

            if ((transform.position-setPos[1].position).magnitude<=0.01f)
            {
                isLand = true;
                anim.SetBool("isLand",isLand);
                ChangeState(BossState.Idle);
            }

            yield return null;
        }
    }
    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(3f);
        isLand = false;
        isReady = true;
        anim.SetBool("isLand", isLand);
        ResetLastAttackTime();
    }
    public void ResetLastAttackTime()
    {
        lastAttackTime = Time.time;
    }
    private void Attack()
    {
        if (isDie == true)  return;

        if (Time.time - lastAttackTime > attackRate)
        {
               ResetLastAttackTime();
               isAttackAble = true;
            
               float distance = Vector3.Distance(transform.position, target.position);
               if (distance>meleeRange && isHalf==false)
               {
                   anim.SetTrigger("onFlame");
               }
               else if (distance > meleeRange  && isHalf == true)
               {
                     anim.SetTrigger("onMeteor");
               }   
               else if (distance <= meleeRange )
               {
                   int patternNumber = Random.Range(0, 5);
                   switch (patternNumber)
                   {
                      case 0:
                      case 1:
                           anim.SetTrigger("onMouth");
                           break;
                      case 2:
                      case 3:
                      case 4:
                          StartCoroutine(FireZone());
                          break;
                  }
               }
        }

    }

    private void LookRotationToTarget()
    {
        if (isDie == true) return;
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.05f);
    }

    public void AttackCollider()
    {
        StartCoroutine(ColliderOnOff());
    }

    public void AttackOn()
    {
        isAttack = true;
    }
    public void AttackOff()
    {
        isAttack=false;
    }

    public void flameOn()
    {
        flameParticle.SetActive(true);
    }
    public void flameOff()
    {
        flameParticle.SetActive(false);
    }

    public void MakeMeteor()
    {
        StartCoroutine(MeteorShot());
    }

    private IEnumerator MeteorShot()
    {
        for(int i = 0; i < 8; i++)
        {
            /*GameObject meteor = Instantiate(meteorPrefab, meteorSpanwPos.position, Quaternion.identity);*/
            GameObject meteor = meteorMemoryPool.SpawnMeteor();
            meteor.transform.position = meteorSpanwPos.position;
            meteor.transform.rotation = Quaternion.identity;
            yield return new WaitForSeconds(0.25f);
        }
    }

    private IEnumerator FireOnOff()
    {
        yield return new WaitForSeconds(0.5f);
        fireCollider.enabled = true;
        firePartcle.SetActive(true);
        yield return new WaitForSeconds(2.8f);
        fireCollider.enabled = false;
        firePartcle.SetActive(false);
    }

    private IEnumerator ColliderOnOff()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackCollider.enabled = false;
    }

    private IEnumerator FireZone()
    {
        fireZone.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        fireZone.SetActive(false);
        anim.SetTrigger("onScream");
        StartCoroutine(FireOnOff());
    }
    private void Die()
    {
        anim.SetTrigger("onDie");
    }
    public void TakeDamage(float damage)
    {
        if (isDie == true) return;
        dragonUI.SetActive(true);
        dragonUI.GetComponent<DragonUI>().ChangeFillArea(damage);
        HP -=damage;
        m_pool.SpawnDamageText(transform.position + new Vector3(0, 1, 0), damage);
        HpBar.gameObject.SetActive(true);
        if (HP <= 0)
        {
            HpBar.gameObject.SetActive(false);
            isDie = true;
            dragonUI.SetActive(false);
            Die();
        }
        StartCoroutine(ChangeColor());
    }
    private IEnumerator ChangeColor()
    {
        Color color = skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = color;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}