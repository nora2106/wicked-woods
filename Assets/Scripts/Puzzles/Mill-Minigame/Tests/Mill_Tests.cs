using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
public class TestBoardBuilder
{
    // board after setup, 18 stones placed, no mill
    public static IMillModel BoardAfterSetup()
    {
        var board = new MillModel();
        int[] playerFields = new int[]{0, 2, 21, 23, 18, 20, 3, 14, 7};
        int[] enemyFields = new int[]{1, 9, 22, 5, 10, 4, 13, 19, 6};
        for (int i = 0; i < playerFields.Length; i++)
        {
            board.UpdateField(playerFields[i], FieldState.Player);
        }
        for (int i = 0; i < enemyFields.Length; i++)
        {
            board.UpdateField(enemyFields[i], FieldState.Enemy);
        }
        return board;
    }
}

[TestFixture]
public class BaseEnemyMoves
{
    [Test]
    // place stone on a valid field
    public void EnemyPlaceStone()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);
        
        // add some player and enemy stones
        model.UpdateField(0, FieldState.Player);
        model.UpdateField(1, FieldState.Enemy);
        model.UpdateField(6, FieldState.Player);

        int newField = enemy.CalcPlaceStone();
        var result = rules.PlaceStone(model, newField, FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.Ok));
        Assert.That(model.GameBoard[newField].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // place stone on an invalid field
    public void DontPlaceInvalidStone()
    {
        var model = new MillModel();
        var rules = new MillRules();
        
        // add some player and enemy stones
        model.UpdateField(0, FieldState.Player);
        model.UpdateField(1, FieldState.Enemy);
        model.UpdateField(6, FieldState.Player);

        int newField = 0;
        var result = rules.PlaceStone(model, newField, FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.Invalid));
        Assert.That(model.GameBoard[newField].state, Is.Not.EqualTo(FieldState.Enemy));
    }

    [Test]
    // move a stone to a valid field
    public void EnemyMoveStone()
    {
        var model = TestBoardBuilder.BoardAfterSetup();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);

        int[] fieldPair = enemy.CalcMoveStone();

        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);

        Assert.That(model.GameBoard[fieldPair[0]].state, Is.EqualTo(FieldState.Empty));
        Assert.That(model.GameBoard[fieldPair[1]].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // move a stone to an invalid field
    public void DontExecuteInvalidMove()
    {
        var model = TestBoardBuilder.BoardAfterSetup();
        var rules = new MillRules();
        var enemy = new EnemyController(model,rules);

        // add some player and enemy stones
        model.UpdateField(0, FieldState.Player);
        model.UpdateField(1, FieldState.Enemy);
        model.UpdateField(6, FieldState.Player);

        int[] fieldPair = new int[]{1, 0};
        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.Invalid));
        Assert.That(model.GameBoard[fieldPair[0]].state, Is.Not.EqualTo(FieldState.Empty));
        Assert.That(model.GameBoard[fieldPair[1]].state, Is.Not.EqualTo(FieldState.Enemy));
    }
}

[TestFixture]
public class CalcEnemyMoves
{
    [Test]
    // find possible mill and close it by placing new stone
    public void ClosePossibleMillByPlacing()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);

        model.UpdateField(19, FieldState.Enemy);
        model.UpdateField(16, FieldState.Enemy);

        int newField = enemy.CalcPlaceStone();
        var result = rules.PlaceStone(model, newField, FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.MillFormed));
    }

    [Test]
    // find possible mill and close it by moving stone
    public void ClosePossibleMillByMoving()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);

        model.UpdateField(19, FieldState.Enemy);
        model.UpdateField(16, FieldState.Enemy);
        model.UpdateField(23, FieldState.Enemy);

        int[] fieldPair = enemy.CalcMoveStone();
        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);
        
        Assert.That(model.GameBoard[fieldPair[1]].state, Is.EqualTo(FieldState.Enemy));
        Assert.That(result, Is.EqualTo(MoveResult.MillFormed));
    }

    [Test]
    // detect possible player mill and avert it by placing stone
    public void AvertPlayerMillByPlacing()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);

        model.UpdateField(21, FieldState.Player);
        model.UpdateField(22, FieldState.Player);

        int target = enemy.CalcPlaceStone();
        var result = rules.PlaceStone(model, target, FieldState.Enemy);

        Assert.That(model.GameBoard[23].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // detect possible player mill and avert it by moving stone
    public void AvertPlayerMillByMoving()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);

        model.UpdateField(21, FieldState.Player);
        model.UpdateField(22, FieldState.Player);
        model.UpdateField(14, FieldState.Enemy);

        int[] fieldPair = enemy.CalcMoveStone();
        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);

        Assert.That(model.GameBoard[23].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // calc move distance between 2 fields
    public void MoveDistance()
    {
        var model = new MillModel();
        var rules = new MillRules();
        int moveCount = model.CalcMoveDistance(21, 0);

        Assert.That(moveCount, Is.EqualTo(2));
    }

    [Test]
    // calc move distance between 2 fields
    public void MoveDistanceWithObstacles()
    {
        var model = new MillModel();
        var rules = new MillRules();

        model.UpdateField(9, FieldState.Player);
        int moveCount = model.CalcMoveDistance(21, 0);
        Assert.That(moveCount, Is.EqualTo(6));
    }

    [Test]
    // calc move distance between 2 fields
    public void MoveDistancePathImpossible()
    {
        var model = new MillModel();
        var rules = new MillRules();
        
        model.UpdateField(9, FieldState.Player);
        model.UpdateField(1, FieldState.Player);
        int moveCount = model.CalcMoveDistance(21, 0);

        Assert.That(moveCount, Is.EqualTo(0));
    }
}

[TestFixture]
public class TestGames
{
    [Test]
    public void TestSetupPhase()
    {
        var model = new MillModel();
        var rules = new MillRules();
        var enemy = new EnemyController(model, rules);
        var player = new EnemyController(model, rules, FieldState.Player, FieldState.Enemy);
    }
}