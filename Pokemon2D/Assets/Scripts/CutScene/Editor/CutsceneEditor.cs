using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cutscene))]
public class CutsceneEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var cutscene = target as Cutscene;

        if (GUILayout.Button("add Dialogue Action"))
         cutscene.AddAction(new DialogueAction());
      else if (GUILayout.Button("add Move Actor Action"))
            cutscene.AddAction(new MoveActorAction());

        base.OnInspectorGUI();
    }
}
