using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ReturnEffect))]
public class ReturnEffectEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ReturnEffect re = (ReturnEffect)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Editor Commands", EditorStyles.boldLabel);
        if (GUILayout.Button("Return")){
            re.StartCoroutine("RestoredToInialPos");
        }
    }
}
