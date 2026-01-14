using System.Diagnostics;
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
        // temporaty: select first empty field
        return model.GetFieldsByState(FieldState.Empty)[0];
    }

    /// <summary>
    /// Select a stone to move to a neighboring field.
    /// </summary>
    /// <returns>Keys of chosen and target field.</returns>
    public int[] CalcMoveStone()
    {
        // first: check for possible mills
        var possibleMillFields = model.GetPossibleMills(FieldState.Enemy);
        foreach(int field in possibleMillFields)
        {
            foreach(int neighbor in model.GetNeighbors(field))
            {
                if(model.GameBoard[neighbor].state == FieldState.Enemy)
                {
                    return new int[2]{neighbor, field};
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
                    return new int[2]{neighbor, field};
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
        // temporary: select first field with player stone
        return model.GetFieldsByState(FieldState.Player)[0];
    }
}
