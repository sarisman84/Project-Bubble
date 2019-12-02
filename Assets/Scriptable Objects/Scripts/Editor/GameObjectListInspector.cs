using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameObjectList))]
public class InspectorCustomizer : Editor //allows for children of elements to be visivle in the editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("list"), true);
        serializedObject.ApplyModifiedProperties();
    }
}
