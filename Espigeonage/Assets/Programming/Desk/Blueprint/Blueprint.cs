using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum Direction
{
    Up = 0,
    Right = 90,
    Down = 180,
    Left = 270
}

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Draggable))]
[RequireComponent(typeof(BlueprintVisuals))]
public class Blueprint : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputReader input;
    private BPCommandInvoker invoker;
    private BlueprintVisuals visuals;
    private Grabbable grabbable;

    [Header("Data")]
    [SerializeField] private string locationName;
    public string LocationName => locationName;
    [SerializeField] private Vector2Int gridSize;

    [Header("Spy")]
    [SerializeField] private Vector2Int spyStartPos;
    [SerializeField] private List<Vector2Int> spyPath;
    public List<Vector2Int> SpyPath => spyPath;
    [SerializeField] private bool canBeModified;

    private void OnEnable()
    {
        input.PathEvent += ProcessPath;
        grabbable.GrabbedStatus += LockModification;
    }

    private void OnDisable()
    {
        input.PathEvent -= ProcessPath;
        grabbable.GrabbedStatus -= LockModification;
    }

    private void OnValidate()
    {
        visuals = GetComponent<BlueprintVisuals>();
        visuals.SetGridSize(gridSize);
    }

    private void Awake()
    {
        visuals = GetComponent<BlueprintVisuals>();
        invoker = new();
        grabbable = GetComponent<Grabbable>();
    }

    private void Start()
    {
        spyPath = new List<Vector2Int>() { spyStartPos };

        visuals.SetGridSize(gridSize);
        visuals.AddTrailPosition(spyStartPos);
        visuals.UpdateVisuals(spyPath[^1], (float)GetSpyDirection());
    }

    public void ProcessPath(Vector2 _input)
    {
        if (!canBeModified) return;

        Vector2Int nextPos = spyPath[^1] + BoardUtils.TruncateVector2(_input);
        if (IsValidMove(nextPos)) invoker.ExecuteCommand(new MoveCommand(this, nextPos));
        else if (WouldUndo(nextPos)) invoker.UndoCommand();
    }

    public bool IsValidMove(Vector2Int next)
    {
        // If not in bounds, return false
        if (!BoardUtils.InBounds(BoardUtils.ToRowMajor(next, gridSize.y), gridSize.x, gridSize.y)) return false;

        // If path is empty, return true
        if (spyPath.Count == 0) return true;

        // If next position is not adjacent to end of path, return false
        if (!BoardUtils.IsAdjacent(next, spyPath[^1])) return false;

        // If next pos would cause path to wrap back on itself, return false
        if (WouldUndo(next)) return false;

        // If reached this point, next pos is valid
        return true;
    }

    // Returns if next position would undo previous move
    public bool WouldUndo(Vector2Int next) => spyPath.Count > 1 && next == spyPath[^2];

    public void AddToPath(Vector2Int _coord)
    {
        if (!BoardUtils.IsAdjacent(_coord, spyPath[^1]) && spyPath.Count > 0)
            throw new ArgumentException("Coordinate must be a valid addition to path");
        spyPath.Add(_coord);

        visuals.AddTrailPosition(_coord);
        visuals.UpdateVisuals(spyPath[^1], (float)GetSpyDirection());

        SoundManager.Instance.PlaySFX(SoundManager.SFXType.MOVE_SPY, transform.position);
    }

    public void RemoveLastFromPath()
    {
        spyPath.RemoveAt(spyPath.Count - 1);

        visuals.RemoveTrailPosition();
        visuals.UpdateVisuals(spyPath[^1], (float)GetSpyDirection());

        SoundManager.Instance.PlaySFX(SoundManager.SFXType.UNDO, transform.position);
    }

    public Direction GetSpyDirection()
    {
        if (spyPath.Count < 2) return Direction.Up;
        Vector2Int _directionVector = spyPath[^1] - spyPath[^2];

        return (_directionVector.x, _directionVector.y) switch
        {
            (0, 1) => Direction.Up,
            (1, 0) => Direction.Right,
            (0, -1) => Direction.Down,
            (-1, 0) => Direction.Left,
            _ => throw new ArgumentException("INVALID SPY DIRECTION")
        };
    }

    private void LockModification(bool _state) => canBeModified = false;
    public void UnlockModification() => canBeModified = true;

    #region String

    private char GetSpyChar()
    {
        Direction dir = GetSpyDirection();
        return (dir) switch
        {
            Direction.Left => '>',
            Direction.Right => '<',
            Direction.Up => '^',
            Direction.Down => 'v',
            _ => throw new ArgumentException("INVALID SPY DIRECTION")
        };
    }

    public override string ToString()
    {
        // Create char matrix with BP dimensions
        char[,] _boardStr = new char[gridSize.y, gridSize.x];
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                _boardStr[i, j] = '-';
            }
        }

        // Add path to char matrix
        for (int i = 0; i < spyPath.Count; i++)
        {
            Vector2Int coord = BoardUtils.ToRowMajor(spyPath[i], gridSize.y);
            if (i < spyPath.Count - 1) _boardStr[coord.x, coord.y] = '+';
            else _boardStr[coord.x, coord.y] = GetSpyChar();
        }

        // Construct string from char matrix
        StringBuilder sb = new();
        for (int i = 0; i < gridSize.y; i++)
        {
            for (int j = 0; j < gridSize.x; j++)
            {
                sb.Append(_boardStr[i, j]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    #endregion
}
