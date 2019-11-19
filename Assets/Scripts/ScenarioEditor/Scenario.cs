using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "Event Scenario/Scenario")]
public class Scenario : ScriptableObject
{
    public Event startEvent = null;
    //public List<Node> editorNodes = new List<Node>();
    public List<ChoiceNode> editorChoiceNodes = new List<ChoiceNode>();
    public List<EventNode> editorEventNodes = new List<EventNode>();
    public List<Connection> editorConnections = new List<Connection>();
    public List<ScenarioEndNode> editorScenarioEndNodes = new List<ScenarioEndNode>();
}

[System.Serializable]
public class Event
{
    public string title;
    public string description;
    public Sprite image;
    public List<Choice> choices = new List<Choice>();
    public Event(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}

public enum ItemTransfer { Off, PlayerGetItem, PlayerLoseItem}
//Contains data of choices and it's effects on the world
[System.Serializable]
public class Choice
{
    public string choiceText = "";
    public Characteristics skillType;

    public NPC_DataContainer affectedNPC;
    public RelationshipAttribute relationshipAttributeToChange;
    public int relationshipAttributeChangeNumber = 0;

    public ItemTransfer itemtransfer;
    public int itemID;

    public Event nextEvent = null;
    public Scenario nextScenario = null;
    public SceneAsset nextScene;

    public Choice(string choiceText)
    {
        this.choiceText = choiceText;
    }
}
