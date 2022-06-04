using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Items_Action : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public Image img;

    float releaseTime;
    bool dragging;


    public void OnPointerDown(PointerEventData eventData)
    {
        img=GetComponent<Image>();
        Manager.Instance.manager_Inven.selectedItem = transform;
        StartCoroutine("ReleaseItem");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopCoroutine("ReleaseItem");
        transform.localScale = Vector3.one;
        if (releaseTime >= 0.1f)
        {
            transform.SetParent(Manager.Instance.manager_Inven.curParent);
            transform.localPosition = Vector3.zero;
            img.raycastTarget = true;
            return;
        }
        else
        {
            // 클릭 이벤트
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            transform.position = eventData.position;
        }
    }

    IEnumerator ReleaseItem()
    {
        releaseTime = 0;
        dragging = false;
        while (true)
        {
            releaseTime+=Time.deltaTime;

            if (releaseTime>=0.1f)
            {
                transform.localScale = new Vector3(1.3f, 1.3f, 1);
                if (!dragging)
                {
                    Manager.Instance.manager_Inven.curParent = transform.parent;
                    transform.SetParent(Manager.Instance.manager_Inven.parentOnDrag);
                    dragging = true;
                    img.raycastTarget = false;
                }
            }
            yield return null;
        }
    }
}