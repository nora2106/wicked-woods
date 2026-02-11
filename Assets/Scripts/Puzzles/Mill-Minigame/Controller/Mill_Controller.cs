using System;
using System.Diagnostics;
using System.Threading.Tasks;

public interface IMillController
{
    void StartGame();
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

        if (rules.CanRemoveStone(model, FieldState.Player))
        {
            var result = rules.RemoveStone(model, e.Key, FieldState.Enemy);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    return;
                case MoveResult.Ok:
                    break;
            }
        }

        else if (rules.CanPlaceStone(model, FieldState.Player))
        {
            var result = rules.PlaceStone(model, e.Key, FieldState.Player);
            switch (result)
            {
                case MoveResult.Invalid:
                    return;
                case MoveResult.MillFormed:
                    view.UpdateBoard(model.GameBoard);
                    return;
                case MoveResult.Ok:
                    break;
            }
        }

        else if (rules.CanMoveStone(model, FieldState.Player) || rules.CanFly(model, FieldState.Player))
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
                        view.UpdateBoard(model.GameBoard);
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
  
        view.UpdateBoard(model.GameBoard);
        SwitchTurn();
    }

    /// <summary>
    /// Switches which player's turn it is.
    /// </summary>
    private void SwitchTurn()
    {
        // check for any loss or draw
        // TODO implement warnings 
        if(model.DrawCount > 20)
        {
            StopGame(FieldState.Empty);
        }
        else if(!rules.HasEnoughStones(model, FieldState.Enemy))
        {
            StopGame(FieldState.Player);
        }
        else if(!rules.HasEnoughStones(model, FieldState.Player))
        {
            StopGame(FieldState.Enemy);
        }
        // TODO implement draw after x moves without any mill
        playerTurn = !playerTurn;

        if (!playerTurn)
        {
            enemy.TakeTurn();
            ExecuteEnemyMove();
        }
    }

    /// <summary>
    /// Visual delay after enemy turn.
    /// </summary>
    private async void ExecuteEnemyMove()
    {
        await Task.Delay(new TimeSpan(0, 0, 2));
        view.UpdateBoard(model.GameBoard);
        if (rules.CanRemoveStone(model, FieldState.Enemy))
        {
            enemy.TakeTurn();
            ExecuteEnemyMove();
        }
        else
        {
            SwitchTurn();
        }
    }

    /// <summary>
    /// Starts a new game.
    /// </summary>
    public void StartGame()
    {
        playerTurn = true;
        // visual cues
    }

    /// <summary>
    /// Ends the current game.
    /// Declares win, loss or draw based on winner param.
    /// <param name="winner">The game's winner, Empty in case of draw.</param>
    /// </summary>
    public void StopGame(FieldState winner)
    {
        playerTurn = false;
        switch (winner)
        {
            case FieldState.Enemy:
                UnityEngine.Debug.Log("You lost.");
                break;
            case FieldState.Player:
                UnityEngine.Debug.Log("You won.");
                break;
            case FieldState.Empty:
                UnityEngine.Debug.Log("Draw.");
                break;
        }
    }
}