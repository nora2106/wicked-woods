public interface IMillRules
{
     MoveResult PlaceStone(IMillModel model, int fieldKey, FieldState player);
     MoveResult MoveStone(IMillModel model, int from, int to, FieldState player);
     MoveResult RemoveStone(IMillModel model, int fieldKey, FieldState player);
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
    /// <summary>
    /// Place a new stone on an empty field.
    /// </summary>
    /// <param name="model">Instance of the model interface.</param>
    /// <param name="fieldKey">Index of the selected field.</param>
    /// <param name="player">Current player - new field state.</param>
    public MoveResult PlaceStone(IMillModel model, int fieldKey, FieldState player)
    {
        if(model.GameBoard[fieldKey].state != FieldState.Empty)
        {
            return MoveResult.Invalid;
        }

        model.UpdateField(fieldKey, player);
        
        if(model.CheckForMill(fieldKey)) {
            return MoveResult.MillFormed;
        }

        return MoveResult.Ok;
    }

    /// <summary>
    /// Move stone from field to a neighboring empty field.
    /// </summary>
    /// <param name="model">Instance of the model interface.</param>
    /// <param name="from">Index of the current field.</param>
    /// <param name="to">Index of the new field.</param>
    /// <param name="player">Current player - new field state.</param>
    public MoveResult MoveStone(IMillModel model, int from, int to, FieldState player)
    {
        if(!model.GameBoard[from].neighbors.Contains(to) || model.GameBoard[to].state != FieldState.Empty)
        {
            return MoveResult.Invalid;
        }

        model.UpdateField(from, FieldState.Empty);
        model.UpdateField(to, player);
        
        if(model.CheckForMill(to)) {
            return MoveResult.MillFormed;
        }

        return MoveResult.Ok;
    }

    /// <summary>
    /// Remove opponent's stone from field.
    /// </summary>
    /// <param name="model">Instance of the model interface.</param>
    /// <param name="fieldKey">Index of the selected field.</param>
    /// <param name="player">Player executing the action.</param>
    public MoveResult RemoveStone(IMillModel model, int fieldKey, FieldState player)
    {
        if(model.GameBoard[fieldKey].state == FieldState.Empty || model.GameBoard[fieldKey].state == player || model.CheckForMill(fieldKey))
        {
            return MoveResult.Invalid;
        }

        model.UpdateField(fieldKey, FieldState.Empty);
        return MoveResult.Ok;
    }
}
