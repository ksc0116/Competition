using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    public GameObject hpBarPrefab;
    public Transform canvasTransform;

    Camera m_cam;


    GameObject hpBar = null;
    private void Awake()
    {
        m_cam = Camera.main;
        hpBar=Instantiate(hpBarPrefab,transform.position,Quaternion.identity,canvasTransform);
    }
    private void Update()
    {
        hpBar.transform.position = m_cam.WorldToScreenPoint(transform.position+new Vector3(0,1.5f,0));
    }
}
