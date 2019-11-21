using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
[CreateAssetMenu(menuName = "Inventory/Item")]
public class ScriptableInventoryItem : ScriptableObject
{
    public string itemName = "";
    public int id = 0;
    //public Sprite icon;
    //public Gameobject 3dobject;
}
