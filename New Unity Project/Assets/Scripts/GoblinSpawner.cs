using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSpawner : MonoBehaviour
{
    [SerializeField]
    DamageTextMemoryPool m_pool;
    [SerializeField]
    private GoblinSceneManager goblinSceneManager;

    public Transform[] hunterSpawnPoints;

    [SerializeField]
    private GameObject hunterGoblinPrefab;

    [SerializeField]
    private Transform target;

    public Transform[] groundSpawnPoints;

    [SerializeField]
    private GameObject groundGoblinPrefab;

    private void Awake()
    {
        for(int i=0; i < groundSpawnPoints.Length; i++)
        {
            GameObject clone = Instantiate(groundGoblinPrefab, groundSpawnPoints[i].position, Quaternion.identity);
            GoblinFSM cloneLogic = clone.GetComponent<GoblinFSM>();
            cloneLogic.SetUp(target,m_pool);
        }
    }
    public IEnumerator SpawnHunter()
    {
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < hunterSpawnPoints.Length; i++)
        {
            GameObject clone = Instantiate(hunterGoblinPrefab, hunterSpawnPoints[i].position, Quaternion.identity);
            GoblinLogic cloneLogic = clone.GetComponent<GoblinLogic>();
            ArrowMemoryPool pool=clone.GetComponent<ArrowMemoryPool>();
            cloneLogic.SetUp(target,m_pool);
        }
    }
}
