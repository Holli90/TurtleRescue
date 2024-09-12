using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(changeShader)), CanEditMultipleObjects]
public class customEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        changeShader script = (changeShader)target;

        if (GUILayout.Button("Change Position"))
        {
            script.showMats();
        }
    }
}
