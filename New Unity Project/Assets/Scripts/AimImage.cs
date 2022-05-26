using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimImage : MonoBehaviour
{
    [SerializeField]
    S_PlayerController player;
    Image aimImage;

    private void Awake()
    {
        aimImage = GetComponent<Image>();   
    }

    private void Update()
    {
        if (player.isPause == true)
        {
            aimImage.enabled = false;
        }
        else if (player.isPause == false)
        {
            aimImage.enabled = true;
        }
    }
}
