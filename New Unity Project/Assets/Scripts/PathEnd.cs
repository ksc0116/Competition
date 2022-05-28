using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathEnd : MonoBehaviour
{
    [SerializeField]
    DamageTextMemoryPool m_pool;

    [SerializeField]
    private Transform target;


    [SerializeField]
    private GameObject fadeObj;
    private Image fadeImage;
    private float fadeTime = 2.0f;

    [Header("Golems")]
    [SerializeField]
    private GameObject rockPrefab;
    [SerializeField]
    private GameObject golemPrefab;
    [SerializeField]
    private Transform[] spawnPoint;

    private GameObject[] rocks;
    private GameObject[] golems;
    PathEndDoor door;
    private void Awake()
    {
        door = GetComponent<PathEndDoor>();
        rocks=new GameObject[spawnPoint.Length];
        golems= new GameObject[spawnPoint.Length];
        fadeImage =fadeObj.GetComponent<Image>();
        SetRock();
    }
    private void SetRock()
    {
        for (int i = 0; i < spawnPoint.Length; i++)
        {
            rocks[i] = Instantiate(rockPrefab, spawnPoint[i].position, Quaternion.identity);
            golems[i] = Instantiate(golemPrefab, spawnPoint[i].position, Quaternion.Euler(0,180f,0));
            golems[i].SetActive(false);
        }
    }
    private void ChangeForm()
    {
        for (int i = 0; i < golems.Length; i++)
        {
            rocks[i].gameObject.SetActive(false);
            golems[i].gameObject.SetActive(true);
            Golem logic = golems[i].GetComponent<Golem>();
            logic.Setup(target,m_pool);
        }
    }

    private IEnumerator Fade()
    {
        fadeObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(FadeInOut(0, 1));
        yield return StartCoroutine(FadeInOut(1, 0));
        yield return StartCoroutine(FadeInOut(0, 1));
        ChangeForm();
        yield return StartCoroutine(FadeInOut(1, 0));
        fadeObj.SetActive(false);
    }

    private IEnumerator FadeInOut(float start,float end)
    {
        float currentTime = 0.0f;
        float percent = 0.0f;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / fadeTime;

            Color color = fadeImage.color;
            color.a = Mathf.Lerp(start,end,percent);
            fadeImage.color = color;

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(door.CloseDoor());
            if (transform.tag == "GolemPortal")
            {
                StartCoroutine(Fade());
            }
        }
    }
}
