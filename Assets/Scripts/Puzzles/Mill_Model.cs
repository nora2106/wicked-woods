using System;
using UnityEngine.UIElements;

public interface IMillModel
{
    Array gameBoard {get; set;}
}

public class MillModel : IMillModel
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