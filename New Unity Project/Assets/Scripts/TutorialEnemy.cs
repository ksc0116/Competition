using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnemy : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
