using NUnit.Framework;
public class TestBoardBuilder
{
    // board after setup, populated with random stone placement
    public static IMillModel BoardAfterSetup()
    {
        var board = new MillModel();
        for (int i = 0; i < 18; i++)
        {
            board.UpdateField(board.GetFieldsByState(FieldState.Empty)[0], FieldState.Player);
            board.UpdateField(board.GetFieldsByState(FieldState.Empty)[0], FieldState.Enemy);
        }
        return board;
    }
}

[TestFixture]
public class MillTests
{
    [Test]
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
    public void EnemyMakeMillAndRemoveStone()
    {
        var board = new MillModel();
        board.UpdateField(0, FieldState.Player);
        board.UpdateField(6, FieldState.Player);
        board.UpdateField(10, FieldState.Player);
        board.UpdateField(22, FieldState.Enemy);
        board.UpdateField(19, FieldState.Enemy);
        board.UpdateField(16, FieldState.Enemy);
    }
}
