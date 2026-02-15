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
    Fly,
}

public class MillController : IMillController
{
    private readonly IMillModel model;
    private readonly IMillView view;
    private readonly IMillRules rules;
    private readonly EnemyController enemy;
    private bool playerTurn;
    private Phase phase = Phase.Setup;
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
                    view.DisplayText("Das hat nicht funktioniert - entweder du hast ein falsches Feld ausgewählt oder dein Gegner hat keine freien Steine.");
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
                    view.UpdateBoard(model);
                    view.DisplayText("Du hast eine Mühle geschlossen! Klicke auf einen gegnerischen Stein, um ihn zu schlagen.");
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
                        view.UpdateBoard(model);
                        view.DisplayText("Du hast eine Mühle geschlossen! Klicke auf einen gegnerischen Stein, um ihn zu schlagen.");
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

        view.UpdateBoard(model);
        SwitchTurn();
    }

    /// <summary>
    /// Switches which player's turn it is.
    /// </summary>
    private void SwitchTurn()
    {
        if (phase == Phase.Setup && rules.CanMoveStone(model, FieldState.Player))
        {
            view.DisplayText("Klicke auf einen deiner Steine und danach ein benachbartes leeres Feld, um ihn zu bewegen.");
            phase = Phase.Move;
        }
        if (rules.CanFly(model, FieldState.Player))
        {
            view.DisplayText("Du hast nur noch 3 Steine - du kannst jetzt fliegen und deine Steine an beliebige Felder bewegen, nicht nur benachbarte.");
        }

        if (rules.CanFly(model, FieldState.Enemy))
        {
            view.DisplayText("Dein Gegner hat nur noch 3 Steine - er kann jetzt fliegen und deine Steine an beliebige Felder bewegen, nicht nur benachbarte.");
        }
        // check for any loss or draw
        // TODO implement warnings 
        if (model.DrawCount > 20)
        {
            StopGame(FieldState.Empty);
        }
        else if (!rules.HasEnoughStones(model, FieldState.Enemy))
        {
            StopGame(FieldState.Player);
        }
        else if (!rules.HasEnoughStones(model, FieldState.Player))
        {
            StopGame(FieldState.Enemy);
        }
        playerTurn = !playerTurn;

        if (!playerTurn)
        {
            view.UpdateTurnText("Dein Gegner ist dran.");
            enemy.TakeTurn();
            ExecuteEnemyMove();
        }
        else
        {
            view.UpdateTurnText("Du bist dran!");
        }
    }

    /// <summary>
    /// Visual delay after enemy turn.
    /// </summary>
    private async void ExecuteEnemyMove()
    {
        await Task.Delay(new TimeSpan(0, 0, 2));
        view.UpdateBoard(model);
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
                view.DisplayText("Du hast verloren :(");
                break;
            case FieldState.Player:
                view.DisplayText("Du hast gewonnen! Yippie ^w^");
                break;
            case FieldState.Empty:
                view.DisplayText("Mehr als 20 Züge ohne eine Mühle... Ich glaube, das gewinnt keiner mehr. Gleichstand.");
                break;
        }
    }
}