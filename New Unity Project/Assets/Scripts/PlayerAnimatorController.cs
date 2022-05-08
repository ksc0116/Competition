using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void NonBattleMove(bool isMove)
    {
        //anim.SetLayerWeight(1, 0.0f);
        LayerWeight(1, 0.0f);
        //anim.SetLayerWeight(2, 0.0f);
        LayerWeight(2, 0.0f);
        /*LayerWeight(3, 0.0f);*/
        anim.SetBool("isNonBattleMove", isMove);
    }
    public void BattleMove(bool isMove, float hAxis, float vAxis)
    {
        //anim.SetLayerWeight(1, 1f);
        LayerWeight(1, 1f);
        LayerWeight(2, 0f);
        /*LayerWeight(3, 0f);*/
        anim.SetBool("isBattleMove", isMove);
        anim.SetFloat("horizontal", hAxis);
        anim.SetFloat("vertical", vAxis);
    }

    public void AimMove(bool isMove)
    {
        LayerWeight(1, 0f);
        LayerWeight(2, 1f);
        /*LayerWeight(3, 1f);*/
        anim.SetBool("isAimMove", isMove);
    }
    public void SelectBoolMove(bool isNonBattleMove,bool isBattleMove, bool isAimMove)
    {
        anim.SetBool("isNonBattleMove", isNonBattleMove);
        anim.SetBool("isBattleMove", isBattleMove);
        anim.SetBool("isAimMove", isAimMove);
    }
    public void AttackAni()
    {
        anim.SetTrigger("onShot");
    }
    public void ReloadAni()
    {
        anim.SetTrigger("onReload");
    }
    public void CallBackAni()
    {
        anim.SetTrigger("onCallBack");
    }
    private void LayerWeight(int layerIndex, float weight)
    {
        anim.SetLayerWeight(layerIndex, weight);
    }
}