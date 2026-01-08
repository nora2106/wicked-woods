public interface IMillRules
{
    public FieldState CurrentState { get; set; }
    public MoveResult PlaceOrMoveStone(IMillModel model, int key);
    public void ExecuteEnemyMove(IMillModel model);
}

public enum MoveResult
{
    Invalid,
    Ok,
    MillFormed,
    FieldSelectRequired,
    GameOver
    
}

public class MillRules : IMillRules
{
    public FieldState currentState = new FieldState();
    public FieldState CurrentState{get {return currentState; } set{currentState = value; }}
    public MoveResult PlaceOrMoveStone(IMillModel model, int key)
    {
        model.UpdateField(key, currentState);
        if(model.CheckForMill(key, currentState))
            return MoveResult.MillFormed;

        return MoveResult.Ok;
    }

    public void ExecuteEnemyMove(IMillModel model)
    {
        
    }
}
