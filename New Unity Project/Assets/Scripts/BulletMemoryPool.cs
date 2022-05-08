using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMemoryPool : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject flashPrefab;
    public GameObject hitPrefab;

    public MemoryPool projectileMemoryPool;
    public MemoryPool flashMemoryPool;
    public MemoryPool hitMemoryPool;

    private void Awake()
    {
        projectileMemoryPool = new MemoryPool(projectilePrefab);
        flashMemoryPool = new MemoryPool(flashPrefab);
        hitMemoryPool = new MemoryPool(hitPrefab);
    }
}
