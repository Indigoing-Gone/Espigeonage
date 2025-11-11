using UnityEngine;

public abstract class BoardUnit
{
    protected Vector2Int position;
    public Vector2Int Position => position;

    public abstract void Reset();
    public abstract bool Update(Vector2Int _playerPos, SpaceType[,] _board);

}
