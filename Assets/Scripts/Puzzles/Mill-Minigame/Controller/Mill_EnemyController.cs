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
    private readonly List<int[]> edgePoints = new List<int[]>() { new int[4] { 21, 23, 0, 2 }, new int[4] { 18, 20, 3, 5 }, new int[4] { 15, 17, 6, 8 } };
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

        // only on first move: if opponent occupies an edge point - select opposite edge
        // TODO for added difficulty: not only for first move, but for every edge row
        if (model.GetFieldsByState(myState).Count <= 1 && model.GetFieldsByState(opponentState).Count <= 1)
        {
            int field = model.GetFieldsByState(opponentState)[0];
            if (edgePoints.Any(l => l.Contains(field)))
            {
                // get row opponent field is in
                int[] row = edgePoints.First(l => l.Contains(field));
                var fieldNeighbors = model.GetNeighbors(field);
                foreach (int node in row)
                {
                    if (model.GetNeighbors(node).Any(n => fieldNeighbors.Contains(n)) && model.GameBoard[node].state == FieldState.Empty)
                    {
                        return node;
                    }
                }
            }
        }

        // place in any row with at least one stone and no opponent stone
        foreach (var mill in model.GetPossibleMills())
        {
            if (mill.Any(n => model.GameBoard[n].state == myState) && !mill.Any(n => model.GameBoard[n].state == opponentState) && mill.Any(n => model.GameBoard[n].state == FieldState.Empty))
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
        UnityEngine.Debug.Log("fallback: random field");
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

        // get all movable fields that aren't blocking opponent mill
        UnityEngine.Debug.Log("blocked fields: " + model.GetBlockedMillFields(opponentState, myState).Count);
        List<int> movableFields = model.GetMovableFields(myState).FindAll(f => !model.GetBlockedMillFields(opponentState, myState).Contains(f));

        // if possible, close own mill
        foreach (int field in almostMills.Keys)
        {
            if (rules.CanFly(model, myState))
            {
                int f = model.GetFieldsByState(myState).First(n => !almostMills[field].Contains(n));
                return new int[2] { f, field };
            }

            // remove almost mill fields from movable fields
            movableFields = movableFields.Where(f => !almostMills[field].Contains(f)).ToList();

            // calculate shortest path to field from all movable stones
            var move = model.CalcShortestPath(field, movableFields);
            var tempTarget = model.CalcMoveDistance(move.field, field).field;
            if (move.dist < minMove.dist && model.GameBoard[tempTarget].state == FieldState.Empty)
            {
                minMove = move;
                target = tempTarget;
            }
        }
        // stop calculating if mill can be closed in 1 turn
        if (minMove.dist == 1)
        {
            UnityEngine.Debug.Log("complete mill");
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

        // if possible in one move, prevent opponent mill
        foreach (int field in almostOppMills.Keys)
        {
            if (rules.CanFly(model, myState))
            {
                return new int[2] { model.GetFieldsByState(myState)[0], field };
            }
            else if (model.GetNeighbors(field).Any(f => model.GameBoard[f].state == myState))
            {
                return new int[2] { model.GetNeighbors(field).First(f => model.GameBoard[f].state == myState), field };
            }
        }

        // TODO fly phase: try to create a mill
        if (rules.CanFly(model, myState))
        {

        }

        // TODO implement blocking enemy mills

        // choose move with the shortest distance until goal reached
        if (target != 0)
        {
            UnityEngine.Debug.Log("try to complete mill");
            return new int[2] { minMove.field, target };
        }

        // move stone to a row that contains no enemy stones
        foreach (int field in movableFields)
        {
            var emptyNeighbors = model.GetNeighbors(field).FindAll(n => model.GameBoard[n].state == FieldState.Empty);
            foreach (int n in emptyNeighbors)
            {
                if (model.GetPossibleMills()[n].All(f => model.GameBoard[f].state != opponentState))
                {
                    return new int[2] { field, n };
                }
            }
        }

        // backup: get first possible fieldpair
        int backupField = model.GetMovableFields(myState).First(f => model.GetNeighbors(f).Any(n => model.GameBoard[n].state == FieldState.Empty));
        if (movableFields.Count > 0)
        {
            backupField = movableFields.First(f => model.GetNeighbors(f).Any(n => model.GameBoard[n].state == FieldState.Empty));
        }
        UnityEngine.Debug.Log("backup: random field: " + backupField);
        return new int[] { backupField, model.GetNeighbors(backupField).First(n => model.GameBoard[n].state == FieldState.Empty) };
    }

    /// <summary>
    /// Find the best player stone to remove.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcRemoveStone()
    {
        // fields that are not in a mill
        var oppFields = model.GetFieldsByState(opponentState);
        foreach (var mill in model.GetMillsByPlayer(opponentState))
        {
            foreach (int field in mill)
            {
                if (oppFields.Contains(field))
                {
                    oppFields.Remove(field);
                }
            }
        }

        // remove stone from almost mill
        foreach (var field in model.GetAlmostMills(opponentState).Values)
        {
            return field.First(f => oppFields.Contains(f));

        }

        // enable possible mill by looking for blocked mills (row containing 2 enemy stones and 1 player stone)
        var blockedFields = model.GetBlockedMillFields(myState, opponentState);
        foreach (int field in blockedFields)
        {
            if (model.GameBoard[field].state == opponentState && oppFields.Contains(field))
            {
                return field;
            }
        }

        UnityEngine.Debug.Log("remove random field");
        return oppFields[0];
    }

    public void TakeTurn()
    {
        if (rules.CanRemoveStone(model, myState) && model.ExistFreeStones(opponentState))
        {
            rules.RemoveStone(model, CalcRemoveStone(), opponentState);
        }
        else if (rules.CanPlaceStone(model, myState))
        {
            rules.PlaceStone(model, CalcPlaceStone(), myState);
        }
        else if (rules.CanMoveStone(model, myState) || rules.CanFly(model, myState))
        {
            rules.MoveStone(model, CalcMoveStone()[0], CalcMoveStone()[1], myState);
        }
    }
}
