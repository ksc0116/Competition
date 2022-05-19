using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemTest : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;

    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    public void TakeDamage()
    {
        StartCoroutine(Change());
    }
    private IEnumerator Change()
    {
        Color color=skinnedMeshRenderer.material.color;
        skinnedMeshRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        skinnedMeshRenderer.material.color = color;
    }
}
