using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Simon Voss
[CreateAssetMenu(menuName = "Scenario/Scenario")]
public class Scenario : ScriptableObject
{
    public Event startEvent = null;

#if UNITY_EDITOR

    public List<ChoiceNode> editorChoiceNodes = new List<ChoiceNode>();
    public List<EventNode> editorEventNodes = new List<EventNode>();
    public List<Connection> editorConnections = new List<Connection>();
    public List<ScenarioEndNode> editorScenarioEndNodes = new List<ScenarioEndNode>();
#endif
    public List<Event> events = new List<Event>();

    public Event FindNextEvent(Choice usedChoice)
    {
        for (int i = 0; i < editorEventNodes.Count; i++)
        {
            if (usedChoice.nextEventID == editorEventNodes[i].myEvent.id)
            {
                return editorEventNodes[i].myEvent;
            }
        }
        Debug.Log("New event not found");
        return null;
    }
}

[System.Serializable]
public class Event
{
    public string locationText;
    public string description;
    public Sprite image;
    public List<Choice> choices = new List<Choice>();

    public double id;
    public List<double> usedIDs = new List<double>();

    public Event(string locationText, string description)
    {
        this.locationText = locationText;
        this.description = description;
        id = GetUniqueID();
    }

    public double GetUniqueID()
    {
        System.Random rng = new System.Random();
        double newID = rng.NextDouble();
        if (!usedIDs.Contains(newID))
        {
            usedIDs.Add(newID);
            return newID;
        }
        else
        {
            return GetUniqueID();
        }
    }
}

public enum ItemTransfer { Off, PlayerGetItem, PlayerLoseItem}
public enum QuestProcessing { GiveQuest, CompleteQuest, Failquest}
//Contains data of choices and it's effects on the world
[System.Serializable]
public class Choice
{
    public string choiceText = "";

    //Reward/skillincrease
    public Characteristics skillType;
    public int skillNumberIncrease;

    public ItemTransfer itemtransfer;
    public ScriptableInventoryItem item;

    public ScriptableQuest connectedQuest;
    public QuestProcessing processQuest;

    public ScriptableQuest requiredStartedQuest;
    public ScriptableQuest requiredCompletedQuest;

    public Characteristics requiredSkill;
    public int requiredSkillNumber;

    public ScriptableNpc affectedNPC;
    public RelationshipAttribute relationshipAttributeToChange;
    public int relationshipAttributeChangeNumber = 0;
    public RelationshipLevel minimumRelationshiplevel;

    //outputs
    //public Event nextEvent = null;
    public double nextEventID;
    public Scenario nextScenario = null;
    public string nextScene = "";

    public Choice(string choiceText)
    {
        this.choiceText = choiceText;
    }
}
