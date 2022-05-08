using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEndDoor : MonoBehaviour
{
    [SerializeField]
    private Transform closePos;

    private float closeTime = 100f;

    public IEnumerator CloseDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / closeTime;

            transform.position = Vector3.Lerp(transform.position, closePos.position, percent);

            yield return null;
        }
    }
}
