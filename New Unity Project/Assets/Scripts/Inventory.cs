using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Inventory : MonoBehaviour
{
    [Header("[SlotInImage]")]
    public GameObject[] slots;
    int[] itemCount;
    Image tempImage;

    private void Awake()
    {
        itemCount = new int[slots.Length];
        for (int i = 0; i < itemCount.Length; i++)
        {
            itemCount[i] = 1;
        }
    }

    public void SetImage(Sprite sprite)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (CheckSprite(sprite) == true)
            {
                break;
            }
            tempImage=slots[i].transform.GetChild(0).GetComponent<Image>();
            if (tempImage.sprite == null)
            {
                tempImage.sprite = sprite;
                slots[i].transform.GetChild(0).gameObject.SetActive(true);
                break;
            }
        }
    }
   bool CheckSprite(Sprite sprite)
    {
        for(int i=0;i<slots.Length; i++)
        {
            if(slots[i].transform.GetChild(0).GetComponent<Image>().sprite == sprite)
            {
                itemCount[i]++;
                slots[i].transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text=itemCount[i].ToString();
                return true;
            }
        }
        return false;
    }
}