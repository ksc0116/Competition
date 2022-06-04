using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Manager")]
    public Manager_SE manager_SE;
    public GoblinSceneManager goblinSceneManager;
    public GolemSceneManager golemSceneManager;
    public OrcSceneManager orcSceneManager;
    public Manager_Inven manager_Inven;
    private void Awake()
    {
        if (Instance !=this)
            Instance = this;
    }
}
