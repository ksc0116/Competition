using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 5;

    private float moveSpeed = 25f;
    public float MoveSpeed {  get { return moveSpeed; }  }

    public bool isGrounded = false;

    private Vector3 tempPos;

    MemoryPool memoryPool;

    Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
    public void Setup(MemoryPool pool)
    {
        moveSpeed = 25f;
        memoryPool = pool;
    }
    private void Update()
    {
        if (isGrounded == true)
        {
            transform.position = tempPos;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            isGrounded = true;
            tempPos = transform.position;
            moveSpeed = 0.0f;
            StartCoroutine(Destroy());
        }
        else if (other.gameObject.tag == "Player")
        {
            memoryPool.DeactivePoolItem(this.gameObject);
            other.gameObject.GetComponent<S_PlayerController>().TakeDamage(damage);
        }
        else if (other.gameObject.tag == "PlayerShield")
        {
            memoryPool.DeactivePoolItem(this.gameObject);
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        isGrounded = false;
        memoryPool.DeactivePoolItem(this.gameObject);
    }
}