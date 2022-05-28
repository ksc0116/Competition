using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum GoblinState {  None=-1, Wander=0, Pursuit, Attack,Die }
public class GoblinFSM : MonoBehaviour
{
    [Header("HP바")]
    [SerializeField] Transform HpBar;
    Camera cam;
    Slider hpSlider;

    [SerializeField]
    private BoxCollider boxCollider;

    private bool isDie;

    [Header("Status")]
    [SerializeField]
    private float HP;
    float maxHP = 100;

    [Header("Pursuit")]
    private float targetRecognitionRange = 3f;
    private float pursuitLimitRange = 4f;

    [Header("Attack")]
    private float attackRange = 1.5f;
    [SerializeField]
    private float attackRate = 1f;
    private float lastAttackTime = 0;

    public GoblinState goblinState= GoblinState.None;

    private NavMeshAgent navMeshAgent;
    private Transform target;

    private Animator anim;

    private bool isWalk;
    private bool isRun;

    [SerializeField]
    private SkinnedMeshRenderer skinnedMeshRenderer;

    DamageTextMemoryPool damageTextPool;
    private void Awake()
    {
        hpSlider = GetComponentInChildren<Slider>(); 
        HpBar.gameObject.SetActive(false);
        cam = Camera.main;
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    public void SetUp(Transform target,DamageTextMemoryPool pool)
    {
        damageTextPool = pool;
        this.target = target;
    }

    private IEnumerator navMeshOn()
    {
        yield return new WaitForSeconds(0.2f);
        navMeshAgent.enabled = true;
        ChangeState(GoblinState.Wander);
    }

    private void OnEnable()
    {
        StartCoroutine(navMeshOn());
        HP = maxHP;
    }
    private void Update()
    {
        Quaternion q_hp=Quaternion.LookRotation(HpBar.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(HpBar.rotation, q_hp, 1000 ).eulerAngles;
        HpBar.rotation=Quaternion.Euler(hp_angle.x, hp_angle.y,0);
        hpSlider.value = HP / maxHP;
    }
    public void ChangeState(GoblinState newState)
    {
        if (goblinState == newState) return;

        StopCoroutine(goblinState.ToString());

        goblinState = newState;

        StartCoroutine(goblinState.ToString());
    }
    private  IEnumerator Die()
    {
        HpBar.gameObject.SetActive(false);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
    private IEnumerator Wander()
    {
        if (isDie == true) yield break;

        isWalk = true;
        isRun = false;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        navMeshAgent.speed = 1f;

        navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            if ((to - from).magnitude < 0.01f)
            {
                navMeshAgent.SetDestination(CalculateWanderPosition());
                ChangeState(GoblinState.Wander);
            }

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }
    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        return targetPosition;
    }
    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }
    private IEnumerator Pursuit()
    {
        if (isDie == true) yield break;

        isWalk = false;
        isRun = true;
        anim.SetBool("isRun", isRun);
        anim.SetBool("isWalk", isWalk);
        while (true)
        {
            navMeshAgent.speed = 2f;

            navMeshAgent.SetDestination(target.position);

            LookRotationToTarget();

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }
    private void LookRotationToTarget()
    {
        if(isDie == true) return;
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        // 바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);

        // 서서히 돌기
        /*Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);*/
    }
    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null || isDie==true) return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackRange)
        {
            ChangeState(GoblinState.Attack);
        }
        else if (distance <= pursuitLimitRange)
        {
            ChangeState(GoblinState.Pursuit);
        }
        else if (distance > pursuitLimitRange)
        {
            ChangeState(GoblinState.Wander);
        }
/*        else if(distance>=targetRecognitionRange)
        {
            ChangeState(GoblinState.Wander);
        }*/
    }

    private IEnumerator Attack()
    {
        if (isDie == true) yield break;

        isWalk = false;
        isRun = false;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        navMeshAgent.ResetPath();

        while (true)
        {
            LookRotationToTarget();

            CalculateDistanceToTargetAndSelectState();

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                if (isDie == false)
                {
                    anim.SetTrigger("onAttack");
                }
            }
            yield return null;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

/*        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);*/

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void TakeDamage(float damage)
    {
        if (isDie == true) return;
        HpBar.gameObject.SetActive(true);
        damageTextPool.SpawnDamageText(transform.position, damage);
        HP -= damage;
        if (HP <= 0)
        {
            navMeshAgent.ResetPath();
            Manager.Instance.goblinSceneManager.groundGoblinCount--;
            boxCollider.enabled = false;
            isDie = true;
            anim.SetTrigger("onDie");
            ChangeState(GoblinState.Die);
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
}