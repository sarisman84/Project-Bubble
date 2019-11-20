using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory")]
public class ScriptableInventory : ScriptableObject
{
    [SerializeField] List<ScriptableInventoryItem> itemsInInventory = new List<ScriptableInventoryItem>();

    public bool CheckIfItemIsInInventory(ScriptableInventoryItem item)
    {
        return (itemsInInventory.Contains(item));
    }

    public void AddItemToInventory(ScriptableInventoryItem item)
    {
        itemsInInventory.Add(item);
    }

    public void RemoveItemFromInventory(ScriptableInventoryItem item)
    {
        if (itemsInInventory.Contains(item))
        {
            itemsInInventory.Remove(item);
        }
    }
}

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ScriptableInventoryItem : ScriptableObject
{
    public string itemName = "";
    public int id = 0;
    //public Sprite icon;
    //public Gameobject 3dobject;
}
