// interactable door object
public class DoorObject : UsableObject
{
    public string nextScene;

    override public void Action()
    {
        gm.QueueInteraction(new ChangeSceneCommand(nextScene));
    }

}
