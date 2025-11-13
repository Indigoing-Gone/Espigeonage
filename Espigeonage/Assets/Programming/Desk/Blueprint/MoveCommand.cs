using UnityEngine;

public class MoveCommand : IBPCommand
{
    private Vector2Int position;
    private Blueprint blueprint;

    public MoveCommand(Blueprint _blueprint, Vector2Int _position)
    {
        blueprint = _blueprint;
        position = _position;
    }

    public void Execute() => blueprint.AddToPath(position);
    public void Undo() => blueprint.RemoveLastFromPath();
}
