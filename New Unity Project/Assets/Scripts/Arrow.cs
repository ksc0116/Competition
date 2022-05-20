using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 5;

    private float moveSpeed = 15f;
    public float MoveSpeed {  get { return moveSpeed; }  }

    private bool isGrounded = false;

    private Vector3 tempPos;

    LineRenderer lr;

    MemoryPool memoryPool;


    Transform target;
    public void Setup(MemoryPool pool,LineRenderer lr)
    {
        moveSpeed = 15f;
        this.lr = lr;
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
            lr.positionCount = 0;
            isGrounded = true;
            tempPos = transform.position;
            moveSpeed = 0.0f;
            StartCoroutine(Destroy());
        }
        else if (other.gameObject.tag == "Player")
        {
            lr.positionCount = 0;
            memoryPool.DeactivePoolItem(this.gameObject);
            other.gameObject.GetComponent<S_PlayerController>().TakeDamage(damage);
        }
        else if (other.gameObject.tag == "PlayerShield")
        {
            lr.positionCount = 0;
            memoryPool.DeactivePoolItem(this.gameObject);
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        memoryPool.DeactivePoolItem(this.gameObject);
    }
}