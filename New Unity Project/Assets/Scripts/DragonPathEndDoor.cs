using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPathEndDoor : MonoBehaviour
{
    [SerializeField]
    private Transform doorOriginPos;
    [SerializeField]
    private Transform doorClosePos;
    [SerializeField]
    private Dragon dragon;

    private IEnumerator CloseDoor()
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / 150.0f;

            doorOriginPos.position=Vector3.Lerp(doorOriginPos.position,doorClosePos.position,percent);

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(CloseDoor());
            StartCoroutine( dragon.ScreamAni());
        }
    }
}
