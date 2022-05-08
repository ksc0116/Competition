using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimImageChange : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private Sprite aimSprite;
    [SerializeField]
    private Sprite originSprite;

    private Image originImage;
    private void Awake()
    {
        originImage = GetComponent<Image>();
    }
    private void Update()
    {
        ChangeImage();
    }
    private void ChangeImage()
    {
        if(playerController.state == PlayerState.AimMode)
        {
            originImage.sprite = aimSprite;
        }
        else
        {
            originImage.sprite = originSprite;
        }
    }
}
