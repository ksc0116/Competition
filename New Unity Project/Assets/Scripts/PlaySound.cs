using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlaySE(AudioClip clip)
    {
        Manager.Instance.manager_SE.seAudio.PlayOneShot(clip);
    }
    public void PlayFootStepSE(float volume)
    {
        Manager.Instance.manager_SE.seAudio.PlayOneShot(Manager.Instance.manager_SE.footStep, volume);
    }
}
