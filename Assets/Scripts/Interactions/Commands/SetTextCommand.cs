using System;

// command that sets the global monologue text
public class SetTextCommand : IInteractionCommand
{
    private readonly String text;
    private GameManager gm;

    public SetTextCommand(String text)
    {
        this.text = text;
        gm = GameManager.Instance;
    }

    public void Execute() => gm.SetText(text);
}
