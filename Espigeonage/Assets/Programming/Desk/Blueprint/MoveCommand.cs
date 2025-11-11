using UnityEngine;

public class MoveCommand : IBPCommand
{
    private Vector2Int position;
    private BlueprintData grid;

    public MoveCommand(BlueprintData _grid, Vector2Int _position)
    {
        grid = _grid;
        position = _position;
    }

    public void Execute()
    {
        grid.AddToPath(position);
    }

    public void Undo()
    {
        grid.RemoveLastFromPath();
    }

}
