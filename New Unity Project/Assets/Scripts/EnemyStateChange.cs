/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, Run }

public class EnemyStateChange : MonoBehaviour
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

    public EnemyState enemyState = EnemyState.None;

    private NavMeshAgent navMeshAgent;
    private Transform target;

    private MemoryPool enemymemoryPool;

    public bool isMove;
    [SerializeField]
    private EnemyAnimator enemyAnim;


    public void Setup(Transform target,MemoryPool enemyMemoryPool,Transform goal)
    {
        navMeshAgent=GetComponent<NavMeshAgent>();

        navMeshAgent.updateRotation = false;

        this.goal = goal;
        this.target = target;
        this.enemymemoryPool = enemyMemoryPool;
        isMove = false;
    }

    private void OnEnable()
    {
        isMove = false;
        enemyAnim.WalkAnim(isMove);
        HP = 100;
        ChangeState(EnemyState.Idle);
    }
    private void OnDisable()
    {
        StopCoroutine(enemyState.ToString());
        enemyState=EnemyState.None;
    }

    public void ChangeState(EnemyState newState)
    {
        if (newState == enemyState) return;

        StopCoroutine(enemyState.ToString());

        enemyState = newState;

        StartCoroutine(enemyState.ToString());
    }

    private IEnumerator Idle()
    {
        isMove = false;
        enemyAnim.WalkAnim(isMove);

        StartCoroutine("AutoChangeFromIdleToWander");

        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = 5;

        yield return new WaitForSeconds(changeTime);
        isMove = true;
        enemyAnim.WalkAnim(isMove);

        ChangeState(EnemyState.Wander);

        float currentTime = 0;
        float maxTime = 10;

        navMeshAgent.speed = 1f;

        navMeshAgent.SetDestination(CalculateWanderPosition());

        Vector3 to = new Vector3(navMeshAgent.destination.x,0,navMeshAgent.destination.z);
        Vector3 from= new Vector3(transform.position.x,0,transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);

            if((to - from).sqrMagnitude<0.01f || currentTime >= maxTime)
            {
                ChangeState(EnemyState.Run);
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
            navMeshAgent.speed = 1f;

            navMeshAgent.SetDestination(goal.position);

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }
}
*/