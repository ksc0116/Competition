using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }
    private IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
