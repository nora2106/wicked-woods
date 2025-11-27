using System;
using UnityEngine;

// generic action command
public class ActionCommand : IInteractionCommand
{
    private readonly Action action;
    private GameManager gm;

    public ActionCommand(Action action)
    {
        this.action = action;
    }

    public void Execute() => action();
}
