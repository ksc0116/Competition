using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPoint;
    public int orcCount;

    [SerializeField]
    private Transform doorOriginPos;
    [SerializeField]
    private Transform doorOpenPos;
    private float openTime = 150f;

    private void Awake()
    {
        orcCount=spawnPoint.Length;
    }

    private void Update()
    {
        if (orcCount == 0)
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent<1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / openTime;

            doorOriginPos.position=Vector3.Lerp(doorOriginPos.position,doorOpenPos.position,percent);

            yield return null;
        }
    }

}
