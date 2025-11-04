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
    private SpyBoard board;

    public void Parse()
    {
        board = new(testFile);
    }

    public void Test()
    {
        Debug.Log("Path resulted in " + board.EvaluatePath(path));
    }
}
