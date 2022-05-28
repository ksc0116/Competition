using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinPathEndCollider : MonoBehaviour
{
    [SerializeField]
    GoblinSpawner spawner;
    PathEndDoor pathEnd;
    private void Awake()
    {
        pathEnd = GetComponent<PathEndDoor>();  
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(spawner.SpawnHunter());
            StartCoroutine(pathEnd.CloseDoor());
        }
    }
}
