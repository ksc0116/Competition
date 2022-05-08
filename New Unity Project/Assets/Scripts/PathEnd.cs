using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathEnd : MonoBehaviour
{
    [SerializeField]
    private PathEndDoor door;

    [SerializeField]
    private GameObject fadeObj;
    private Image fadeImage;
    private float fadeTime = 2.0f;

    [Header("Golems")]
    [SerializeField]
    private GameObject[] golems;
    [SerializeField]
    private GameObject[] rocks;
    private void Awake()
    {
        fadeImage=fadeObj.GetComponent<Image>();
    }

    private void ChangeForm()
    {
        for(int i = 0; i < golems.Length; i++)
        {
            rocks[i].gameObject.SetActive(false);
            golems[i].gameObject.SetActive(true);
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
