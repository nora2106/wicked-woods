using System;

public interface IMillView
{
    Array gameBoard {get; set;}
}

public class MillView : IMillView
{
    public Array gameBoard
    {
        get {return gameBoard; }
        set
        {
            // set selected field (a0 - g6) to "" (empty), w (white) or b (black)
        }
    }
}