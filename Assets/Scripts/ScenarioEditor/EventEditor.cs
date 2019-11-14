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

    private static ConnectionPoint selectedInPoint;
    private static ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    public static Scenario openScenario;


    public static void OpenWindow(Scenario input)
    {
        EventEditor window = GetWindow<EventEditor>();
        window.titleContent = new GUIContent("Event Window - " + input.name);
        openScenario = input;
        if (input.editorNodes.Count != 0)
        {
            LoadEvent();
            GUI.changed = true;
        }
        else
        {
            allNodes.Clear();
            allConnections.Clear();
            GUI.changed = true;
        }
    }
    private void OnEnable()
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
                allNodes[i].Draw();
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
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            GUI.changed = true;
        }

        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center - Vector2.left * 50f,
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
    }

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
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
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
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
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
        if (selectedInPoint.node is EventNode && selectedOutPoint.node is EventNode)
        {
            return;
        }
        if (selectedInPoint.node is ChoiceNode && selectedOutPoint.node is ChoiceNode)
        {
            return;
        }

        if (allConnections == null)
        {
            allConnections = new List<Connection>();
        }


        allConnections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));

        if (selectedOutPoint.node is EventNode)
        {
            EventNode eventNode = selectedOutPoint.node as EventNode;
            ChoiceNode choiceNode = selectedInPoint.node as ChoiceNode;

            eventNode.myEvent.choices.Add(choiceNode.myChoice);
        }
        else if (selectedOutPoint.node is ChoiceNode)
        {
            ChoiceNode choiceNode = selectedOutPoint.node as ChoiceNode;
            EventNode eventNode = selectedInPoint.node as EventNode;

            choiceNode.myChoice.nextEvent = eventNode.myEvent;
        }
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
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
        Node startNode = null;

        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] is EventNode)
            {
                EventNode test = allNodes[i] as EventNode;
                
                if (test.isStartNode)
                {
                    if (startNode == null)
                    {
                        startNode = allNodes[i];
                        openScenario.startEvent = test.myEvent;
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

        //Clear old lists and add the active nodes and connections to them instead
        openScenario.editorNodes.Clear();
        openScenario.editorConnections.Clear();
        for (int i = 0; i < allNodes.Count; i++)
        {
            openScenario.editorNodes.Add(allNodes[i]);
        }
        for (int i = 0; i < allConnections.Count; i++)
        {
            openScenario.editorConnections.Add(allConnections[i]);
        }

        //Save the information in the nodes/events
        

        ////Save the information from the connections/choices
        //for (int i = 0; i < allConnections.Count; i++)
        //{
        //    for (int j = 0; j < allNodes.Count; j++)
        //    {
        //        if (allConnections[i].inPoint.node == allNodes[j])
        //        {
        //            allNodes[j].myEvent.choices.add
        //        }
        //    }
        //}
    }

    private static void LoadEvent()
    {
        allNodes.Clear();
        allConnections.Clear();
        allNodes.AddRange(openScenario.editorNodes);
        allConnections.AddRange(openScenario.editorConnections);
    }
}