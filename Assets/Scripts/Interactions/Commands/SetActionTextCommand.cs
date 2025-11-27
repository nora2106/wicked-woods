using System;

// command that sets the global action text
public class SetActionTextCommand : IInteractionCommand
{
    private readonly string text;
    private GameManager gm;

    public SetActionTextCommand(string text)
    {
        this.text = text;
        gm = GameManager.Instance;
    }

    public void Execute() => gm.SetActionText(text);
}
