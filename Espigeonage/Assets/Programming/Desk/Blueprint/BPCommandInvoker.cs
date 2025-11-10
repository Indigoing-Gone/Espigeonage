using System.Collections.Generic;
using UnityEngine;

public class BPCommandInvoker
{
    private Stack<IBPCommand> undoStack = new();
    private Stack<IBPCommand> redoStack = new();

    public void ExecuteCommand(IBPCommand command)
    {
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
    }

    public void UndoCommand()
    {
        if (undoStack.Count > 0)
        {
            IBPCommand command = undoStack.Pop();
            redoStack.Push(command);
            command.Undo();
        }
    }

    public void RedoCommand()
    {
        if (redoStack.Count > 0)
        {
            IBPCommand command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }    
    }

    public void ClearCommands()
    {
        undoStack.Clear();
        redoStack.Clear();
    }
}
