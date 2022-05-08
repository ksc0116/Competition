using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightHandSword : MonoBehaviour
{
    private int attackPower = 50;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Orc")
        {
            OrcLogic logic=other.gameObject.GetComponent<OrcLogic>();
            logic.TakeDamage(attackPower);
        }
    }
}
