using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemKind { Weapon=0, Potion}
public class Items_Info : MonoBehaviour
{
    public ItemKind itemKind;
    public int atk_Bonus;
    public float hp_Bonus;
    public float mp_Bonus;
}
