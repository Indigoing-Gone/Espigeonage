//using System;
//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//public class BlueprintLogic : MonoBehaviour
//{
//    [Header("Components")]
//    private BPCommandInvoker invoker;

//    [Header("Data")]
//    private Vector2Int gridSize;
//    [SerializeField] private List<Vector2Int> spyPath;
//    public List<Vector2Int> SpyPath => spyPath;

//    //Events
//    public event Action<Vector2Int> PathContinued;
//    public event Action PathRemoved;

//    public BlueprintLogic(Vector2Int _gridSize, Vector2Int _startPos)
//    {
//        invoker = new();
//        gridSize = _gridSize;
//        spyPath = new List<Vector2Int>() { _startPos };
//    }

//    public void ProcessPath(Vector2 input)
//    {
//        Vector2Int nextPos = spyPath[^1] + BoardUtils.TruncateVector2(input);
//        if (IsValidMove(nextPos)) invoker.ExecuteCommand(new MoveCommand(this, nextPos));
//        else if (WouldUndo(nextPos)) invoker.UndoCommand();
//    }

//    // Returns if next position would undo previous move
//    public bool WouldUndo(Vector2Int next) => spyPath.Count > 1 && next == spyPath[^2];

//    public bool IsValidMove(Vector2Int next)
//    {
//        // If not in bounds, return false
//        if (!BoardUtils.InBounds(BoardUtils.ToRowMajor(next, gridSize.y), gridSize.x, gridSize.y)) return false;

//        // If path is empty, return true
//        if (spyPath.Count == 0) return true;

//        // If next position is not adjacent to end of path, return false
//        if (!BoardUtils.IsAdjacent(next, spyPath[^1])) return false;

//        // If next pos would cause path to wrap back on itself, return false
//        if (WouldUndo(next)) return false;

//        // If reached this point, next pos is valid
//        return true;
//    }

//    public void AddToPath(Vector2Int coordinate)
//    {
//        if (spyPath.Count > 0 && !BoardUtils.IsAdjacent(coordinate, spyPath[^1])) => throw new ArgumentException("Coordinate must be a valid addition to path");
//        spyPath.Add(coordinate);

//        PathContinued?.Invoke(coordinate);
//    }

//    public void RemoveLastFromPath()
//    {
//        spyPath.RemoveAt(spyPath.Count - 1);

//        PathRemoved?.Invoke();
//    }

//    public Direction GetSpyDirection()
//    {
//        if (spyPath.Count < 2) return Direction.Up;
//        Vector2Int _directionVector = spyPath[^1] - spyPath[^2];

//        return (_directionVector.x, _directionVector.y) switch
//        {
//            (0, 1) => Direction.Up,
//            (1, 0) => Direction.Right,
//            (0, -1) => Direction.Down,
//            (-1, 0) => Direction.Left,
//            _ => throw new ArgumentException("INVALID SPY DIRECTION")
//        };
//    }

//    #region String

//    private char GetSpyChar()
//    {
//        Direction dir = GetSpyDirection();
//        return (dir) switch
//        {
//            Direction.Left => '>',
//            Direction.Right => '<',
//            Direction.Up => '^',
//            Direction.Down => 'v',
//            _ => throw new ArgumentException("INVALID SPY DIRECTION")
//        };
//    }

//    public override string ToString()
//    {
//        // Create char matrix with BP dimensions
//        char[,] _boardStr = new char[height, width];
//        for (int i = 0; i < height; i++)
//        {
//            for (int j = 0; j < width; j++)
//            {
//                _boardStr[i, j] = '-';
//            }
//        }

//        // Add path to char matrix
//        for (int i = 0; i < spyPath.Count; i++)
//        {
//            Vector2Int coord = BoardUtils.ToRowMajor(spyPath[i], height);
//            if (i < spyPath.Count - 1) _boardStr[coord.x, coord.y] = '+';
//            else _boardStr[coord.x, coord.y] = GetSpyChar();
//        }

//        // Construct string from char matrix
//        StringBuilder sb = new();
//        for (int i = 0; i < height; i++)
//        {
//            for (int j = 0; j < width; j++)
//            {
//                sb.Append(_boardStr[i, j]);
//            }
//            sb.Append('\n');
//        }
//        return sb.ToString();
//    }

//    #endregion
//}