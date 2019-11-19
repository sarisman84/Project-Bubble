using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiladsMaskItem : MonoBehaviour, IInteractable //Dejan, gillads mask 3d object
{
    public int itemID; //uniue item id

    public bool InteractWith() //executes when interacted with, adds item to invetory
    {
        Inventory.instance.AddItemToInventory(0);
        Destroy(gameObject);
        QuestLog.Instance().ActivateQuest(0);
        return false;
    }

    public string MessageOnDetection() //displays on hover
    {
        return "Gilad's Mask";
    }

    public bool CanBeInteractedWith() //can always be interacted with
    {
        return true;
    }
    public void EndInteration() { } //not needed
}
