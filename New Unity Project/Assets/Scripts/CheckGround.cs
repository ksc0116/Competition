using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    public bool isGround;
    public LayerMask layerMask;

    Animator anim;
    bool isFall;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        CheckGrounded();
    }

    private void CheckGrounded()
    {
        Vector3 rayStartPosition = transform.position + new Vector3(0, 0.5f, 0);
        if (Physics.Raycast(rayStartPosition, Vector3.down, 0.6f, layerMask))
        {
            isGround = true;
            isFall = false;
            anim.SetBool("isFall", isFall);
        }
        else
        {
            isGround = false;
            isFall = true;
            anim.SetBool("isFall", isFall);
        }
    }
}
