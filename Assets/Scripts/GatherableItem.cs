using UnityEngine;

public class GatherableItem : MonoBehaviour, IInteractable //Dejan
{
    public Invetory inventory; //a reference to the inventory prefab
    public int itemID; //item id used in Inventory Items scriptable object

    public void InteractWith()
    {
        if (this != null)
        {
            inventory.AddItemToInventory(itemID); //adds item to inventory
        }
        Destroy(gameObject);
    }

    public string MessageOnDetection()
    {
        return "Ready to be picked up, I'm a key!";
    }
}
