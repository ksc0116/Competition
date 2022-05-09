using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GoblinState {  None=-1, Wander=0, Pursuit, Attack,Die }
public class GoblinFSM : MonoBehaviour
{
    [SerializeField]
    private BoxCollider boxCollider;

    private bool isDie;

    [Header("Status")]
    [SerializeField]
    private int HP;

    [Header("Persuit")]
    private float targetRecognitionRange = 3f;
    private float pursuitLimitRange = 4f;

    [Header("Attack")]
    private float attackRange = 1f;
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
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    public void SetUp(Transform target)
    {
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
        HP = 100;
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

        float currentTime = 0;

        navMeshAgent.speed = 1f;

        navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

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
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackRange && isDie==false)
        {
            ChangeState(GoblinState.Attack);
        }
        else if (distance <= pursuitLimitRange && isDie == false)
        {
            ChangeState(GoblinState.Pursuit);
        }
        else if(distance>=targetRecognitionRange && isDie == false)
        {
            ChangeState(GoblinState.Wander);
        }
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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void TakeDamage(int damage)
    {
        if (isDie == true) return;
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