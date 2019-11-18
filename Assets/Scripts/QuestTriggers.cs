using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggers : MonoBehaviour //Dejan, this script should be used in combination with the QuestLog.cs to activate/Deactivate quests on trigger enter
{
    QuestLog questLog; //a reference to the QuestLog.cs (singelton)

    [SerializeField] int questID = 0; //the unique ID of the quest to be activated/deactivated
    [SerializeField] bool start = false; //set true on the trigger that should activate the quest
    [SerializeField] bool debug = true; //set false before building the game

    private void Start()
    {
        questLog = QuestLog.Instance(); //initiates/finds the singeltone QuestLog.cs
        if (!debug)
        {
            GetComponent<MeshRenderer>().enabled = false; //deactivates the mesh
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (start) //if start is true, the quest is activated, otherwise it is ended
        {
            questLog.ActivateQuest(questID);
            questLog.UpdateObjectives();
        }
        else
        {
            questLog.EndQuest(questID);
            questLog.UpdateObjectives();
        }
    }
}
