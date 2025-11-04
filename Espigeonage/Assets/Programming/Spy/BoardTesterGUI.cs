using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardTester))]
public class BoardTesterGUI : Editor
{
    public override void OnInspectorGUI()
    {
        BoardTester tester = (BoardTester)target;

        DrawDefaultInspector();
        if (GUILayout.Button("Parse File"))
        {
            tester.Parse();
        }
        if (GUILayout.Button("Test Path"))
        {
            tester.Test();
        }
    }
}
