using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    MemoryPool meteorPool;
    MemoryPool pointPool;
    MemoryPool explosionPool;

    private int attackPower = 40;

    private Rigidbody rigid;


    private Transform target;
    Vector3 tempTarget;

    private GameObject tempPoint;

    bool isActive;
    private void Awake()
    {
        rigid=GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isActive==true)
        {
            transform.position = Vector3.MoveTowards(transform.position, tempTarget, 20f * Time.deltaTime);
        }
    }
    public void Setup(Transform target, MemoryPool meteorPool, MemoryPool pointPool, MemoryPool explosionPool)
    {
        isActive = true;
        this.explosionPool = explosionPool;
        this.meteorPool = meteorPool;
        this.pointPool = pointPool;
        this.target=target;
        tempTarget=target.position;
        ToTarget();
    }
    private void ToTarget()
    {
        isActive = true;
        Vector3 targetPosition = target.position + Vector3.up;
        Vector3 moveDir= targetPosition - transform.position;
        if (target != null)
        {
            tempPoint = pointPool.ActivePoolItem();
            tempPoint.transform.position = target.position + Vector3.up * 0.1f;
            tempPoint.transform.rotation = Quaternion.Euler(90, 0, 0);
            /*tempPoint = Instantiate(pointPrefab, target.position + Vector3.up * 0.1f, Quaternion.Euler(90, 0, 0));*/
        }
        /*rigid.AddForce(moveDir.normalized*25f,ForceMode.Impulse);*/
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isActive = false;
            meteorPool.DeactivePoolItem(gameObject);
            /*Destroy(gameObject);*/

            pointPool.DeactivePoolItem(tempPoint);
            /*Destroy(tempPoint);*/

            GameObject explosion = explosionPool.ActivePoolItem();
            explosion.transform.position = tempPoint.transform.position;
            explosion.transform.rotation = Quaternion.identity;
            /*Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);*/
        }
        else if(collision.gameObject.tag == "Player")
        {
            isActive = false;

            S_PlayerController logic=collision.gameObject.GetComponent<S_PlayerController>();
            logic.TakeDamage(attackPower);

            GameObject explosion = explosionPool.ActivePoolItem();
            explosion.transform.position = tempPoint.transform.position;
            explosion.transform.rotation = Quaternion.identity;
            /*Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);*/

            pointPool.DeactivePoolItem(tempPoint);
            /*Destroy(tempPoint);*/

            meteorPool.DeactivePoolItem(gameObject);
            /*Destroy(gameObject);*/
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerShield")
        {
            isActive = false;

            meteorPool.DeactivePoolItem(gameObject);

            pointPool.DeactivePoolItem(tempPoint);

            GameObject explosion = explosionPool.ActivePoolItem();
            explosion.transform.position = tempPoint.transform.position;
            explosion.transform.rotation = Quaternion.identity;

            /*Destroy(gameObject);
            Destroy(tempPoint);
            Instantiate(explosionPrefab, tempPoint.transform.position, Quaternion.identity);*/
        }
    }
}
