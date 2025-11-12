using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static event Action<bool> MissionResult;
    public static event Action<bool> GameEnded;

    [SerializeField] private int puzzlesToWin;
    [SerializeField] private List<TextAsset> puzzleFiles;

    private int currentPuzzle;
    private int puzzlesSucceded;

    #region Events

    private void OnEnable()
    {
        MissionGrabber.MissionCompleted += OnMissionCompleted;
    }

    private void OnDisable()
    {
        MissionGrabber.MissionCompleted -= OnMissionCompleted;
    }

    #endregion

    #region Initialization

    private void Init()
    {
        currentPuzzle = 0;
        puzzlesSucceded = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    #endregion

    #region Game

    private void OnMissionCompleted(MissionData data)
    {
        SpyBoard puzzle = new(puzzleFiles[currentPuzzle]);

        bool result;
        if (data.Name != puzzle.Name) result = false;
        else result = puzzle.EvaluatePath(data.Path);

        if (result) puzzlesSucceded++;
        MissionResult?.Invoke(result);

        Debug.Log("Mission was a " + (result ? "success." : "failure."));

        currentPuzzle++;

        if (currentPuzzle == puzzleFiles.Count)
        {
            bool gameWon = puzzlesSucceded >= puzzlesToWin;
            OnGameEnd(gameWon);
            GameEnded?.Invoke(gameWon);
        }
    }

    private void OnGameEnd(bool result)
    {
        Debug.Log("You " + (result ? "win!" : "lose :("));
    }

    #endregion
}
