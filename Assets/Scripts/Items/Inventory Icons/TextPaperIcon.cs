using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPaperIcon : MonoBehaviour, IItemIcon //Dejan, icon that represents the Text Paper prefab
{
    public GameObject textPaper; //Text Paper prefab

    public int ItemID() //unique item id, same as corresponding 3D object
    {
        return 1;
    }

    public string ItemName() //item name
    {
        return "Text Paper";
    }

    public bool UseItem() //Instantiates and executes InteractWith() on Text Paper prefab
    {
        GameObject newTextPaper = Instantiate(textPaper, new Vector3(0, 1000, 0), Quaternion.identity);
        newTextPaper.GetComponent<IInteractable>().InteractWith();
        Destroy(gameObject);
        return true;
    }
}
