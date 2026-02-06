using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
public class EnemyController
{
    private readonly IMillModel model;
    private readonly IMillRules rules;
    private readonly FieldState myState;
    private readonly FieldState opponentState;

    // list containing all edge nodes of the 3 squares (strategically important)
    private readonly List<int[]> edgePoints = new List<int[]>() { new int[4] { 21, 23, 0, 2 }, new int[4] { 18, 20, 3, 5 }, new int[4] { 15, 17, 6, 3 } };
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

        // place first stone opposite to player stone
        if (model.AvailableStones[opponentState] == 8 && model.AvailableStones[myState] == 9)
        {
            int firstStone = model.GetFieldsByState(opponentState)[0];
        }

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

        // if opponent occupies an edge point - select opposite edge
        foreach (int field in model.GetFieldsByState(opponentState))
        {
            if (edgePoints.Any(l => l.Contains(field)))
            {
                // get row opponent field is in
                int[] row = edgePoints.First(l => l.Contains(field));
                foreach (int node in row)
                {
                    if (model.CalcMoveDistance(field, node) == 2 && model.GameBoard[node].state == FieldState.Empty)
                    {
                        return node;
                    }
                }
            }
        }

        // place in any row with at least one stone and no opponent stone
        foreach (var mill in model.GetPossibleMills())
        {
            if (mill.Any(n => model.GameBoard[n].state == myState) && !mill.Any(n => model.GameBoard[n].state == opponentState))
            {
                return mill.First(n => model.GameBoard[n].state == FieldState.Empty);
            }
        }

        // fallback: occupy strategically smart fields if possible
        foreach (int field in edgePoints[0])
        {
            if (model.GameBoard[field].state == FieldState.Empty)
            {
                return field;
            }
        }

        // final fallback: first empty field   
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
        FieldDistance minMove = new(0, int.MaxValue);
        int target = 0;
        // movable fields
        List<int> movableFields = model.GetMovableFields(myState);

        // get fields that are currently blocking an enemy mill
        //var blockingFields = new List<int>();

        // if possible, close own mill
        foreach (int field in almostMills.Keys)
        {
            // calculate shortest path to field from all stones that are not in any almost mill
            List<int> availableFields = movableFields.Where(f => !almostMills[field].Contains(f)).ToList();
            var closeMillMove = model.CalcShortestPath(field, availableFields);
            if (closeMillMove.dist < minMove.dist)
            {
                minMove = closeMillMove;
                target = field;
            }
        }

        if (minMove.dist == 1)
        {
            return new int[2] { minMove.field, target };
        }

        // if closed mill and no open opponent mill: open mill
        if (model.GetMillsByPlayer(myState).Count > 0 && !model.HasOpenMills(opponentState))
        {
            foreach (var mill in model.GetMillsByPlayer(myState))
            {
                foreach (int field in mill)
                {
                    if (model.GetNeighbors(field).Any(n => model.GameBoard[n].state == FieldState.Empty))
                    {
                        return new int[2] { field, model.GetNeighbors(field).First(n => model.GameBoard[n].state == FieldState.Empty) };
                    }
                }
            }
        }

        // if possible, prevent opponent mill
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

        // backup: get first possible fieldpair
        UnityEngine.Debug.Log("backup: random field");
        int backupField = movableFields[0];
        return new int[] { backupField, model.GetNeighbors(backupField)[0] };
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
