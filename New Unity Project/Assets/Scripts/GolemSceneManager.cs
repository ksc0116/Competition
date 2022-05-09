using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] spawnPoint;
    [SerializeField]
    private GameObject clearDoor;
    [SerializeField]
    private Transform doorOpenPos;

    public int golemCount;

    private float openTime = 2000f;

    private void Awake()
    {
        golemCount =spawnPoint.Length;
    }
    private void Update()
    {
        if (golemCount == 0)
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

            clearDoor.transform.position=Vector3.Lerp(clearDoor.transform.position, doorOpenPos.position, percent);

            yield return null;
        }
    }
}