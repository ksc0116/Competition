using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Item_Drop : MonoBehaviour, IDropHandler,IPointerClickHandler
{
    public bool inQuick = false;
    Image img;
    public void OnDrop(PointerEventData eventData)
    {
        if (inQuick == true)
        {
            Debug.Log("Quick");
            Transform item = transform.GetChild(1);
            item.SetParent(Manager.Instance.manager_Inven.curParent);
            item.localPosition = Vector3.zero;
        }
        else
        {
            Debug.Log("Inventory");
            Transform item = transform.GetChild(0);
            item.SetParent(Manager.Instance.manager_Inven.curParent);
            item.localPosition = Vector3.zero;
        }
        Manager.Instance.manager_Inven.selectedItem.SetParent(transform);
        Manager.Instance.manager_Inven.selectedItem.localPosition = Vector3.zero;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        img = GetComponent<Image>();
        img.color = Color.black;
    }
}