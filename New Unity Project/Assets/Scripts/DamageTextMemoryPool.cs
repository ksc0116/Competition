using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageTextMemoryPool : MonoBehaviour
{
    [SerializeField]
    GameObject damageTextPrefab;
    MemoryPool m_pool;


    private void Awake()
    {
        m_pool = new MemoryPool(damageTextPrefab);
    }

    public void SpawnDamageText(Vector3 position,float damage)
    {
        Debug.Log("텍스트 생성");
        GameObject clone = m_pool.ActivePoolItem();
        clone.GetComponent<DamageText>().SetUp(m_pool);
        clone.transform.localScale = Vector3.one * 0.5f;
        clone.GetComponent<TextMesh>().text = damage.ToString();
        clone.transform.parent=transform;
        clone.transform.position = position+new Vector3(0,1.5f,0);
        clone.transform.rotation = Quaternion.identity;
    }
}
