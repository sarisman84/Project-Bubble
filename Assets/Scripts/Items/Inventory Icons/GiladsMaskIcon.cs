using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiladsMaskIcon : MonoBehaviour, IItemIcon //Dejan, icon representing Gilad's Mask 3d object
{
    public int ItemID() //unique item id, same as corresponding 3d item
    {
        return 0;
    }

    public string ItemName() //item name
    {
        return "Gilad's Mask";
    }

    public bool UseItem() //does nothing
    {
        return false;
    }
}
