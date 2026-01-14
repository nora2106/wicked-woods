using System;
using NUnit.Framework;
public class TestBoardBuilder
{
    // board after setup, populated with random stone placement
    public static IMillModel BoardAfterSetup()
    {
        var board = new MillModel();
        for (int i = 0; i < 9; i++)
        {
            board.UpdateField(board.GetFieldsByState(FieldState.Empty)[0], FieldState.Player);
            board.UpdateField(board.GetFieldsByState(FieldState.Empty)[0], FieldState.Enemy);
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
        var enemy = new EnemyController(model);
        var rules = new MillRules();
        
        // add some player and enemy stones
        model.UpdateField(0, FieldState.Player);
        model.UpdateField(1, FieldState.Enemy);
        model.UpdateField(6, FieldState.Player);

        int newField = enemy.CalcPlaceStone();
        var result = rules.PlaceStone(model, newField, FieldState.Enemy);
        

        switch (result)
        {
            case MoveResult.Invalid:
                throw new InvalidOperationException("Invalid field for placement.");
            case MoveResult.MillFormed:
                break;
            case MoveResult.Ok:
                break;
        }

        Assert.That(model.GameBoard[newField].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // place stone on an invalid field
    public void DontPlaceInvalidStone()
    {
        var model = new MillModel();
        var enemy = new EnemyController(model);
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
        var enemy = new EnemyController(model);
        var rules = new MillRules();

        int[] fieldPair = enemy.CalcMoveStone();

        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);
        switch (result)
        {
            case MoveResult.Invalid:
                return;
            case MoveResult.MillFormed:
                break;
            case MoveResult.Ok:
                break;
        }

        Assert.That(model.GameBoard[fieldPair[0]].state, Is.EqualTo(FieldState.Empty));
        Assert.That(model.GameBoard[fieldPair[1]].state, Is.EqualTo(FieldState.Enemy));
    }

    [Test]
    // move a stone to an invalid field
    public void DontExecuteInvalidMove()
    {
        var model = TestBoardBuilder.BoardAfterSetup();
        var enemy = new EnemyController(model);
        var rules = new MillRules();

        int[] fieldPair = enemy.CalcMoveStone();
        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.Invalid));
        Assert.That(model.GameBoard[fieldPair[0]].state, Is.Not.EqualTo(FieldState.Empty));
        Assert.That(model.GameBoard[fieldPair[1]].state, Is.Not.EqualTo(FieldState.Enemy));
    }
}

[TestFixture]
public class EnemyCalcMoves
{
    [Test]
    // find possible mill and close it by placing new stone
    public void ClosePossibleMillByPlacing()
    {
        var model = new MillModel();
        var enemy = new EnemyController(model);
        var rules = new MillRules();

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
        var enemy = new EnemyController(model);
        var rules = new MillRules();

        model.UpdateField(19, FieldState.Enemy);
        model.UpdateField(16, FieldState.Enemy);
        model.UpdateField(23, FieldState.Enemy);

        int[] fieldPair = enemy.CalcMoveStone();
        var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);

        Assert.That(result, Is.EqualTo(MoveResult.MillFormed));
    }
}
