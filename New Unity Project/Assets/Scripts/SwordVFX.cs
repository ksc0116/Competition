using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFX : MonoBehaviour
{
    [SerializeField]
    GameObject leftSwordVFX;
    [SerializeField]
    GameObject rightSwordVFX;

    public void LeftSwordVFXOn()
    {
        leftSwordVFX.SetActive(true);
    }
    public void LeftSwordVFXOff()
    {
        leftSwordVFX.SetActive(false);
    }
//========================================================
    public void RightSwordVFXOn()
    {
        rightSwordVFX.SetActive(true);
    }
    public void RightSwordVFXOff()
    {
        rightSwordVFX.SetActive(false);
    }
}
