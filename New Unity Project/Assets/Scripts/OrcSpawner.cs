using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcSpawner : MonoBehaviour
{
    [SerializeField]
    DamageTextMemoryPool m_pool;

    [SerializeField]
    private GameObject spawnPoint;
    [SerializeField]
    private Transform firstSpawnPoint;
    [SerializeField]
    private Transform[] setPos;
    [SerializeField]
    private GameObject orcPrefab;
    [SerializeField]
    private Transform target;

    private GameObject[] orcs;

    public bool[] isReady;

    private bool finalCheck = false;

    private bool isFirst = true;
    private void Awake()
    {
        isReady = new bool[setPos.Length];
        for(int i = 0; i < isReady.Length; i++)
        {
            isReady[i] = false;
        }
        orcs = new GameObject[setPos.Length];
    }
    private void Update()
    {
        CheckReady();
        Ready();
    }
    private void CheckReady()
    {
        if(isReady[0]==true && isReady[1] == true&& isReady[2] == true && isReady[3] == true && isReady[4] == true && isReady[5] == true && isReady[6] == true)
        {
            finalCheck = true;
        }
    }

    private void Ready()
    {
        if (isFirst == true)
        {
            if (finalCheck == true)
            {
                isFirst = false;
                spawnPoint.SetActive(false);
                for (int i = 0; i < orcs.Length; i++)
                {
                    orcs[i].GetComponent<OrcLogic>().isReady = true;
                }
            }
        }
    }
    public void CreateOrc()
    {
        for(int i = 0; i < setPos.Length; i++)
        {
            orcs[i]= Instantiate(orcPrefab, firstSpawnPoint.position, Quaternion.identity);
            orcs[i].transform.parent = transform;
            orcs[i].name="orc"+i.ToString();
            OrcLogic logic = orcs[i].GetComponent<OrcLogic>();
            logic.Setup(target,this,i,setPos[i],m_pool);
            logic.ChangeState(OrcState.First);
            /*StartCoroutine( logic.FirstSetDestination(setPos[i]));*/
        }
    }
}
