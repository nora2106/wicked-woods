using System;
using System.Collections.Generic;
using System.Linq;
public class EnemyController
{
    private readonly IMillModel model;
    private readonly IMillRules rules;
    private readonly FieldState myState;
    private readonly FieldState opponentState;

    private readonly List<int> strategicPoints = new List<int>() { 15, 17, 6, 8, 21, 23, 0, 2 };
    public EnemyController(IMillModel model, IMillRules rules, FieldState myState = FieldState.Enemy, FieldState opponentState = FieldState.Player)
    {
        this.model = model;
        this.rules = rules;
        this.myState = myState;
        this.opponentState = opponentState;
    }

    /// <summary>
    /// Find the best field to place a stone.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcPlaceStone()
    {
        var possibleMillFields = model.GetPossibleMillFields(myState);
        var possibleOppMillFields = model.GetPossibleMillFields(opponentState);

        // if possible, complete own mill
        foreach (int field in possibleMillFields)
        {
            if (model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // if possible, prevent enemy mill
        foreach (int field in possibleOppMillFields)
        {
            if (model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // occupy strategically smart fields
        // TODO sort strategic point by effectivity (most possible mill, no opponent stone blocking)
        foreach (int field in strategicPoints)
        {
            if (model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // fallback: random empty field   
        return model.GetFieldsByState(FieldState.Empty)[0];
    }

    /// <summary>
    /// Select a stone to move to a neighboring field.
    /// </summary>
    /// <returns>Keys of chosen and target field.</returns>
    public int[] CalcMoveStone()
    {
        var almostMills = model.GetAlmostMills(myState);
        var almostOppMills = model.GetAlmostMills(opponentState);

        // if possible, complete own mill
        foreach (int field in almostMills.Keys)
        {
            // TODO calculate shortest path to field from all stones that are not in this almost mill
            foreach (int neighbor in model.GetNeighbors(field))
            {
                if (model.GameBoard[neighbor].state == myState && !almostMills[field].Contains(neighbor))
                {
                    return new int[2] { neighbor, field };
                }
            }
        }

        // if possible, prevent enemy mill
        foreach (int field in almostOppMills.Keys)
        {
            // TODO calculate shortest path to field from all stones that are not in almost mills
            foreach (int neighbor in model.GetNeighbors(field))
            {
                if (model.GameBoard[neighbor].state == myState && !almostOppMills[field].Contains(neighbor))
                {
                    return new int[2] { neighbor, field };
                }
            }
        }

        // TODO add backup (after testing)
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
        var possibleMillFieldsPlayer = model.GetPossibleMillFields(opponentState);
        foreach (int field in possibleMillFieldsPlayer)
        {
            foreach (var neighbor in model.GetNeighbors(field))
            {
                if (model.GameBoard[neighbor].state == opponentState)
                {
                    return neighbor;
                }
            }
        }

        // enable possible mill by looking for blocked mills (row containing 2 enemy stones and 1 player stone)
        var blockedFields = model.GetBlockedMillFields(myState);
        foreach (int field in blockedFields)
        {
            if (model.GameBoard[field].state == opponentState)
            {
                return field;
            }
        }

        // fallback: select first field with player stone
        return model.GetFieldsByState(opponentState)[0];
    }

    public void TakeTurn()
    {
        if (rules.CanRemoveStone(model, myState))
        {
            rules.RemoveStone(model, CalcRemoveStone(), myState);
        }
        else if (rules.CanPlaceStone(model, myState))
        {
            rules.PlaceStone(model, CalcPlaceStone(), myState);
        }
        else if (rules.CanMoveStone(model, myState))
        {
            rules.MoveStone(model, CalcMoveStone()[0], CalcMoveStone()[1], myState);
        }
    }
}
