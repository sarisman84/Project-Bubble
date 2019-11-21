using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scenario/Quest")]
public class ScriptableQuest : ScriptableObject
{
    public string questName = "";
    public int id = 0;

    public ScriptableQuest()
    {
        System.Random rng = new System.Random();
        id = rng.Next();
    }
}
