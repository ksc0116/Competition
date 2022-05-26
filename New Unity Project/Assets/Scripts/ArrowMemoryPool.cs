using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMemoryPool : MonoBehaviour
{
    [SerializeField]
    GameObject arrowPrefab;
    MemoryPool memoryPool;
    LineRenderer lr;

    [SerializeField]
    Transform target;
    [SerializeField]
    Transform firePos;

    private void Awake()
    {
        lr=GetComponent<LineRenderer>();
        memoryPool = new MemoryPool(arrowPrefab);
    }

    public void SpawnArrow(Vector3 position,Transform target)
    {
        this.target=target;
        DrawLine();
        GameObject item = memoryPool.ActivePoolItem();
        item.GetComponent<Arrow>().Setup(memoryPool);
        item.transform.position = position;
        item.transform.rotation = Quaternion.LookRotation(target.position - item.transform.position);
        item.transform.localScale = Vector3.one * 0.2f;
        item.GetComponent<Rigidbody>().velocity = Vector3.zero;
        item.GetComponent<Rigidbody>().AddForce((target.position - item.transform.position).normalized * item.GetComponent<Arrow>().MoveSpeed, ForceMode.Impulse);
    }


    private void DrawLine()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, firePos.position);
        lr.SetPosition(1, target.position + new Vector3(0, -1.0f, 0));
        StartCoroutine(AutoLineDestroy());
    }
    private IEnumerator AutoLineDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        lr.positionCount=0;
    }
}
