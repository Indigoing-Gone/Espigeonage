using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Grabbable))]
[RequireComponent(typeof(Draggable))]
public class BlueprintGrid : MonoBehaviour
{
    [SerializeField] private bool isDebug = false;

    [Header("Components")]
    [SerializeField] private InputReader input;
    private Grabbable grabbable;
    private BPCommandInvoker invoker = new();

    [SerializeField] private bool canBeModified;

    [Header("Data")]
    [SerializeField] private string locationName;
    public string LocationName => locationName;
    private int width;
    private int height;
    private Vector2Int startPos;
    [SerializeField] private List<Vector2Int> spyPath;

    public List<Vector2Int> SpyPath => spyPath;
    public Vector2Int SpyPosition => spyPath[^1];

    //Events
    public event Action<Vector2Int> PathContinued;
    public event Action PathRemoved;

    #region Input

    private void OnEnable()
    {
        input.PathEvent += HandlePathInput;
        grabbable.GrabbedStatus += LockModification;
    }

    private void OnDisable()
    {
        input.PathEvent -= HandlePathInput;
        grabbable.GrabbedStatus -= LockModification;
    }


    private void LockModification(bool _state)
    {
        canBeModified = false;
    }

    public void UnlockModification()
    {
        canBeModified = true;
    }

    public void HandlePathInput(Vector2 input)
    {
        if (!canBeModified) return;

        Vector2Int nextPos = spyPath[^1] + BoardUtils.TruncateVector2(input);
        if (IsValidMove(nextPos))
        {
            invoker.ExecuteCommand(new MoveCommand(this, nextPos));
        }
        else if (WouldUndo(nextPos))
        {
            invoker.UndoCommand();
        }
        //print(this);
    }

    #endregion

    private void Awake()
    {
        grabbable = GetComponent<Grabbable>();

        if (isDebug)
        {
            input.SetInspect();
            //Init();
        }
    }

    public void Init(Vector2Int _gridSize, Vector2Int _startPos)
    {
        startPos = _startPos;
        width = _gridSize.x;
        height = _gridSize.y;
        spyPath = new List<Vector2Int>() { startPos };
    }

    #region Path

    public Vector2Int GetSpyDirection()
    {
        if (spyPath.Count < 2) return Vector2Int.right;
        return spyPath[^1] - spyPath[^2]; 
    }

    public float GetSpyRotation()
    {
        if (spyPath.Count < 2) return 90;
        Vector2Int _direction = spyPath[^1] - spyPath[^2];

        return (_direction.x, _direction.y) switch
        {
            (1, 0) => 90,
            (-1, 0) => 270,
            (0, 1) => 0,
            (0, -1) => 180,
            _ => 0
        };
    }

    public void AddToPath(Vector2Int coordinate)
    {
        if (spyPath.Count > 0 && !BoardUtils.IsAdjacent(coordinate, spyPath[^1]))
        {
            throw new ArgumentException("Coordinate must be a valid addition to path");
        }
        spyPath.Add(coordinate);

        PathContinued?.Invoke(coordinate);
    }

    public void RemoveLastFromPath()
    {
        spyPath.RemoveAt(spyPath.Count - 1);

        PathRemoved?.Invoke();
    }

    // Returns if next position would undo previous move
    public bool WouldUndo(Vector2Int next)
    {
        return spyPath.Count > 1 && next == spyPath[^2];
    }

    public bool IsValidMove(Vector2Int next)
    {
        // If not in bounds, return false
        if (!BoardUtils.InBounds(BoardUtils.ToRowMajor(next, height), width, height)) return false;

        // If path is empty, return true
        if (spyPath.Count == 0) return true;

        // If next position is not adjacent to end of path, return false
        if (!BoardUtils.IsAdjacent(next, spyPath[^1])) return false;

        // If next pos would cause path to wrap back on itself, return false
        if (WouldUndo(next)) return false;

        // If reached this point, next pos is valid
        return true;
    }

    #endregion

    #region String

    private char GetSpyChar()
    {
        Vector2Int dir = GetSpyDirection();
        return (dir.x, dir.y) switch
        {
            (1, 0) => '>',
            (-1, 0) => '<',
            (0, 1) => '^',
            (0, -1) => 'v',
            _ => throw new ArgumentException("INVALID SPY DIRECTION")
        };
    }

    public override string ToString()
    {
        // Create char matrix with BP dimensions
        char[,] _boardStr = new char[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                _boardStr[i, j] = '-';
            }
        }

        // Add path to char matrix
        for (int i = 0; i < spyPath.Count; i++)
        {
            Vector2Int coord = BoardUtils.ToRowMajor(spyPath[i], height);
            if (i < spyPath.Count - 1) _boardStr[coord.x, coord.y] = '+';
            else _boardStr[coord.x, coord.y] = GetSpyChar();
        }

        // Construct string from char matrix
        StringBuilder sb = new();
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                sb.Append(_boardStr[i, j]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }

    #endregion
}
