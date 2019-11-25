using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Simon Voss
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

