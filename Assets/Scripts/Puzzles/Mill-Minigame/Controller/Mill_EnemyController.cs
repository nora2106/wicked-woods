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
        var possibleMillFields = model.GetPossibleMillFields(myState);
        var possibleOppMillFields = model.GetPossibleMillFields(opponentState);
        Dictionary<int, int[]> possibleMoves = new();

        // TODO maybe auslagern?
        // selects the move with the fewest necessary moves based on a list of target nodes
        void GetMinMoveCount(FieldState state, List<int> fieldset)
        {
            int minMoveCount = int.MaxValue;
            int[] startTarget = new int[2];
            // get move distance to all possible mill fields
            foreach (int target in fieldset)
            {
                foreach (int start in model.GetFieldsByState(state))
                {
                    int dist = model.CalcMoveDistance(start, target);
                    // save if first or smaller than current value
                    if (dist < minMoveCount && start != target)
                    {
                        minMoveCount = dist;
                        startTarget[0] = start;
                        startTarget[1] = target;
                    }
                }
            }
            if (minMoveCount != int.MaxValue && !possibleMoves.Keys.Contains(minMoveCount))
            {
                possibleMoves.Add(minMoveCount, startTarget);
            }
        }

        // first: calculate and store actions with smallest distance to close mill and prevent enemy mill
        GetMinMoveCount(myState, possibleMillFields);
        GetMinMoveCount(opponentState, possibleOppMillFields);

        // TODO calculate moves to block enemy mill
        var fullOppMills = model.GetMillsByPlayer(opponentState);
        List<int> fieldsToBlock = new();

        foreach (var mill in fullOppMills)
        {
            foreach (int field in mill)
            {
                foreach (int neighbor in model.GetNeighbors(field))
                {
                    if (model.GameBoard[field].state == FieldState.Empty)
                    {
                        fieldsToBlock.Add(neighbor);
                    }
                }
            }
        }

        GetMinMoveCount(opponentState, fieldsToBlock);

        // choose action with least moves required
        int minMoveCount = int.MaxValue;
        foreach (var pair in possibleMoves)
        {
            if (pair.Key < minMoveCount)
            {
                minMoveCount = pair.Key;

                // stop calculating if an action only requires one move
                if (pair.Key == 1)
                {
                    UnityEngine.Debug.Log("chose to either build or prevent a mill");
                    return possibleMoves[minMoveCount];
                }
            }
        }

        // TODO also allow actions with more than 1 move? or trash idea and change to only allow actions with one move
        if (minMoveCount > 1)
        {
            UnityEngine.Debug.Log("chose to either build or prevent a mill");
            return possibleMoves[minMoveCount];
        }

        // backup: return random fieldset
        foreach (int field in model.GetFieldsByState(myState))
        {
            foreach (int neighbor in model.GetNeighbors(field))
            {
                if (model.GameBoard[neighbor].state == FieldState.Empty)
                {
                    UnityEngine.Debug.Log("chose backup random field");
                    return new int[2] { field, neighbor };
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
        var possibleMillFields = model.GetBlockedMillFields(myState);
        foreach (int field in possibleMillFields)
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
