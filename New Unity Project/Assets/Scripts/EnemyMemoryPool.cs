using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform goal;
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float enemySpawnTime = 1;
    [SerializeField]
    private float enemySpawnLatency = 1;


    private int numberOfEnemiesSpawnedAtOne = 30;


    [SerializeField]
    private GameObject enemyPrefab;
    private MemoryPool enemyMemoryPool;

    [SerializeField]
    private GameObject airEnemyPrefab;
    private MemoryPool airEnemyMemoryPool;

    [SerializeField]
    private GameObject[] groundSpawnPoint;
    [SerializeField]
    private GameObject[] airSpawnPoint;

    private void Awake()
    {
        enemyMemoryPool = new MemoryPool(enemyPrefab);
        airEnemyMemoryPool = new MemoryPool(airEnemyPrefab);
    }

    private void OnEnable()
    {
        SpawnFlyrEnemy();
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        for(int i = 0; i < numberOfEnemiesSpawnedAtOne; i++)
        {
            GameObject item = enemyMemoryPool.ActivePoolItem();
            int index = Random.Range(0, groundSpawnPoint.Length);
            Vector3 position = new Vector3(groundSpawnPoint[index].transform.position.x, 0, groundSpawnPoint[index].transform.position.z);
            item.transform.position = position;
            item.GetComponent<EnemyFSM>().Setup(target, enemyMemoryPool, goal);
        }
    }

    private void SpawnFlyrEnemy()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject item = airEnemyMemoryPool.ActivePoolItem();
            int indgex = Random.Range(0,airSpawnPoint.Length);  
            item.transform.position = airSpawnPoint[indgex].transform.position;
            item.GetComponent<FlyingEnemyFSM>().Setup(target, airEnemyMemoryPool, goal);
        }
    }

    /*   private IEnumerator SpawnTile()
       {
           int currentNumber = 0;
           int maximumNumber = 50;

           while (true)
           {
               // ���ÿ� numberOfEnemiesSpawnAtOne ���ڸ�ŭ �����ǵ��� �ݺ��� ���
               for (int i = 0; i < numberOfEnemiesSpawnedAtOne; i++)
               {
                   GameObject item = spawnPointMemoryPool.ActivePoolItem();

                   item.transform.position = new Vector3(spawnPoint.transform.position.x, 1,
                                                                       spawnPoint.transform.position.z);

                   StartCoroutine("SpawnEnemy", item);
               }
               currentNumber++;
               if (currentNumber >= maximumNumber)
               {
                   currentNumber = 0;
                   numberOfEnemiesSpawnedAtOne++;
               }

               yield return new WaitForSeconds(enemySpawnTime);
           }
       }

       private IEnumerator SpawnEnemy(GameObject point)
       {
           yield return new WaitForSeconds(enemySpawnLatency);

           // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
           GameObject item = enemyMemoryPool.ActivePoolItem();
           Vector3 position = new Vector3(point.transform.position.x, 0, point.transform.position.z);
           item.transform.position = position;
           item.GetComponent<EnemyFSM>().Setup(target, enemyMemoryPool, goal);

           // Ÿ�� ������Ʈ ��Ȱ��ȭ
           spawnPointMemoryPool.DeactivePoolItem(point);
       }*/
}
