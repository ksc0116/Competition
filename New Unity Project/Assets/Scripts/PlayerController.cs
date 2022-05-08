using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { NonBattle=0, Battle=1,AimMode,Reload,};
public class PlayerController : MonoBehaviour
{
    public bool isTutorial;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private int maxShotCount = 10;
    public int MaxShotCount { get { return maxShotCount; }  }
    [SerializeField]
    private int curShotCount = 0;
    public int CurShotCount { get { return curShotCount; } set { curShotCount = value; } }

    [SerializeField]
    private GameObject battleWeapon;

    [SerializeField]
    private Transform playerBody;
    [SerializeField]
    private Transform cameraArm;
    [SerializeField]
    public float moveSpeed = 5f;
    [SerializeField]
    public PlayerAnimatorController player;
    [SerializeField]
    private Transform playerOrigin;
    [SerializeField]
    private GameObject dashEffect;

    private Rigidbody rigid;

    
    [SerializeField]
    private bool isBattleMode = false;
    public bool IsBattleMode { get { return isBattleMode; }  }

    public float current = 0.0f;
    public float maxWait =10.0f;

    public PlayerState state = PlayerState.NonBattle;

    public int HPMax = 100;
    public int HP=0;

    public bool isDash = false;
    public bool isCallBack = false;
    public bool isBorder=false;

    /*private float rayDistance;*/

    public bool isNonBattleMove = false;
    public bool isBattleMove = false;
    public bool isAimMove = false;
    private bool isMove = false;

    [Header("SpawnPosition")]
    [SerializeField]
    private Transform tutorialPosition;
    [SerializeField]
    private Transform mainMapPlayerSpawnPosition;

    [SerializeField]
    private GameObject enemyMemoryPool;

    private void Awake()
    {
        transform.position = tutorialPosition.position;
        HP = HPMax;
        curShotCount = maxShotCount;
        rigid =GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        WeaponChange();
        CheckBattleMode();
        LookForward();
        Dash();
        OnBattleMode();
        Reload();
        CheckCollider();
    }

    private void Move()
    {
        if (isCallBack == true) return;
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector2 moveInput = new Vector2(hAxis,vAxis );

        if (state == PlayerState.NonBattle)
        {
            isNonBattleMove = moveInput.magnitude != 0;
            isMove = isNonBattleMove;
            isBattleMove = false;
            isAimMove = false;
        }
        if(state == PlayerState.Battle)
        {
            isNonBattleMove = false;
            isBattleMove = moveInput.magnitude != 0;
            isMove = isBattleMove;
            isAimMove = false;
        }
        if (state == PlayerState.AimMode)
        {
            isNonBattleMove = false;
            isBattleMove = false;
            isAimMove = moveInput.magnitude != 0; 
            isMove = isAimMove;
        }

        player.SelectBoolMove(isNonBattleMove,isBattleMove,isAimMove);

        if (state == PlayerState.Battle)
        {
            player.BattleMove(isBattleMove, hAxis, vAxis);

        }
        if (state == PlayerState.NonBattle)
        {
            player.NonBattleMove(isNonBattleMove);
        }
        if (state == PlayerState.AimMode)
        {
            player.AimMove(isAimMove);
        }


        if (isMove)
        {
            Vector3 lookForward=new Vector3(cameraArm.forward.x,0,cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0, cameraArm.right.z).normalized;
            Vector3 moveDir=lookForward*moveInput.y + lookRight*moveInput.x;


            if (state == PlayerState.NonBattle)
            {
                // 캐릭터가 움직이는 방향 바라보게 하기
                playerBody.forward = moveDir;
                rigid.MovePosition(rigid.position+moveDir * moveSpeed * Time.deltaTime);
            }
            else
            {
                rigid.MovePosition(rigid.position + moveDir * (moveSpeed * Input.GetAxis("Vertical") >= 0 ? 5f : 3f) * Time.deltaTime);
            }
        }
    }
    private void LookForward()
    {
        if (state == PlayerState.Battle )
        {
            playerBody.rotation = Quaternion.Euler(0, cameraArm.eulerAngles.y, 0);
        }
        else if (state == PlayerState.AimMode)
        {
            playerBody.rotation = Quaternion.Euler(cameraArm.eulerAngles.x, cameraArm.eulerAngles.y,0);
        }
    }
    private void CheckBattleMode()
    {
        if(state == PlayerState.Battle)
        {
            current += Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                current = 0.0f;
                return;
            }
            if(current > maxWait)
            {
                state = PlayerState.NonBattle;
                current = 0.0f;
            }
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
        switch (state)
        {
            case PlayerState.Battle:
                state = PlayerState.Battle;
                break;
            case PlayerState.AimMode:
                state = PlayerState.Battle;
                break;
            case PlayerState.NonBattle:
                state = PlayerState.NonBattle;
                break ;
        }
        
        isDash = true;
        dashEffect.SetActive(true);
        moveSpeed = 20f;
        yield return new WaitForSeconds(0.3f);
        dashEffect.SetActive(false);
        moveSpeed = 5f;
        isDash = false;
    }

    private void WeaponChange()
    {
        switch (state)
        {
            case PlayerState.NonBattle:
                battleWeapon.SetActive(false);
                break;
            case PlayerState.Battle:
                battleWeapon.SetActive(true);
                break;
            case PlayerState.AimMode:
                battleWeapon.SetActive(true);
                break ;
        }
    }
    private void OnBattleMode()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (state == PlayerState.Battle)
            {
                state=PlayerState.NonBattle;
            }
            else if(state == PlayerState.NonBattle)
            {
                state = PlayerState.Battle;
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (state == PlayerState.Reload || isDash==true || isCallBack==true) return;
            battleWeapon.transform.localScale = Vector3.one * 0.7f;
            state = PlayerState.AimMode;
            current = 0;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            if(state == PlayerState.Reload) return;
            state=PlayerState.Battle;
        }
    }

    public void Reload()
    {
        if (curShotCount == maxShotCount || state==PlayerState.NonBattle || isCallBack ==true) return;
        if (curShotCount == 0 || Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine("Reloading");
        }
    }
    private IEnumerator Reloading()
    {
        state = PlayerState.Reload;
        player.ReloadAni();
        yield return new WaitForSeconds(1.0f);
        curShotCount = maxShotCount;
        state = PlayerState.Battle;
    }

    private void CheckCollider()
    {
        if (isDash == false) return;
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(hAxis,0,vAxis);
        Vector3 position=transform.position+new Vector3(0,0.5f,0);
        RaycastHit hit;
        if (state == PlayerState.NonBattle)
        {
            Debug.DrawRay(position, playerBody.forward*1f, Color.green);
            if (Physics.Raycast(position, playerBody.forward, out hit,1f, layerMask))
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
        else
        {
            Debug.DrawRay(position, moveDirection.normalized*1f, Color.green);
            if (Physics.Raycast(position, moveDirection.normalized, out hit,1f, layerMask))
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
    }

    public void TakeDamage(int damage)
    {
        state = PlayerState.Battle;
        HP-=damage;
        Debug.Log(HP-damage);
        if (HP <= 0)
        {
            Debug.Log("GameOver");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Portal")
        {
            transform.position = mainMapPlayerSpawnPosition.position;
            enemyMemoryPool.SetActive(true);
        }
    }
}
