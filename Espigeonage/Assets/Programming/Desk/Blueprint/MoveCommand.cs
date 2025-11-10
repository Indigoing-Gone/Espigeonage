using UnityEngine;

public class MoveCommand : IBPCommand
{
    private Vector2Int position;
    private BPGrid grid;

    public MoveCommand(BPGrid _grid, Vector2Int _position)
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
