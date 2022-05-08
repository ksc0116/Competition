using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rigid;

/*    public void MoveTo(Vector3 dir)
    {
        rigid.AddForce(dir * 20f, ForceMode.VelocityChange);
    }*/

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
}
