using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


[System.Serializable]
abstract public class Node
{
    //Main box
    public Rect rect;
    public enum NodeType { ChoiceNode, EventNode, ScenarioEndNode }
    public NodeType typeOfNode;
    public List<NodeType> allowedConnectionTypes = new List<NodeType>();


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


//Eventnode
[System.Serializable]
public class EventNode : Node
{
    public Event myEvent;
    public bool isStartNode = false;

    //Positions and sizes
    public const float
        boxWidth = 300,
        boxHeight = 180;

    //Small boxes and graphics
    public Rect imageRect;
    public Rect titleRect;
    public Rect descriptionRect;

    public Rect isStartNodeRect;


    public EventNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, boxWidth, boxHeight);
        isStartNodeRect = new Rect(position.x + PADDING, position.y + PADDING, boxWidth / 2 - PADDING - SPACING, TEXTSQUAREHEIGHT);
        titleRect = new Rect(position.x + PADDING, isStartNodeRect.y + isStartNodeRect.height + SPACING, boxWidth / 2 - PADDING - SPACING, TEXTSQUAREHEIGHT * 3);
        imageRect = new Rect(position.x + boxWidth / 2, isStartNodeRect.y, boxWidth / 2 - PADDING, titleRect.height + isStartNodeRect.height + SPACING + SPACING);

        descriptionRect = new Rect(position.x + PADDING, imageRect.y + imageRect.height + SPACING, boxWidth - PADDING * 2, TEXTSQUAREHEIGHT * 5);

        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, 0f);


        ConnectionPoint choiceA = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 5);
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

        typeOfNode = NodeType.EventNode;
        allowedConnectionTypes.Add(NodeType.ChoiceNode);

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
        inPoint.Draw(this);
        for (int i = 0; i < outPoints.Count; i++)
        {
            outPoints[i].Draw(this);
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

//Choicenode
[System.Serializable]
public class ChoiceNode : Node
{
    public Choice myChoice;

    //Sizes and Positions
    public const float
        boxWidth = 250,
        boxHeight = 200;


    public Rect choiceTextRect;

    public Rect skillTypeRect;

    public Rect itemTransferTypeRect;
    public Rect itemTransferRect;

    public Rect requiredSkillTypeRect;
    public Rect requiredSkillNumberRect;

    public Rect npcRect;
    public Rect relationshipTypeChangeRect;
    public Rect relationshipNumberChangeRect;

    public Rect requiredRelationshiplevelRect;

    public override void Drag(Vector2 delta)
    {
        rect.position += delta;

        choiceTextRect.position += delta;

        skillTypeRect.position += delta;

        itemTransferTypeRect.position += delta;
        itemTransferRect.position += delta;

        requiredSkillTypeRect.position += delta;
        requiredSkillNumberRect.position += delta;

        npcRect.position += delta;
        relationshipTypeChangeRect.position += delta;
        relationshipNumberChangeRect.position += delta;

        requiredRelationshiplevelRect.position += delta;
    }

    public override void Draw()
    {
        EditorStyles.textField.wordWrap = true;
        inPoint.Draw(this);
        for (int i = 0; i < outPoints.Count; i++)
        {
            outPoints[i].Draw(this);
        }
        GUI.Box(rect, "", style);

        myChoice.choiceText = EditorGUI.TextField(choiceTextRect, myChoice.choiceText);

        myChoice.skillType = (Characteristics)EditorGUI.EnumPopup(skillTypeRect, "Skill increase", myChoice.skillType);

        myChoice.itemtransfer = (ItemTransfer)EditorGUI.EnumPopup(itemTransferTypeRect, "Give/Take item", myChoice.itemtransfer);
        if (myChoice.itemtransfer != ItemTransfer.Off)
        {
            myChoice.item = (ScriptableInventoryItem)EditorGUI.ObjectField(itemTransferRect, "Item to give/take", myChoice.item, typeof(ScriptableInventoryItem), false);
            //myChoice.itemID = EditorGUI.IntField(itemTransferRect, "Item ID", myChoice.itemID);
        }

        myChoice.requiredSkill = (Characteristics)EditorGUI.EnumPopup(requiredSkillTypeRect, "Required skill", myChoice.requiredSkill);
        if (myChoice.requiredSkill != Characteristics.None)
        {
            myChoice.requiredSkillNumber = EditorGUI.IntField(requiredSkillNumberRect, "Number of skillpoints", myChoice.requiredSkillNumber);
        }

        myChoice.affectedNPC = (ScriptableNpc)EditorGUI.ObjectField(npcRect, "AffectedNPC", myChoice.affectedNPC, typeof(ScriptableNpc), false);
        if (myChoice.affectedNPC)
        {
            myChoice.relationshipAttributeToChange = (RelationshipAttribute)EditorGUI.EnumPopup(relationshipTypeChangeRect, "Change", myChoice.relationshipAttributeToChange);
            myChoice.relationshipAttributeChangeNumber = EditorGUI.IntField(relationshipNumberChangeRect, "Change Number", myChoice.relationshipAttributeChangeNumber);

            myChoice.minimumRelationshiplevel = (RelationshipLevel)EditorGUI.EnumPopup(requiredRelationshiplevelRect, "Required relationship", myChoice.minimumRelationshiplevel);
        }
    }

    public ChoiceNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        rect = new Rect(position.x, position.y, boxWidth, boxHeight);

        float adaptiveY = rect.y + PADDING;
        float xLeft = rect.x + PADDING;
        float xMiddle = rect.x + boxWidth / 2;
        float halfWidth = boxWidth / 2 - (PADDING * 2);
        float fullWidth = boxWidth - (PADDING * 2);

        choiceTextRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT * 2);
        adaptiveY += TEXTSQUAREHEIGHT * 2;

        skillTypeRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        itemTransferTypeRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        itemTransferRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        requiredSkillTypeRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        requiredSkillNumberRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        npcRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        relationshipTypeChangeRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        relationshipNumberChangeRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;

        requiredRelationshiplevelRect = new Rect(xLeft, adaptiveY, fullWidth, TEXTSQUAREHEIGHT);
        adaptiveY += TEXTSQUAREHEIGHT;



        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, 0f);
        outPoints.Add(new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint, boxHeight / 2));

        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        typeOfNode = NodeType.ChoiceNode;
        allowedConnectionTypes.Add(NodeType.EventNode);
        allowedConnectionTypes.Add(NodeType.ScenarioEndNode);


        myChoice = new Choice("New Choice");
    }
}

//ScenarioEndNode
[System.Serializable]
public class ScenarioEndNode : Node
{
    //Base need
    //Sizes and Positions
    public const float
        boxWidth = 200,
        boxHeight = 70;

    //More squares
    public Rect newScenarioRect;
    public Rect newSceneRect;


    //Connected class or logic
    public Scenario nextScenario = null;
    public SceneAsset nextScene;

    public override void Drag(Vector2 delta)
    {
        //Base need
        rect.position += delta;

        newScenarioRect.position += delta;
        newSceneRect.position += delta;
    }

    public override void Draw()
    {
        //Base need
        EditorStyles.textField.wordWrap = true;
        inPoint.Draw(this);
        GUI.Box(rect, "", style);

        nextScenario = (Scenario)EditorGUI.ObjectField(newScenarioRect, nextScenario, typeof(Scenario), false);
        nextScene = (SceneAsset)EditorGUI.ObjectField(newSceneRect, nextScene, typeof(SceneAsset), false);
    }


    public ScenarioEndNode(Vector2 position, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        //Base need
        rect = new Rect(position.x, position.y, boxWidth, boxHeight);

        newScenarioRect = new Rect(rect.x + PADDING, rect.y + PADDING, rect.width - PADDING * 2, TEXTSQUAREHEIGHT);
        newSceneRect = new Rect(rect.x + PADDING, newScenarioRect.y + newScenarioRect.height + SPACING, rect.width - PADDING * 2, TEXTSQUAREHEIGHT);

        //Base need
        style = nodeStyle;
        inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint, 0f);

        //Base need
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;

        //Base need
        typeOfNode = NodeType.ScenarioEndNode;
    }
}
