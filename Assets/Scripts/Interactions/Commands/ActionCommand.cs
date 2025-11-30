using System;

// generic action command
public class ActionCommand : IInteractionCommand
{
    private readonly Action action;

    public ActionCommand(Action action)
    {
        this.action = action;
    }

    public void Execute() => action();
}
