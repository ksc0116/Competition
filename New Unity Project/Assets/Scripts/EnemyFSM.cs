using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {  None=-1,Idle=0,Wander,Pursuit,Attack,Run}
public class EnemyFSM : MonoBehaviour
{
    private Transform goal;

    [Header("Status")]
    [SerializeField]
    private int HP;

    [Header("Persuit")]
    [SerializeField]
    private float targetRecognitionRange = 8;
    [SerializeField]
    private float pursuitLimitRange = 10;

    [Header("Attack")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float attackRange = 5;
    [SerializeField]
    private float attackRate = 1;
    private float lastAttackTime = 0;

    public EnemyState enemyState=EnemyState.None;

    private NavMeshAgent navMeshAgent;
    private Transform target;

    private MemoryPool enemymemoryPool;

    public bool isMove;
    [SerializeField]
    private EnemyAnimator enemyAnim;

    private int wanderCount = 0;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void Setup(Transform target,MemoryPool enemyMemoryPool,Transform goal)
    {
        navMeshAgent.updateRotation = false;

        this.goal = goal;
        this.target = target;
        this.enemymemoryPool = enemyMemoryPool;
    }

    private void OnEnable()
    {
        wanderCount++;

        StartCoroutine(navMeshOn());
        isMove = false;
        enemyAnim.WalkAnim(isMove);
        HP = 100;
        /*ChangeState(EnemyState.Idle);*/
    }
    private IEnumerator navMeshOn()
    {
        yield return new WaitForSeconds(0.2f);
        navMeshAgent.enabled = true;
        ChangeState(EnemyState.Wander);
    }
    private void OnDisable()
    {
        wanderCount = 0;

        navMeshAgent.enabled = false;

        StopCoroutine(enemyState.ToString());

        enemyState=EnemyState.None;
    }

    public void ChangeState(EnemyState newState)
    {
        if (enemyState == newState) return;

        StopCoroutine(enemyState.ToString());

        enemyState = newState;

        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        isMove = false;
        enemyAnim.WalkAnim(isMove);

        wanderCount++;

        StartCoroutine("AutoChangeFromIdleTowWander");

        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleTowWander()
    {
        int changeTime =10;

        yield return new WaitForSeconds(changeTime);
        isMove = true;
        enemyAnim.WalkAnim(isMove);
        ChangeState(EnemyState.Wander);
    }
    private IEnumerator Wander()
    {
        isMove = true;
        enemyAnim.WalkAnim(isMove);

        float currentTime = 0;
        float maxTime = 0;

        if (wanderCount == 1)
        {
            maxTime = 3f;
            navMeshAgent.speed = 8f;
        }
        else if (wanderCount == 2)
        {
            maxTime = 8f;
            navMeshAgent.speed = 1f;
        }
       

        navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to  =new Vector3(navMeshAgent.destination.x,0,navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x,0,transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if((to-from).magnitude<0.01f || currentTime >= maxTime)
            {
                if (wanderCount == 1)
                {
                    ChangeState(EnemyState.Idle);
                }
                else if(wanderCount == 2)
                {
                    ChangeState(EnemyState.Run);
                }
            }

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Run()
    {
        isMove = true;
        enemyAnim.WalkAnim(isMove);
        navMeshAgent.updateRotation = true;
        while (true)
        {
            navMeshAgent.speed = 1;

            navMeshAgent.SetDestination(goal.position);

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

/*        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale=Vector3.one*100.0f;*/

        wanderJitter=Random.Range(wanderJitterMin,wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

/*        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);*/

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
        isMove = true;
        enemyAnim.WalkAnim(isMove);
        while (true)
        {
            navMeshAgent.speed = 2;

            navMeshAgent.SetDestination(target.position);

            LookRotationToTarget();

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }
    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);

        Vector3 from=new Vector3(transform.position.x,0, transform.position.z);

        // 바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);

        // 서서히 돌기
        /*Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);*/
    }
    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position,transform.position);
        if (distance <= attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance <= targetRecognitionRange)
        {
            ChangeState(EnemyState.Pursuit);
        }
        else if (distance >= pursuitLimitRange)
        {
            if(enemyState == EnemyState.Pursuit)    
                    ChangeState(EnemyState.Pursuit);
            else if(enemyState == EnemyState.Run)
                ChangeState(EnemyState.Run);
        }
    }

    private IEnumerator Attack()
    {
        isMove = false;
        enemyAnim.WalkAnim(isMove);
        navMeshAgent.ResetPath();

        while (true)
        {
            LookRotationToTarget();

            CalculateDistanceToTargetAndSelectState();

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;

                GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
                Rigidbody rigid=clone.GetComponent<Rigidbody>();
                Vector3 dir=target.position-transform.position;
                rigid.AddForce(dir.normalized*10f,ForceMode.Impulse);
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

        Gizmos.color = new Color(0.39f,0.04f,0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public void TakeDamage(int damage)
    {
        HP-=damage;
        if (HP <= 0)
        {
            enemymemoryPool.DeactivePoolItem(gameObject);
        }
    }
}
