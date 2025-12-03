using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
    
public class GameManager : MonoBehaviour
{

    public static event Action<Grabbable, bool> SendNote;
    public static event Action<bool> MissionResult;
    public static event Action<bool> GameEnded;

    [SerializeField] private bool retryUntilSuccess;
    [SerializeField] private int puzzlesToWin;
    [SerializeField] private List<TextAsset> puzzleFiles;
    [SerializeField] private List<string> puzzleNotes;

    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject pigeonPrefab;
    [SerializeField] private Transform pigeonStart;
    [SerializeField] private Transform pigeonEnd;

    [SerializeField] private string successText;
    [SerializeField] private string failureText;

    [SerializeField] private float timeToNextPigeon;

    private int currentPuzzle;
    private int puzzlesSucceded;

    #region Events

    private void OnEnable()
    {
        Pigeon.PigeonReady += OnPigeonReady;
        MissionGrabber.MissionCompleted += OnMissionCompleted;
    }

    private void OnDisable()
    {
        Pigeon.PigeonReady -= OnPigeonReady;
        MissionGrabber.MissionCompleted -= OnMissionCompleted;
    }

    #endregion

    #region Initialization

    private void Init()
    {
        currentPuzzle = 0;
        puzzlesSucceded = 0;

        StartCoroutine(CreatePigeon());
    }

    private IEnumerator CreatePigeon()
    {
        Pigeon pigeon = Instantiate(pigeonPrefab).GetComponent<Pigeon>();
        yield return null;
        pigeon.Init(CreateNote(puzzleNotes[0]), pigeonStart, pigeonEnd);
        yield break;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();
    }

    #endregion

    #region Game

    private Grabbable CreateNote(string _text)
    {
        GameObject _note = Instantiate(notePrefab);
        _note.GetComponentInChildren<TextMeshProUGUI>().text = _text;
        return _note.GetComponent<Grabbable>();
    }

    private void OnPigeonReady()
    {
        if (currentPuzzle == puzzleFiles.Count)
        {
            bool gameWon = puzzlesSucceded >= puzzlesToWin;
            OnGameEnd(gameWon);
            GameEnded?.Invoke(gameWon);
        }
        else StartCoroutine(PigeonTransitionRoutine(puzzleNotes[currentPuzzle], true));
    }

    private void OnMissionCompleted(MissionData data)
    {
        SpyBoard puzzle = new(puzzleFiles[currentPuzzle]);

        bool result;
        if (data.Name != puzzle.Name) result = false;
        else result = puzzle.EvaluatePath(data.Path);

        if (result) puzzlesSucceded++;

        if (!retryUntilSuccess || result) currentPuzzle++;

        MissionResult?.Invoke(result);

        StartCoroutine(PigeonTransitionRoutine(result ? successText : failureText, false));
    }

    private IEnumerator PigeonTransitionRoutine(string _noteText, bool _isMission)
    {
        yield return new WaitForSeconds(timeToNextPigeon);
        SendNote.Invoke(CreateNote(_noteText), _isMission);
    }

    private void OnGameEnd(bool result)
    {
        Debug.Log("You " + (result ? "win!" : "lose :("));
    }

    #endregion
}
