using System;

// command that changes the current scene
public class ChangeSceneCommand : IInteractionCommand
{
    private readonly string scene;
    private GameManager gm;

    public ChangeSceneCommand(string scene)
    {
        this.scene = scene;
        gm = GameManager.Instance;
    }

    public void Execute() => gm.ChangeScene(scene);
}
