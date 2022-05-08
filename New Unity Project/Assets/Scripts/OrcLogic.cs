using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcLogic : MonoBehaviour
{
    public int HP = 100;

    [SerializeField]
    private Material orcMaterial;

    [SerializeField]
    private Material wolfMaterial;
    public void TakeDamage(int damage)
    {
        HP -= damage;
        StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor()
    {
        Color wolfColor = wolfMaterial.color;
        wolfMaterial.color = Color.red;

        Color orcColor=orcMaterial.color;
        orcMaterial.color = Color.red;  
        
        yield return new WaitForSeconds(0.1f);
        wolfMaterial.color = wolfColor;
        orcMaterial.color= orcColor;
    }
}
