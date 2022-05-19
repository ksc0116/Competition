using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 5;

    public float moveSpeed = 50f;

    private bool isGrounded = false;

    private Vector3 tempPos;

    LineRenderer lr;

    public void Setup(LineRenderer lr)
    {
        this.lr = lr;
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
            Destroy(gameObject);
            other.gameObject.GetComponent<S_PlayerController>().TakeDamage(damage);
        }
        else if (other.gameObject.tag == "PlayerShield")
        {
            lr.positionCount = 0;
            Destroy(gameObject);
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}