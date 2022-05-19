using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcScenePathEnd : MonoBehaviour
{
    [SerializeField]
    private Transform doorOriginPos;
    [SerializeField]
    private Transform doorClosePos;
    [SerializeField]
    private OrcSpawner spawner;
    private float closeTime = 250f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CloseDoor());
            spawner.CreateOrc();
        }
    }

    private IEnumerator CloseDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime+=Time.deltaTime;
            percent = currentTime / closeTime;

            doorOriginPos.position = Vector3.Lerp(doorOriginPos.position, doorClosePos.position, percent);

            yield return null;
        }
    }
}
