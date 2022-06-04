using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldItem : MonoBehaviour
{
    public Inventory inventory;
    public Sprite itemImage;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.SetImage(itemImage);
                gameObject.SetActive(false);
            }
        }
    }
}
