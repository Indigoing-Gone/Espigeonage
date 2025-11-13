using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoardTester : MonoBehaviour
{
    [SerializeField] private TextAsset testFile;
    [SerializeField] private List<Vector2Int> path;
    [SerializeField] private Blueprint blueprint;

    private SpyBoard board;
    public void Parse()
    {
        board = new(testFile);
    }

    public void TestFromPath()
    {
        Debug.Log("Path resulted in " + board.EvaluatePath(path));
    }

    public void TestFromBlueprint()
    {
        if (blueprint != null) Debug.Log("Path resulted in " + board.EvaluatePath(blueprint.SpyPath));
    }
}
