using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSceneManager : MonoBehaviour
{
    [SerializeField]
    private GoblinSpawner goblinSpawner;
    [SerializeField]
    private Transform door;
    [SerializeField]
    private Transform openPos;

    public int hunterGoblinCount;
    public int groundGoblinCount;

    private float closeTime = 1000f;
    private void Awake()
    {
        hunterGoblinCount = goblinSpawner.hunterSpawnPoints.Length;
        groundGoblinCount=goblinSpawner.groundSpawnPoints.Length;
    }
    private void Update()
    {
        GoblinSceneClear();
    }
    private void GoblinSceneClear()
    {
        if(hunterGoblinCount == 0 && groundGoblinCount == 0)
        {
            StartCoroutine(OpenDoor());
        }
    }

    private IEnumerator OpenDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / closeTime;

            door.position = Vector3.Lerp(door.localPosition, openPos.localPosition, percent);

            yield return null;
        }
    }
}
