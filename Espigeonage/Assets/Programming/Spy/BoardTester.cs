using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BoardTester : MonoBehaviour
{
    [SerializeField]
    private TextAsset testFile;

    [SerializeField]
    private List<Vector2> path;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpyBoard testBoard = new(testFile);
        Debug.Log("Path resulted in " + testBoard.EvaluatePath(path));
    }

}
