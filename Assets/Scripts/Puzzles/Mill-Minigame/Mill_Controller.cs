using System;
using System.Threading.Tasks;

public interface IMillController
{
    void StartGame();
    Phase GamePhase { get; set; }
}

public enum Phase
{
    Setup,
    Move,
    Remove,
    GameOver,
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;
    private readonly IMillRules rules;
    private readonly EnemyController enemy;

    public Phase gamePhase;
    public Phase GamePhase
    {
        get => gamePhase;
        set => gamePhase = value;
    }
    private int maxStones = 18;
    private bool playerTurn;
    private PointClickedEventArgs selectedField = null;

    public MillController(IMillModel model, IMillView view, IMillRules rules)
    {
        this.model = model;
        this.view = view;
        this.rules = rules;
        enemy = new EnemyController(model, rules);

        // subscribe to the view board change event
        view.OnBoardChanged += HandlePlayerInput;
    }

    /// <summary>
    /// Handle player input and call action depending on the game phase.
    /// </summary>
    /// <param name="sender">The object triggering the Event.</param>
    /// <param name="e">The Event.</param>
    public void HandlePlayerInput(object sender, PointClickedEventArgs e)
    {
        if (!playerTurn)
        {
            return;
        }

        if (gamePhase == Phase.Setup)
        {
            var result = rules.PlaceStone(model, e.Key, FieldState.Player);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    UpdateView();
                    maxStones--;
                    gamePhase = Phase.Remove;
                    UnityEngine.Debug.Log("select stone to remove");
                    return;
                case MoveResult.Ok:
                    maxStones--;
                    break;
            }
        }

        else if (gamePhase == Phase.Move)
        {
            // select stone to move
            if (e.State == FieldState.Player)
            {
                selectedField = e;
                view.UpdateField(e.Key, FieldState.Selected);
                UnityEngine.Debug.Log("click on any empty neighboring field to move selected stone.");
                return;
            }
            // select field to move to
            else if (selectedField != null)
            {
                var result = rules.MoveStone(model, selectedField.Key, e.Key, FieldState.Player);
                switch (result)
                {
                    case MoveResult.Invalid:
                        return;
                    case MoveResult.MillFormed:
                        UpdateView();
                        gamePhase = Phase.Remove;
                        UnityEngine.Debug.Log("select stone to remove");
                        selectedField = null;
                        return;
                    case MoveResult.Ok:
                        selectedField = null;
                        break;
                }
            }

            // no valid field clicked
            else
            {
                return;
            }
        }

        // select stone to remove
        else if (gamePhase == Phase.Remove)
        {
            var result = rules.RemoveStone(model, e.Key, FieldState.Player);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    return;
                case MoveResult.Ok:
                    gamePhase = Phase.Setup;
                    if (maxStones <= 0)
                    {
                        gamePhase = Phase.Move;
                    }
                    break;
            }
        }

        UpdateView();
        SwitchTurn();
    }

    /// <summary>
    /// Switches which player's turn it is.
    /// </summary>
    private void SwitchTurn()
    {
        // end setup phase if all stones have been placed
        if (gamePhase == Phase.Setup && maxStones <= 0)
        {
            gamePhase = Phase.Move;
            UnityEngine.Debug.Log("move phase");
        }

        playerTurn = !playerTurn;

        if (!playerTurn)
        {
            EnemyMove();
        }
    }

    /// <summary>
    /// Calculate enemy move.
    /// </summary>
    private void EnemyMove()
    {
        if (gamePhase == Phase.Setup)
        {
            // calculate field to place stone
            // for testing: get first empty field
            int targetField = enemy.CalcPlaceStone();
            var result = rules.PlaceStone(model, targetField, FieldState.Enemy);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    maxStones--;
                    EnemyRemoveStone();
                    break;
                case MoveResult.Ok:
                    maxStones--;
                    break;
            }
        }

        else if (gamePhase == Phase.Move)
        {
            // calculate field to move stone to
            // for testing: get first enemy field with empty neighbors and move to first empty neighbor
            // FIXME not working (line 197: Object reference not set to an instance of an object)
            int[] fieldPair = enemy.CalcMoveStone();
            var result = rules.MoveStone(model, fieldPair[0], fieldPair[1], FieldState.Enemy);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    maxStones--;
                    EnemyRemoveStone();
                    break;
                case MoveResult.Ok:
                    maxStones--;
                    break;
            }
        }

        ExecuteEnemyMove();
    }

    public async void ExecuteEnemyMove()
    {
        await Task.Delay(new TimeSpan(0, 0, 1));
        UpdateView();
        SwitchTurn();
    }

    /// <summary>
    /// Remove a stone.
    /// </summary>
    private void EnemyRemoveStone()
    {
        // for testing purposes: get first field with player stone
        int targetField = enemy.CalcRemoveStone();
        var result = rules.RemoveStone(model, targetField, FieldState.Enemy);

        switch (result)
        {
            case MoveResult.Invalid:
                return;
            case MoveResult.MillFormed:
                break;
            case MoveResult.Ok:
                break;
        }
    }

    // Aligns the view board to the model board.
    private void UpdateView()
    {
        view.UpdateBoard(model.GameBoard);
    }

    public void StartGame()
    {
        // start game
        gamePhase = Phase.Setup;
        playerTurn = true;
    }

    public void StopGame()
    {
    }
}