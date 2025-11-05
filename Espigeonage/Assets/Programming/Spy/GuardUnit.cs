using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GuardUnit : BoardUnit
{
    private int range;
    private Vector2Int[] patrolPath;
    private int patrolIndex;
    private Vector2Int direction;

    private bool firstTurn = true;
    
    public GuardUnit(List<Vector2Int> _patrolPath, char _direction, int _range)
    {
        patrolPath = _patrolPath.ToArray();
        position = patrolPath[0];
        range = _range;

        direction = _direction switch
        {
            'U' => new Vector2Int(-1, 0),
            'D' => new Vector2Int(1, 0),
            'L' => new Vector2Int(0, -1),
            'R' => new Vector2Int(0, 1),
            _ => throw new ArgumentException(_direction + " NOT A VALID DIRECTION"),
        };
    }

    // Searches for player at position in direction with range, returns if player was found
    private bool Search(Vector2Int _playerPos, SpaceType[,] _board)
    {
        if (position == _playerPos) return true;

        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkPos = position + direction * i;
            if (checkPos == _playerPos) return true;
            if (!SpyBoard.InBounds(checkPos, _board.GetLength(1), _board.GetLength(0))) return false;
            if (_board[checkPos[0], checkPos[1]] == SpaceType.WALL) return false;       
        }
        return false;
    }

    public override bool Update(Vector2Int _playerPos, SpaceType[,] _board)
    {
        if (patrolPath.Length == 1 || firstTurn)
        {
            firstTurn = false;
            return !Search(_playerPos, _board);
        }

        int prevIndex = patrolIndex;
        if (patrolIndex == patrolPath.Length - 1) patrolIndex = 0;
        else patrolIndex++;

        position = patrolPath[patrolIndex];
        direction = position - patrolPath[prevIndex];
        return !Search(_playerPos, _board);
    }
}
