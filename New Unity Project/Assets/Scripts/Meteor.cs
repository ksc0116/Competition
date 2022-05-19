using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private int attackPower = 40;

    private Rigidbody rigid;

    [SerializeField]
    private GameObject pointPrefab;

    private Transform target;

    [SerializeField]
    private GameObject explosionPrefab;

    private GameObject tempPoint;
    private void Awake()
    {
        rigid=GetComponent<Rigidbody>();
    }
    public void Setup(Transform target)
    {
        this.target=target;
        ToTarget();
    }
    private void ToTarget()
    {
        Vector3 targetPosition = target.position + Vector3.up;
        Vector3 moveDir= targetPosition - transform.position;
        if (target != null)
        {
            tempPoint=Instantiate(pointPrefab, target.position + Vector3.up * 0.1f, Quaternion.Euler(90,0,0));
        }
        rigid.AddForce(moveDir.normalized*25f,ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
            Destroy(tempPoint);
            Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);
        }
        else if(collision.gameObject.tag == "Player")
        {
            S_PlayerController logic=collision.gameObject.GetComponent<S_PlayerController>();
            logic.TakeDamage(attackPower);
            Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);
            Destroy(tempPoint);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerShield")
        {
            Destroy(gameObject);
            Destroy(tempPoint);
            Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);
        }
    }
}
