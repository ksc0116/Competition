using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    [Header("Manager")]
    public Manager_SE manager_SE;
    public GoblinSceneManager goblinSceneManager;

    private void Awake()
    {
        if (Instance !=this)
            Instance = this;
    }
}
