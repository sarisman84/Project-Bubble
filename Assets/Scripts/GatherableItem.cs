using UnityEngine;

public class GatherableItem : MonoBehaviour, IInteractable //Dejan
{
    public int itemID; //item id used in Inventory Items scriptable object

    public bool CanBeInteractedWith()
    {
        return true;
    }

    public void EndInteration()
    {
    }

    public bool InteractWith()
    {
        if (this != null)
        {
            Inventory.instance.AddItemToInventory(itemID); //adds item to inventory
            Destroy(gameObject);
            return true;
        }
        return false;
    }

    public string MessageOnDetection()
    {
        return "Ready to be picked up, I'm a key!";
    }
}
