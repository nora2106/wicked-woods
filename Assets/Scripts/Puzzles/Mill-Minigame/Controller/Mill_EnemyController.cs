using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

public class EnemyController
{
    private IMillModel model;
    public EnemyController(IMillModel model)
    {
        this.model = model;
    }

    /// <summary>
    /// Find the best field to place a stone.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcPlaceStone()
    {
        // get possible mills
        var possibleMillFields = model.GetPossibleMillFields(FieldState.Enemy);
        foreach(int field in possibleMillFields)
        {
            if(model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // get possible player mills
        var possibleMillFieldsPlayer = model.GetPossibleMillFields(FieldState.Player);
        foreach(int field in possibleMillFieldsPlayer)
        {
            if(model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // for testing: get random empty field      
        return model.GetFieldsByState(FieldState.Empty)[0];
    }

    /// <summary>
    /// Select a stone to move to a neighboring field.
    /// </summary>
    /// <returns>Keys of chosen and target field.</returns>
    public int[] CalcMoveStone()
    {
        // first: check for possible mills
        var almostMills = model.GetAlmostMills(FieldState.Enemy);
        foreach(int key in almostMills.Keys)
        {
            foreach(int neighbor in model.GetNeighbors(key))
            {
                // choose field with enemy stone that's not in the current almostMill
                if(model.GameBoard[neighbor].state == FieldState.Enemy && !almostMills[key].Contains(neighbor))
                {
                    return new int[2]{neighbor, key};
                }
            }
        }

        // for testing: get random fieldpair  
        foreach(int field in model.GetFieldsByState(FieldState.Enemy))
        {
            foreach(int neighbor in model.GetNeighbors(field))
            {
                if(model.GameBoard[neighbor].state == FieldState.Empty)
                {
                    return new int[2]{field, neighbor};
                }
            }
        }      
        return new int[2];
    }

    /// <summary>
    /// Find the best player stone to remove.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcRemoveStone()
    {
        // prevent player mill
        // TODO order depends on game phase - setup: remove one of 2 stones in row, move: predict player moves
        var possibleMillFieldsPlayer = model.GetPossibleMillFields(FieldState.Player);
        foreach(int field in possibleMillFieldsPlayer)
        {
            foreach(var neighbor in model.GetNeighbors(field))
            {
                if(model.GameBoard[neighbor].state == FieldState.Player)
                {
                    return neighbor;
                }
            }
        }

        // enable possible mill by looking for blocked mills (row containing 2 enemy stones and 1 player stone)
        var possibleMillFields = model.GetBlockedMillFields(FieldState.Enemy);
        foreach(int field in possibleMillFields)
        {
            if(model.GameBoard[field].state == FieldState.Player)
            {
                return field;
            }
        }

        // fallback: select first field with player stone
        return model.GetFieldsByState(FieldState.Player)[0];
    }
}
