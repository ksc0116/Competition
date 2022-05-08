using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator anim;


    public void WalkAnim(bool isMove)
    {
        anim.SetBool("isMove",isMove);
    }
}
