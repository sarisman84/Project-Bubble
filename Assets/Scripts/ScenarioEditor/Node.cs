using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class Node
{
    //Main box
    public Rect rect;


    public const int
        PADDING = 15,
        SPACING = 5,
        TEXTSQUAREHEIGHT = 15;

    public bool isDragged;
    public bool isSelected;

    public ConnectionPoint inPoint;
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;


    public abstract void Drag(Vector2 delta);
    public abstract void Draw();

    public bool ProcessEvents(UnityEngine.Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }
                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }
        return false;
    }
    private void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
        genericMenu.ShowAsContext();
    }

    private void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}

public class EventNode : Node
{
    public Event myEvent;
    public bool isStartNode = false;


    //Positions and sizes
    public const int
        boxWidth = 300,
        boxHeight = 180;

    //Small boxes and graphics
    Rect imageRect;
    Rect titleRect;
    Rect descriptionRect;

    Rect isStartNodeRect;


    public EventNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, boxWidth, boxHeight);
        isStartNodeRect = new Rect(position.x + PADDING, position.y + PADDING, boxWidth/2 - PADDING -SPACING, TEXTSQUAREHEIGHT);
        titleRect = new Rect(position.x + PADDING, isStartNodeRect.y + isStartNodeRect.height + SPACING, boxWidth/2 - PADDING - SPACING, TEXTSQUAREHEIGHT*3);
        imageRect = new Rect(position.x + boxWidth/2, isStartNodeRect.y, boxWidth/2 - PADDING, titleRect.height+isStartNodeRect.height + SPACING + SPACING);

        descriptionRect = new Rect(position.x + PADDING, imageRect.y + imageRect.height + SPACING, boxWidth - PADDING * 2, TEXTSQUAREHEIGHT * 5);
    
        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, 0f);


        ConnectionPoint choiceA = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight/5);
        ConnectionPoint choiceB = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 5 * 2);
        ConnectionPoint choiceC = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 5 * 3);
        ConnectionPoint choiceD = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 5 * 4);

        outPoints.Add(choiceA);
        outPoints.Add(choiceB);
        outPoints.Add(choiceC);
        outPoints.Add(choiceD);


        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;


        myEvent = new Event("Title Text", "Event Text");
    }


    public override void Drag(Vector2 delta)
    {
        rect.position += delta;
        titleRect.position += delta;
        descriptionRect.position += delta;
        imageRect.position += delta;
        isStartNodeRect.position += delta;
    }


    public override void Draw()
    {
        EditorStyles.textField.wordWrap = true;
        inPoint.Draw();
        for (int i = 0; i < outPoints.Count; i++)
        {
            outPoints[i].Draw();
        }
        GUI.Box(rect, "", style);

        myEvent.title = EditorGUI.TextField(titleRect, myEvent.title);
        myEvent.image = (Sprite)EditorGUI.ObjectField(imageRect, myEvent.image, typeof(Sprite), false);
        myEvent.description = EditorGUI.TextField(descriptionRect, myEvent.description);
        EditorGUI.DrawRect(isStartNodeRect, Color.grey);
        EditorGUI.LabelField(isStartNodeRect, "   Is startnode?");
        isStartNode = EditorGUI.Toggle(isStartNodeRect, isStartNode);
    }
}

public class ChoiceNode : Node
{
    public Choice myChoice;

    //Sizes and Positions
    float boxWidth = 250;
    float boxHeight = 100;


    Rect choiceTextRect;
    Rect skillTypeInfoRect;
    Rect skillTypeRect;


    public override void Drag(Vector2 delta)
    {
        rect.position += delta;
        choiceTextRect.position += delta;
        skillTypeInfoRect.position += delta;
        skillTypeRect.position += delta;
    }

    public override void Draw()
    {
        EditorStyles.textField.wordWrap = true;
        inPoint.Draw();
        for (int i = 0; i < outPoints.Count; i++)
        {
            outPoints[i].Draw();
        }
        GUI.Box(rect, "", style);

        myChoice.choiceText = EditorGUI.TextField(choiceTextRect, myChoice.choiceText);
        EditorGUI.DrawRect(skillTypeInfoRect, Color.grey);
        EditorGUI.LabelField(skillTypeInfoRect, "Skill increase");
        myChoice.skillType = (Characteristics)EditorGUI.EnumPopup(skillTypeRect, myChoice.skillType);
    }

    public ChoiceNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, boxWidth, boxHeight);
        choiceTextRect = new Rect(position.x + PADDING, position.y + PADDING, boxWidth - PADDING * 2, TEXTSQUAREHEIGHT*2);
        skillTypeInfoRect = new Rect(choiceTextRect.x, choiceTextRect.y + choiceTextRect.height + SPACING, boxWidth / 2 - PADDING, TEXTSQUAREHEIGHT);
        skillTypeRect = new Rect(skillTypeInfoRect.x + boxWidth / 2 - PADDING, skillTypeInfoRect.y, boxWidth / 2 - PADDING, TEXTSQUAREHEIGHT);


        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, 0f);
        outPoints.Add(new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 2));

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;


        myChoice = new Choice("New Choice");
    }
}
