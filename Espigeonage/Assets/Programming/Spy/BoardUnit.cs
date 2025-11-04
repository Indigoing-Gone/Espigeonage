using UnityEngine;

public abstract class BoardUnit
{
    private Vector2 position;
    public abstract bool Update(Vector2 _playerPos);
}
