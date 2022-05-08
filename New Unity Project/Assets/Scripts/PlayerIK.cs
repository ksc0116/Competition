using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIK : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private Transform leftHand;
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (player.state == PlayerState.NonBattle)
        {
            return;
        }
        anim.SetIKPosition(AvatarIKGoal.LeftHand,leftHand.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

        anim.SetIKRotation(AvatarIKGoal.LeftHand,leftHand.rotation);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
    }
}
