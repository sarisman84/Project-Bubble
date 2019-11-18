using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//http://gram.gs/gramlog/creating-node-based-editor-unity/

public enum ConnectionPointType { In, Out }

public class EventEditor : EditorWindow
{
    public static List<Node> allNodes = new List<Node>();
    private static List<Connection> allConnections = new List<Connection>();

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private static ConnectionPoint selectedNodeInPoint;
    private static ConnectionPoint selectedNodeOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    public static Scenario openScenario;

    //private static EventEditor instance;


    public static void OpenWindow(Scenario input)
    {
        EventEditor window = GetWindow<EventEditor>();
        window.titleContent = new GUIContent("Event Window - " + input.name);
        openScenario = input;
        allNodes.Clear();
        allConnections.Clear();
        //if (input.editorEventNodes.Count != 0)
        //{
        //    LoadEvent();
        //    GUI.changed = true;
        //}
        //else
        //{
        //    allNodes.Clear();
        //    allConnections.Clear();
        //    GUI.changed = true;
        //}
    }


    private void OnEnable()
    {
        //instance = this;

        SetStyles();
    }

    private void SetStyles()
    {
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }


    private void OnGUI()
    {
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();
        DrawButtons();

        DrawConnectionLine(UnityEngine.Event.current);

        ProcessNodeEvents(UnityEngine.Event.current);
        ProcessEvents(UnityEngine.Event.current);

        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawNodes()
    {
        if (allNodes != null)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                if (allNodes[i] is EventNode)
                {
                    EventNode node = allNodes[i] as EventNode;
                    node.Draw();
                }
                else if (allNodes[i] is ChoiceNode)
                {
                    ChoiceNode node = allNodes[i] as ChoiceNode;
                    node.Draw();
                }
                //allNodes[i].Draw();
            }
        }
    }
    private void DrawConnections()
    {
        if (allConnections != null)
        {
            for (int i = 0; i < allConnections.Count; i++)
            {
                allConnections[i].Draw();
            }
        }
    }
    private void DrawConnectionLine(UnityEngine.Event e)
    {
        if (selectedNodeInPoint != null && selectedNodeOutPoint == null)
        {
            Handles.DrawBezier(
                selectedNodeInPoint.rect.center,
                e.mousePosition,
                selectedNodeInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedNodeOutPoint != null && selectedNodeInPoint == null)
        {
            Handles.DrawBezier(
                selectedNodeOutPoint.rect.center,
                e.mousePosition,
                selectedNodeOutPoint.rect.center - Vector2.left * 50f,
                e.mousePosition + Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawButtons()
    {
        if (GUILayout.Button("Save"))
        {
            SaveEvent();
        }

        if (GUILayout.Button("Load Data"))
        {
            LoadEvent();
        }

        //if (GUILayout.Button("Check if saved to disk"))
        //{
        //    CheckIfDirty();
        //}
    }

    //public void CheckIfDirty()
    //{
    //    Debug.Log("Is it dirty" + EditorUtility.IsDirty(openScenario));
    //}

    private void ProcessEvents(UnityEngine.Event e)
    {
        drag = Vector2.zero;

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }
    private void ProcessNodeEvents(UnityEngine.Event e)
    {
        if (allNodes != null)
        {
            for (int i = allNodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = allNodes[i].ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    //Rightclicking
    private void ProcessContextMenu(Vector2 mousePosition)
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add event node"), false, () => OnClickAddEventNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add choice node"), false, () => OnCLickAddChoiceNode(mousePosition));

        genericMenu.ShowAsContext();
    }

    //Create Eventnode
    private void OnClickAddEventNode(Vector2 mousePosition)
    {
        if (allNodes == null)
        {
            allNodes = new List<Node>();
        }

        allNodes.Add(new EventNode(mousePosition, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }

    //Create Choicenode
    private void OnCLickAddChoiceNode(Vector2 mousePosition)
    {
        if (allNodes == null)
        {
            allNodes = new List<Node>();
        }

        allNodes.Add(new ChoiceNode(mousePosition, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    }


    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        Debug.Log("Clicked on Inpoint");
        selectedNodeInPoint = inPoint;

        if (selectedNodeOutPoint != null)
        {
            if (selectedNodeOutPoint.node != selectedNodeInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickOutPoint(ConnectionPoint outPoint)
    {
        Debug.Log("Clicked on Outpoint");
        selectedNodeOutPoint = outPoint;

        if (selectedNodeInPoint != null)
        {
            if (selectedNodeOutPoint.node != selectedNodeInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    //Remove connection
    private void OnClickRemoveConnection(Connection connection)
    {
        allConnections.Remove(connection);
    }

    //Create connection
    private void CreateConnection()
    {
        //If it is of the same type, dont allow connection
        //if (selectedInPoint.node is EventNode && selectedOutPoint.node is EventNode)
        if (selectedNodeInPoint.node.typeOfNode == selectedNodeOutPoint.node.typeOfNode)
        {
            return;
        }
        //if (selectedInPoint.node is ChoiceNode && selectedOutPoint.node is ChoiceNode)
        //{
        //    return;
        //}

        if (allConnections == null)
        {
            allConnections = new List<Connection>();
        }


        allConnections.Add(new Connection(selectedNodeOutPoint, selectedNodeInPoint, OnClickRemoveConnection));

        //if (selectedOutPoint.node is EventNode)
        //{
        //    EventNode eventNode = selectedOutPoint.node as EventNode;
        //    ChoiceNode choiceNode = selectedInPoint.node as ChoiceNode;

        //    eventNode.myEvent.choices.Add(choiceNode.myChoice);
        //}
        //else if (selectedOutPoint.node is ChoiceNode)
        //{
        //    ChoiceNode choiceNode = selectedOutPoint.node as ChoiceNode;
        //    EventNode eventNode = selectedInPoint.node as EventNode;

        //    choiceNode.myChoice.nextEvent = eventNode.myEvent;
        //}
    }

    private void ClearConnectionSelection()
    {
        selectedNodeInPoint = null;
        selectedNodeOutPoint = null;
    }
    private void OnClickRemoveNode(Node node)
    {
        if (allConnections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < allConnections.Count; i++)
            {
                for (int j = 0; j < node.outPoints.Count; j++)
                {
                    if (allConnections[i].outPoint == node.outPoints[j])
                    {
                        connectionsToRemove.Add(allConnections[i]);
                    }
                }
                if (allConnections[i].inPoint == node.inPoint)
                {
                    connectionsToRemove.Add(allConnections[i]);
                }
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
            {
                allConnections.Remove(connectionsToRemove[i]);
            }

            connectionsToRemove = null;
        }

        //if (node is EventNode)
        //{
        //    EventNode removalNode = node as EventNode;

        //    //removalnode.remove choice
        //}
        //else if (node is ChoiceNode)
        //{
        //    ChoiceNode removalNode = node as ChoiceNode;

        //    //removalnode remove myevent
        //}

        allNodes.Remove(node);
    }
    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        if (allNodes != null)
        {
            for (int i = 0; i < allNodes.Count; i++)
            {
                allNodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    public void SaveEvent()
    {
        if (!openScenario)
        {
            Debug.LogError("No open scenario");
            return;
        }

        //Find startnode and secure that there is only one
        EventNode startNode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] is EventNode)
            {
                EventNode test = allNodes[i] as EventNode;

                if (test.isStartNode)
                {
                    if (startNode == null)
                    {
                        startNode = test;
                    }
                    else
                    {
                        Debug.LogError("2 nodes marked as startnode, save will be corrupted");
                        return;
                    }
                }
            }
        }

        if (startNode == null)
        {
            Debug.LogError("No startnode found, Save Failed");
            return;
        }

        //SAVE
        //NEW TRY START FROM STARTNODE IN EDITOR AND GO FORTH
        openScenario.startEvent = startNode.myEvent;
        openScenario.startEvent.choices.Clear();

        SaveOutputsFromEvent(openScenario.startEvent, startNode as EventNode);
        //END NEW TRY


        //Clear old lists and add the active nodes and connections to them instead
        openScenario.editorConnections.Clear();
        openScenario.editorChoiceNodes.Clear();
        openScenario.editorEventNodes.Clear();

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] is EventNode)
            {
                EventNode node = allNodes[i] as EventNode;
                openScenario.editorEventNodes.Add(node);
            }
            else if (allNodes[i] is ChoiceNode)
            {
                ChoiceNode node = allNodes[i] as ChoiceNode;
                openScenario.editorChoiceNodes.Add(node);
            }
        }
        for (int i = 0; i < allConnections.Count; i++)
        {
            openScenario.editorConnections.Add(allConnections[i]);
        }



        



        EditorUtility.SetDirty(openScenario);

        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        Debug.Log("Save successfull");
    }

    //New method that saves the output from events
    private void SaveOutputsFromEvent(Event inputEvent, EventNode eventNode)
    {
        List<ChoiceNode> newChoiceNodes = new List<ChoiceNode>();

        for (int i = 0; i < allConnections.Count; i++)
        {
            for (int j = 0; j < eventNode.outPoints.Count; j++)
            {
                if (allConnections[i].inPoint.id == eventNode.outPoints[j].id)
                {
                    switch (allConnections[i].outPoint.node.typeOfNode)
                    {
                        case Node.NodeType.ChoiceNode:
                            ChoiceNode newChhoiceNode = allConnections[i].outPoint.node as ChoiceNode;
                            newChoiceNodes.Add(newChhoiceNode);
                            inputEvent.choices.Add(newChhoiceNode.myChoice);
                            Debug.Log("Choice added and saved");
                            break;
                        case Node.NodeType.EventNode:
                            Debug.LogError("EventNode connected to EventNode in Save");
                            break;
                    }
                }
            }
        }

        //Continue save if found outputs
        for (int i = 0; i < newChoiceNodes.Count; i++)
        {
            SaveOutputsFromChoiceNode(inputEvent.choices[i], newChoiceNodes[i]);
        }
    }

    //New method that saves the output from choicenodes
    private void SaveOutputsFromChoiceNode(Choice inputChoice, ChoiceNode choiceNode)
    {
        EventNode newNode = null;
        Event newEvent = null;
        for (int i = 0; i < allConnections.Count; i++)
        {
            for (int j = 0; j < choiceNode.outPoints.Count; j++)
            {
                if (allConnections[i].inPoint.id == choiceNode.outPoints[j].id)
                {
                    switch (allConnections[i].outPoint.node.typeOfNode)
                    {
                        case Node.NodeType.ChoiceNode:
                            Debug.LogError("Choicenode connected to Choicenode in Save");
                            break;
                        case Node.NodeType.EventNode:
                            newNode = allConnections[i].outPoint.node as EventNode;
                            newNode.myEvent.choices.Clear();
                            newEvent = newNode.myEvent;
                            inputChoice.nextEvent = newEvent;
                            Debug.Log("Event added and saved");
                            break;
                    }
                }
            }
        }

        //Continue save if found output
        if (newNode != null && newEvent != null)
        {
            SaveOutputsFromEvent(newEvent, newNode);
        }
    }


    //private static void LoadEvent()
    //{
    //    allNodes.Clear();
    //    allConnections.Clear();

    //    //Add all nodes from editor event nodes
    //    for (int i = 0; i < openScenario.editorEventNodes.Count; i++)
    //    {
    //        allNodes.Add(openScenario.editorEventNodes[i]);
    //    }

    //    //Add all nodes from editor choice nodes
    //    for (int i = 0; i < openScenario.editorChoiceNodes.Count; i++)
    //    {
    //        allNodes.Add(openScenario.editorChoiceNodes[i]);
    //    }


    //    //Add all editor connections
    //    for (int i = 0; i < openScenario.editorConnections.Count; i++)
    //    {
    //        allConnections.Add(openScenario.editorConnections[i]);
    //    }

    //    //Add connectionpoints to editor connections
    //    for (int i = 0; i < allConnections.Count; i++)
    //    {
    //        for (int j = 0; j < allNodes.Count; j++)
    //        {
    //            //Set inpoints
    //            if (allConnections[i].inPoint.id == allNodes[j].inPoint.id)
    //            {
    //                allConnections[i].inPoint = allNodes[j].inPoint;
    //            }

    //            //Set outpoints
    //            for (int k = 0; k < allNodes[j].outPoints.Count; k++)
    //            {
    //                if (allConnections[i].outPoint.id == allNodes[j].outPoints[k].id)
    //                {
    //                    allConnections[i].outPoint = allNodes[j].outPoints[k];
    //                }
    //            }
    //        }
    //    }
    //}

    private void LoadEvent()
    {
        allNodes.Clear();
        allConnections.Clear();

        //Add all nodes from editor event nodes
        for (int i = 0; i < openScenario.editorEventNodes.Count; i++)
        {
            allNodes.Add(openScenario.editorEventNodes[i]);
        }

        //Add all nodes from editor choice nodes
        for (int i = 0; i < openScenario.editorChoiceNodes.Count; i++)
        {
            allNodes.Add(openScenario.editorChoiceNodes[i]);
        }


        //Add all editor connections
        for (int i = 0; i < openScenario.editorConnections.Count; i++)
        {
            allConnections.Add(openScenario.editorConnections[i]);
        }

        //Add connectionpoints to editor connections
        for (int i = 0; i < allConnections.Count; i++)
        {
            allConnections[i].OnClickRemoveConnection = OnClickRemoveConnection;
            for (int j = 0; j < allNodes.Count; j++)
            {
                //Setup inpoints
                if (allConnections[i].outPoint.id == allNodes[j].inPoint.id)
                {
                    allConnections[i].outPoint = allNodes[j].inPoint;
                    allNodes[j].inPoint.OnClickConnectionPoint = OnClickInPoint;
                }

                //Set outpoints
                for (int k = 0; k < allNodes[j].outPoints.Count; k++)
                {
                    if (allConnections[i].inPoint.id == allNodes[j].outPoints[k].id)
                    {
                        allConnections[i].inPoint = allNodes[j].outPoints[k];
                        allNodes[j].outPoints[k].OnClickConnectionPoint = OnClickOutPoint;
                    }
                }
            }
        }

        GUI.changed = true;

    }
}