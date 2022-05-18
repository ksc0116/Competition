using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField]
    Blink playerDash;
    [SerializeField]
    CheckGround playerGrounded;
    public void PlaySE(AudioClip clip)
    {
        Manager.Instance.manager_SE.seAudio.PlayOneShot(clip);
    }
    public void PlayFootStepSE(float volume)
    {
        if (playerDash.isDash == true || playerGrounded.isGround==false) return;
        Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.footStep, volume);
    }
}
