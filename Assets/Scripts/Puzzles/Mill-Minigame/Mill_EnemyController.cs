public class EnemyController
{
    private IMillModel model;
    private IMillRules rules;
    public EnemyController(IMillModel model, IMillRules rules)
    {
        this.model = model;
        this.rules = rules;
    }

    /// <summary>
    /// Find the best field to place a stone.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcPlaceStone()
    {
        // temporaty: select first empty field
        return model.GetFieldsByState(FieldState.Empty)[0];
    }

    /// <summary>
    /// Select a stone to move to a neighboring field.
    /// </summary>
    /// <returns>Keys of chosen and target field.</returns>
    public int[] CalcMoveStone()
    {
        int[] fieldPair = null;
        for (int i = 0; i < model.GameBoard.Count; i++)
        {
            foreach (var neighbor in model.GameBoard[i].neighbors)
            {
                if (model.GameBoard[neighbor].state == FieldState.Empty)
                {
                    fieldPair[0] = i;
                    fieldPair[1] = neighbor;
                    i = model.GameBoard.Count;
                }
            }
        }
        return fieldPair;
    }

    /// <summary>
    /// Find the best player stone to remove.
    /// </summary>
    /// <returns>Key of the chosen field.</returns>
    public int CalcRemoveStone()
    {
        // temporary: select first field with player stone
        return model.GetFieldsByState(FieldState.Player)[0];
    }
}
