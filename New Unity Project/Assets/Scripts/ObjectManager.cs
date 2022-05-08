using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    private GameObject[] playerBullet;
    public GameObject playerBulletPrefab;

    private GameObject[] playerBulletFlash;
    public GameObject playerBulletFlashPrefab;

    private GameObject[] playerBulletHit;
    public GameObject playerBulletHitPrefab;

    private GameObject[] targetPool;
    private void Awake()
    {
        playerBullet = new GameObject[50];
        playerBulletFlash=new GameObject[50];
        playerBulletHit=new GameObject[50];

        Generate();
    }
    private void Generate()
    {
        for(int i = 0; i < playerBullet.Length; i++)
        {
            playerBullet[i]=Instantiate(playerBulletPrefab);
            playerBullet[i].transform.SetParent(transform);
            playerBullet[i].SetActive(false);
        }

        for (int i = 0; i < playerBulletFlash.Length; i++)
        {
            playerBulletFlash[i] = Instantiate(playerBulletFlashPrefab);
            playerBulletFlash[i].transform.SetParent(transform);
            playerBulletFlash[i].SetActive(false);
        }

        for (int i = 0; i < playerBulletHit.Length; i++)
        {
            playerBulletHit[i] = Instantiate(playerBulletHitPrefab);
            playerBulletHit[i].transform.SetParent(transform);
            playerBulletHit[i].SetActive(false);
        }
    }
    public GameObject MakeObj(string type)
    {
        
        switch (type)
        {
            case "playerBullet":
                targetPool = playerBullet;
                break;
            case "playerBulletFlash":
                targetPool=playerBulletFlash;
                break;
            case "playerBulletHit":
                targetPool = playerBulletHit;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }
        return null;
    }
}
