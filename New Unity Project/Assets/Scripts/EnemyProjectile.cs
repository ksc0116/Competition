using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private MovementTransform movement;
    private float projectileDistance = 30;

    public void Setup(Vector3 position)
    {
        movement = GetComponent<MovementTransform>();

        StartCoroutine("OnMove", position);
    }

    private IEnumerator Onmove(Vector3 targetPosition)
    {
        Vector3 start = transform.position;

        movement.MoveTo((targetPosition - start).normalized);

        while (true)
        {
            if(Vector3.Distance(transform.position,start)>=projectileDistance)
            {
                Destroy(gameObject);

                yield break;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player Hit");

            Destroy(gameObject);
        }
    }
}
