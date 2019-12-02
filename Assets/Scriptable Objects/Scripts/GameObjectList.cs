using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Gameobject List", menuName = "Scriptable Objects/ Gameobject List", order = 0)]
public class GameObjectList : ScriptableObject //Dejan
{
    public List<ItemPairs> list = new List<ItemPairs>(); //a list of lists
}

[System.Serializable]
public class ItemPairs //class wrap for pairs of items
{
    public List<GameObject> pair;
}


