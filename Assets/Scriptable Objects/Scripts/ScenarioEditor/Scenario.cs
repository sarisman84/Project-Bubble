using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Simon Voss
[CreateAssetMenu(menuName = "Scenario/Scenario")]
public class Scenario : ScriptableObject
{
    public Event startEvent = null;
    public List<ChoiceNode> editorChoiceNodes = new List<ChoiceNode>();
    public List<EventNode> editorEventNodes = new List<EventNode>();
    public List<Connection> editorConnections = new List<Connection>();
    public List<ScenarioEndNode> editorScenarioEndNodes = new List<ScenarioEndNode>();
}

[System.Serializable]
public class Event
{
    public string locationText;
    public string description;
    public Sprite image;
    public List<Choice> choices = new List<Choice>();
    public Event(string locationText, string description)
    {
        this.locationText = locationText;
        this.description = description;
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
    public Event nextEvent = null;
    public Scenario nextScenario = null;
    public SceneAsset nextScene;

    public Choice(string choiceText)
    {
        this.choiceText = choiceText;
    }
}
