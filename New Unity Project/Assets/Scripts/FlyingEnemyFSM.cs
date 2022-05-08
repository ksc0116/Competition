using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FlyEnemyState {  None=-1,Idle=0,Wander,Run}

public class FlyingEnemyFSM : MonoBehaviour
{
    private Transform goal;

    [Header("Status")]
    [SerializeField]
    private float HP;



    public FlyEnemyState enemyState=FlyEnemyState.None;

    private NavMeshAgent navMeshAgent;
    private Transform target;

    private MemoryPool flyEnemyMemoryPool;

    private int wanderCount = 0;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void Setup(Transform target, MemoryPool flyEnemymemoryPool,Transform goal)
    {
        navMeshAgent.updateRotation = false;

        this.goal = goal;
        this.target = target;
        this.flyEnemyMemoryPool = flyEnemymemoryPool;
    }

    private void OnEnable()
    {
        wanderCount++;
        StartCoroutine(navMeshOn());
        HP = 100;
    }
    private IEnumerator navMeshOn()
    {
        yield return new WaitForSeconds(0.2f);
        navMeshAgent.enabled = true;
        ChangeState(FlyEnemyState.Wander);
    }
    private void OnDisable()
    {
        wanderCount = 0;

        navMeshAgent.enabled=false;

        StopCoroutine(enemyState.ToString());

        enemyState = FlyEnemyState.None;
    }

    public void ChangeState(FlyEnemyState newState)
    {
        if (enemyState == newState) return;

        StopCoroutine(enemyState.ToString());

        enemyState = newState;

        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        wanderCount++;

        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            yield return null;
        }
    }
    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = 10;

        yield return new WaitForSeconds(changeTime);
        ChangeState(FlyEnemyState.Wander);
    }

    private IEnumerator Wander()
    {
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

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x,0,transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            if((to-from).magnitude < 0.01f || currentTime >= maxTime)
            {
                if (wanderCount == 1)
                {
                    ChangeState(FlyEnemyState.Idle);
                }
                else if (wanderCount == 2)
                {
                    ChangeState(FlyEnemyState.Run);
                }
            }

            yield return null;
        }
    }

    private IEnumerator Run()
    {
        navMeshAgent.updateRotation = true;
        while (true)
        {
            navMeshAgent.speed = 5f;

            navMeshAgent.SetDestination(goal.position);


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
    private Vector3 SetAngle(float radius,int angle)
    {
        Vector3 position = Vector3.zero;

        position.x=Mathf.Cos(angle)*radius;
        position.y=Mathf.Sin(angle)*radius;

        return position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            flyEnemyMemoryPool.DeactivePoolItem(gameObject);
        }
    }
}
