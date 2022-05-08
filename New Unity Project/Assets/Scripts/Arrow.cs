using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private int damage = 5;

    public float moveSpeed = 50f;

    private bool isGrounded = false;

    private Vector3 tempPos;

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
            Destroy(gameObject);
            other.gameObject.GetComponent<S_PlayerController>().TakeDamage(damage);
        }
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}