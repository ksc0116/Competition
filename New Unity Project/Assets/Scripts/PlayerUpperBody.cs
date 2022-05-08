using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpperBody : MonoBehaviour
{
    [SerializeField]
    private Quaternion originUpperBody;

    public void RotateChest(Quaternion rot)
    {
        transform.rotation= rot;
    }

    public void ResetRot()
    {
        transform.rotation = originUpperBody;
    }
}
