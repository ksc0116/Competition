using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum OrcState {None=-1, Idle=0,Wander=1, Pursuit=2, Attack=3, Die = 4,First=5,Idle2=6}

public class OrcLogic : MonoBehaviour
{
    private Rigidbody rigid;

    public BoxCollider boxCollider;
    private SphereCollider attackCollider;

    public bool isReady;

    public OrcState orcState = OrcState.None;

    [Header("Status")]
    public int HP = 100;

    [Header("Pursuit")]
/*    private float targetRecognitionRange=3f;
    private float pursuitLimitRange = 4f;*/

    [Header("Attack")]
    private float attackRate = 1f;
    private int attackPower = 40;
    /*private float attackRange = 2f;*/
    private float attackRange = 12f;
    /*private float lastAttackTime = 0.0f;*/

    private NavMeshAgent navMeshAgent;
    public Transform target;

    private Animator anim;

    private bool isWalk;
    private bool isRun;
    public bool isDie;

    public float setDistance;

    private SkinnedMeshRenderer skinnedMeshRenderer;

    private OrcSpawner orcSpawner;
    private int createnumber;

    private Transform setPos;
    private void Awake()
    {
        attackCollider = GetComponent<SphereCollider>();
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        navMeshAgent=GetComponent<NavMeshAgent>();  
        anim = GetComponent<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    public IEnumerator First()
    {
        isWalk = false;
        isRun = true;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        float currentTime = 0;

        navMeshAgent.speed = 5f;

        navMeshAgent.SetDestination(setPos.position);

        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        transform.rotation = Quaternion.LookRotation(to - from);

        while (true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);
            from = new Vector3(transform.position.x, 0, transform.position.z);
            if ((to - from).magnitude < 0.1f)
            {
                orcSpawner.isReady[createnumber] = true;
                ChangeState(OrcState.Idle);
            }
            yield return null;
        }
    }

    private IEnumerator Idle()
    {
        isWalk = false;
        isRun = false;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);

        while (true)
        {
            if (isReady == true)
            {
                StartCoroutine(AutoWanderChange());
                isReady = false;
            }
            LookRotationToTarget();
            yield return null;
        }
    }

    private IEnumerator AutoWanderChange()
    {
        yield return new WaitForSeconds(2f);
        ChangeState(OrcState.Wander);
    }
    private IEnumerator Wander()
    {
        if(isDie==true) yield break;
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
            }

            CalculateDistanceToTargetAndSelectState();

            yield return null;
        }
    }

/*    private IEnumerator Pursuit()
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
    }*/

/*    private IEnumerator Attack()
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

            if(Time.time-lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                anim.SetTrigger("onAttack");
            }
            yield return null;  
        }
    }*/
    private  IEnumerator Attack()
    {
        isWalk = false;
        isRun=true;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 attackDir = to - from;
        navMeshAgent.ResetPath();
        LookRotationToTarget();

        yield return new WaitForSeconds(0.1f);

        rigid.AddForce(attackDir.normalized*15f,ForceMode.Impulse);
        yield return new WaitForSeconds(0.15f);
        anim.SetTrigger("onAttack");
        yield return new WaitForSeconds(0.7f);

        rigid.velocity = Vector3.zero;
        ChangeState(OrcState.Idle2);
    }
    private IEnumerator Idle2()
    {
        isWalk = false;
        isRun = false;
        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isRun", isRun);
        yield return new WaitForSeconds(2f);
        CalculateDistanceToTargetAndSelectState();
    }
    private IEnumerator Die()
    {
        rigid.velocity = Vector3.zero;
        Manager.Instance.orcSceneManager.orcCount--;
        anim.enabled = false;
        skinnedMeshRenderer.materials[0].color = Color.black;
        skinnedMeshRenderer.materials[1].color = Color.black;
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    public void Setup(Transform target,OrcSpawner orcSpawner,int createnumber,Transform setPos)
    {
        this.setPos = setPos;
        this.createnumber = createnumber;
        this.orcSpawner = orcSpawner;
        this.target = target;
    }

    public void ChangeState(OrcState newState)
    {
            if (orcState == newState) return;

            StopCoroutine(orcState.ToString());

            orcState = newState;

            StartCoroutine(orcState.ToString());
    }

    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        wanderJitter=Random.Range(wanderJitterMin, wanderJitterMax);

        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius,int angle)
    {
        Vector3 position = Vector3.zero;

        position.x=Mathf.Cos(angle)*radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x,0,target.position.z);
        Vector3 from =new Vector3(transform.position.x,0,transform.position.z);

        transform.rotation = Quaternion.LookRotation(to-from);
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        float distance=Vector3.Distance(target.position, transform.position);
        if (distance <= attackRange)
        {
            ChangeState(OrcState.Attack);
        }
        else if(distance > attackRange)
        {
            ChangeState(OrcState.Wander);
        }
/*        else if (distance <= pursuitLimitRange)
        {
            ChangeState(OrcState.Pursuit);
        }
        else if (distance >= targetRecognitionRange)
        {
            ChangeState(OrcState.Wander);
        }*/
    }

    public void TakeDamage(int damage)
    {
        if (isDie == true) return;
        HP -= damage;
        if (HP <= 0)
        {
            isDie = true;
            ChangeState(OrcState.Die);
        }
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        Color color1 = skinnedMeshRenderer.materials[0].color;
        Color color2 = skinnedMeshRenderer.materials[1].color;
        skinnedMeshRenderer.materials[0].color = Color.red;
        skinnedMeshRenderer.materials[1].color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.materials[0].color = color1;
        skinnedMeshRenderer.materials[1].color = color2;
    }
    public void AttackColliderOn()
    {
        StartCoroutine(ColliderOnOff());
    }
    private IEnumerator ColliderOnOff()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackCollider.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            target.gameObject.GetComponent<S_PlayerController>().TakeDamage(attackPower);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

/*        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);*/

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
