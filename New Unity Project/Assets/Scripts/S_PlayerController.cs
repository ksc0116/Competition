using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform center;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public S_PlayerAnimatorController playerAnimator;
    [SerializeField]
    private Transform playerOrigin;
    [SerializeField]
    private GameObject dashEffect;

    private Rigidbody rigid;

    public float HP = 0;
    public float maxHP = 100;

    public bool isDash = false;
    public bool isCallBack = false;
    public bool isBorder = false;
    public bool isFire = true;
    public bool isDownSkill = false;

    [Header("SpawnPosition")]
    [SerializeField]
    private Transform tutorialPosition;
    [SerializeField]
    private Transform goblinScenePlayerSpawnPoint;


    private bool isMove=false;

    [SerializeField]
    private GoblinSpawner goblinSpawner;


    private void Awake()
    {
        transform.position = tutorialPosition.position;
        HP = maxHP;
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Dash();
        CheckCollider();
        Attack();
    }

    private void Move()
    {
        if (isCallBack == true) return;

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector2 moveInput=new Vector2(hAxis, vAxis);

        Vector3 lookForward = new Vector3(cameraArm.forward.x, 0, cameraArm.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraArm.right.x, 0, cameraArm.right.z).normalized;
        Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

        isMove = moveInput == Vector2.zero? false: true;

        playerAnimator.MoveAni(isMove, hAxis, vAxis);

        playerBody.rotation = Quaternion.Euler(0,cameraArm.eulerAngles.y, 0);

        SelMoveSpeed();

        rigid.MovePosition(rigid.position + moveDir.normalized * moveSpeed  * Time.deltaTime);
    }

    private void SelMoveSpeed()
    {
        if (playerAnimator.isAttack == true || isDash==true) return;

        if (Input.GetAxis("Vertical") >= 0)
        {
            moveSpeed = 5f;
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            moveSpeed = 3f;
        }
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine("DashStart");
        }
    }
    private IEnumerator DashStart()
    {
        isDash = true;
        dashEffect.SetActive(true);
        moveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        dashEffect.SetActive(false);
        moveSpeed = 5f;
        isDash = false;
    }

    private void CheckCollider()
    {
        if (isDash == false) return;
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(hAxis, 0, vAxis);
        Vector3 position = transform.position + new Vector3(0, 0.5f, 0);
        RaycastHit hit;

        Debug.DrawRay(position, moveDirection.normalized * 1f, Color.green);
        if (Physics.Raycast(position, moveDirection.normalized, out hit, 1f, layerMask))
        {
            isBorder = true;
            Debug.Log(hit.transform.name);
            moveSpeed = 0f;
        }
        else
        {
            isBorder = false;
            if (isDash == false)
            {
                moveSpeed = 5f;
            }
        }
    }
    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerAnimator.AttackAni();
        }
    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
        Debug.Log(HP);
        if (HP <= 0)
        {
            Debug.Log("GameOver");
        }
    }
    public IEnumerator TakeDotDamage(int damage)
    {
        if (isFire == true)
        {
            HP -= damage;
            isFire = !isFire;
            yield return new WaitForSeconds(0.5f);
            isFire = !isFire;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            transform.position = goblinScenePlayerSpawnPoint.position;
            //enemyMemoryPool.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "GoblinPathEnd")
        {
            StartCoroutine(goblinSpawner.SpawnHunter());
        }
    }
}
