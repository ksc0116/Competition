using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSoundManager : MonoBehaviour
{
    public AudioSource bgmSource;

    public AudioSource btnSource;

    private void Start()
    {
/*        bgmSource.volume = 1f;
        btnSource.volume = 1f;*/
        bgmSource.volume= PlayerPrefs.GetFloat("BGM");
        btnSource.volume=PlayerPrefs.GetFloat("SFX");
    }
    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = volume;
        SetBgmLevel(volume);
        Debug.Log(PlayerPrefs.GetFloat("BGM"));
    }

    public void SetBgmLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("BGM", sliderValue);
    }

    public void SetButtonVolume(float volume)
    {
        btnSource.volume = volume;
        SetSFXLevel(volume);
        Debug.Log(PlayerPrefs.GetFloat("SFX"));
    }
    public void SetSFXLevel(float sliderValue)
    {
        PlayerPrefs.SetFloat("SFX", sliderValue);
    }

    public void ButtonClick()
    {
        btnSource.Play();
    }
}
