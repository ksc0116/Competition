using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreenLogic : MonoBehaviour
{
    [SerializeField]
    private Transform targetPosition;
    [SerializeField]
    private Transform originCameraPosition;

    public float moveSpeed = 0.01f;

    public bool isReach = false;

    public float maxTime = 10f;
    private float currentTime = 0f;

    private void Update()
    {
        ChangeFlag();
        MoveToTarget();
        MoveToOriginPosition();
    }

    private void MoveToTarget()
    {
        if (isReach == false)
        {
           transform.position = Vector3.Lerp(transform.position, targetPosition.position, moveSpeed);
        }
    }
    private void MoveToOriginPosition()
    {
        if (isReach == true)
        {
            transform.position = Vector3.Lerp(transform.position, originCameraPosition.position, moveSpeed);
        }
    }

    private void ChangeFlag()
    {
        if (Vector3.Distance(transform.position, targetPosition.position) < 0.001f)
        {
            isReach = true;
        }
        else if (Vector3.Distance(transform.position, originCameraPosition.position) < 0.001f)
        {
            isReach = false;
        }
    }
}  