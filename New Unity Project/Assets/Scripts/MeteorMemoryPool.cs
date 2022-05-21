using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorMemoryPool : MonoBehaviour
{
    [SerializeField]
    Transform target;
    [SerializeField]
    GameObject meteorPrefab;
    [SerializeField]
    GameObject pointPrefab;
    [SerializeField]
    GameObject explosionPrefab;

    MemoryPool meteorMemoryPool;
    MemoryPool pointMemoryPool;
    MemoryPool explosionMemoryPool;

    private void Awake()
    {
        meteorMemoryPool = new MemoryPool(meteorPrefab);
        pointMemoryPool=new MemoryPool(pointPrefab);
        explosionMemoryPool=new MemoryPool(explosionPrefab);
    }
    public GameObject SpawnMeteor()
    {
        GameObject item= meteorMemoryPool.ActivePoolItem();
        item.GetComponent<Meteor>().Setup(target, meteorMemoryPool, pointMemoryPool, explosionMemoryPool);
        return item;
    }
}
