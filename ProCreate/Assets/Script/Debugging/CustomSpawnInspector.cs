using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Breed))]
public class CustomSpawnInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Breed b = (Breed)target;
       
        if (GUILayout.Button("Breed")) {
            b.spawnChild();
         }
    }
}
