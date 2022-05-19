using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCollider : MonoBehaviour
{
    private int attackPower = 15;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            S_PlayerController logic=other.GetComponent<S_PlayerController>();
            logic.TakeDamage(attackPower);
        }
    }
}
