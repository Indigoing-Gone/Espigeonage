using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoardTester : MonoBehaviour
{
    [SerializeField]
    private TextAsset testFile;

    [SerializeField]
    private List<Vector2Int> path;

    [SerializeField]
    private BlueprintData grid;

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
        if (grid != null) Debug.Log("Path resulted in " + board.EvaluatePath(grid.SpyPath));
    }
}
