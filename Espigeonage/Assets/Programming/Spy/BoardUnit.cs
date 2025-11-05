using UnityEngine;

public abstract class BoardUnit
{
    protected Vector2Int position;
    public Vector2 Positon => position;

    public abstract bool Update(Vector2Int _playerPos, SpaceType[,] _board);

}
