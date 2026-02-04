public interface IMillRules
{
     MoveResult PlaceStone(IMillModel model, int fieldKey, FieldState player);
     MoveResult MoveStone(IMillModel model, int from, int to, FieldState player);
     MoveResult RemoveStone(IMillModel model, int fieldKey, FieldState player);
    bool CanPlaceStone(IMillModel model, FieldState player);
    bool CanMoveStone(IMillModel model, FieldState player);
    bool CanFly(IMillModel model, FieldState player);
    bool CanRemoveStone(IMillModel model, FieldState player);
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
    private FieldState canRemove = FieldState.Empty;
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
        model.AvailableStones[player]--;
        
        if(model.CheckForMill(fieldKey, player)) {
            canRemove = player;
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
        if(model.GameBoard[to].state != FieldState.Empty)
        {
            return MoveResult.Invalid;
        }

        else if(!model.GameBoard[from].neighbors.Contains(to) && !CanFly(model, player))
        {
            return MoveResult.Invalid;
        }

        model.UpdateField(from, FieldState.Empty);
        model.UpdateField(to, player);
        
        if(model.CheckForMill(to, player)) {
            if(model.ExistFreeStones(CalcOpponent(player)))
            {
                canRemove = player;
            }
            else
            {
                UnityEngine.Debug.Log("mill formed, but no free enemy stones to remove");
            }
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
        if(model.GameBoard[fieldKey].state == FieldState.Empty || model.GameBoard[fieldKey].state == player || model.CheckForMill(fieldKey, player))
        {
            return MoveResult.Invalid;
        }

        model.UpdateField(fieldKey, FieldState.Empty);
        canRemove = FieldState.Empty;
        return MoveResult.Ok;
    }

    public bool CanPlaceStone(IMillModel model, FieldState player)
    {
        return model.AvailableStones[player] > 0;
    }

    public bool CanMoveStone(IMillModel model, FieldState player)
    {
        return model.GetFieldsByState(player).Count > 3 && model.AvailableStones[player] <= 0;
    }

    public bool CanFly(IMillModel model, FieldState player)
    {
        return model.GetFieldsByState(player).Count <= 3;
    }

    public bool CanRemoveStone(IMillModel model, FieldState player)
    {
        return canRemove == player;
    }

    public FieldState CalcOpponent(FieldState player)
    {
        if(player == FieldState.Player)
        {
            return FieldState.Enemy;
        }
        else if(player == FieldState.Enemy)
        {
            return FieldState.Player;
        }
        return FieldState.Empty;
    }
}