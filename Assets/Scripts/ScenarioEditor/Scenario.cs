﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event Scenario/Scenario")]
public class Scenario : ScriptableObject
{
    public Event startEvent = null;
    //public List<Node> editorNodes = new List<Node>();
    public List<ChoiceNode> editorChoiceNodes = new List<ChoiceNode>();
    public List<EventNode> editorEventNodes = new List<EventNode>();
    public List<Connection> editorConnections = new List<Connection>();
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



[System.Serializable]
public class Choice
{
    public string choiceText = "";
    public Event nextEvent = null;
    public Characteristics skillType;

    //public NPC npc;
    
    //ATTRIBUTECHANGE TYPE
    //ATTRIBUTE CHANGE NUMBER

    public Choice(string choiceText)
    {
        this.choiceText = choiceText;
    }
}
