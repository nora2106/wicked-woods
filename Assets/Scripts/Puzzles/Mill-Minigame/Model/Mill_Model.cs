using System.Collections.Generic;
using System.Linq;

public struct FieldDistance
{
    public int field;
    public int dist;

    public FieldDistance(int field, int dist)
    {
        this.field = field;
        this.dist = dist;
    }
}

public interface IMillModel
{
    Dictionary<int, BoardNode> GameBoard { get; set; }
    void InitializeBoard();
    void UpdateField(int key, FieldState player);
    bool AreNeighbors(int key1, int key2);
    List<int> GetFieldsByState(FieldState state);
    bool CheckForMill(int key, FieldState player);

    int[][] GetPossibleMills();

    /// <summary>
    /// Get fields that can be moved from.
    /// </summary>
    /// <param name="state">The required field state.</param>
    /// /// <returns>All field keys with at least one empty neighbor.</returns>
    List<int> GetMovableFields(FieldState state);

    /// <summary>
    /// Get possible mills that are only missing one stone.
    /// </summary>
    /// <param name="state">The required field state.</param>
    /// <returns>Dictionary containing empty field and its full row fields.</returns>
    Dictionary<int, int[]> GetAlmostMills(FieldState state);

    /// <summary>
    /// Get fields that could form a mill within the next move.
    /// </summary>
    /// <param name="state">The required field state.</param>
    /// <returns>All field keys that are the last missing stone for a mill.</returns>
    List<int> GetPossibleMillFields(FieldState state);

    /// <summary>
    /// Get fields that could form a mill but are blocked by an opponent's stone.
    /// </summary>
    /// <param name="state">The required field state.</param>
    /// <returns>All field keys that are the last missing stone for a mill.</returns>
    List<int> GetBlockedMillFields(FieldState state);
    List<int> GetNeighbors(int key);
    List<int[]> GetMillsByPlayer(FieldState player);
    Dictionary<FieldState, int> AvailableStones { get; set; }

    /// <summary>
    /// Get distance in moves between two fields.
    /// </summary>
    /// <param name="from">Start field ID.</param>
    /// <param name="to">Target field ID.</param>
    /// <returns>Move count. 0 if move is not possible.</returns>
    int CalcMoveDistance(int from, int to);

    /// <summary>
    /// Calculate the shortest distance out of multiple fields to a target field.
    /// </summary>
    /// <param name="target">Target field.</param>
    /// <param name="fields">Array of (movable!) player fields.</param>
    /// <returns>A FieldDistance object.</returns>
    FieldDistance CalcShortestPath(int target, List<int> field);

    /// <summary>
    /// Check if any non-mill stones of a certain player exist.
    /// </summary>
    /// <param name="player">The player.</param>
    /// <returns>True if free stones exist, false if not.</returns>
    bool ExistFreeStones(FieldState player);

    bool HasOpenMills(FieldState player);
}

public class BoardNode
{
    // (0 for empty (default), 1 for player 1 (white), 2 for player 2 (black))
    public FieldState state = FieldState.Empty;
    public List<int> neighbors;

    public BoardNode(List<int> neighbors)
    {
        this.neighbors = neighbors;
    }

    public void SetState(FieldState state)
    {
        this.state = state;
    }
}

public class MillModel : IMillModel
{
    // Dictionary containing BoardNode (with state) and its key/ID
    public Dictionary<int, BoardNode> gameBoard = new Dictionary<int, BoardNode>();
    private Dictionary<int, List<int[]>> millsByNode;
    private Dictionary<FieldState, int> availableStones;
    public Dictionary<int, BoardNode> GameBoard { get => gameBoard; set => gameBoard = value; }
    public Dictionary<FieldState, int> AvailableStones { get => availableStones; set => availableStones = value; }
    public List<int>[] neighborNodes;

    // all possible mills, each containing 3 node IDs
    static readonly int[][] mills =
    {
        new[] {0, 1, 2},
        new[] {3, 4, 5},
        new[] {6, 7, 8},
        new[] {9, 10, 11},
        new[] {12, 13, 14},
        new[] {15, 16, 17},
        new[] {18, 19, 20},
        new[] {21, 22, 23},
        new[] {21, 9, 0},
        new[] {18, 10, 3},
        new[] {15, 11, 6},
        new[] {22, 19, 16},
        new[] {7, 4, 1},
        new[] {17, 12, 8},
        new[] {20, 13, 5},
        new[] {23, 14, 2},
    };

    public MillModel()
    {
        InitializeBoard();
    }

    public void UpdateField(int key, FieldState state)
    {
        GameBoard[key].SetState(state);
    }

    // create gameboard model
    public void InitializeBoard()
    {
        // assign neighbor nodes to each node (only direct neighbors to model movement paths)
        neighborNodes = new List<int>[24];
        neighborNodes[0] = new List<int> { 1, 9 };
        neighborNodes[1] = new List<int> { 0, 2, 4 };
        neighborNodes[2] = new List<int> { 1, 14 };
        neighborNodes[3] = new List<int> { 10, 4 };
        neighborNodes[4] = new List<int> { 3, 7, 1, 5 };
        neighborNodes[5] = new List<int> { 4, 13 };
        neighborNodes[6] = new List<int> { 11, 7 };
        neighborNodes[7] = new List<int> { 6, 4, 8 };
        neighborNodes[8] = new List<int> { 7, 12 };
        neighborNodes[9] = new List<int> { 21, 0, 10 };
        neighborNodes[10] = new List<int> { 9, 11, 18, 3 };
        neighborNodes[11] = new List<int> { 10, 15, 6 };
        neighborNodes[12] = new List<int> { 17, 8, 13 };
        neighborNodes[13] = new List<int> { 12, 20, 5, 14 };
        neighborNodes[14] = new List<int> { 23, 13, 2 };
        neighborNodes[15] = new List<int> { 11, 16 };
        neighborNodes[16] = new List<int> { 15, 19, 17 };
        neighborNodes[17] = new List<int> { 16, 12 };
        neighborNodes[18] = new List<int> { 19, 10 };
        neighborNodes[19] = new List<int> { 18, 22, 20, 16 };
        neighborNodes[20] = new List<int> { 19, 13 };
        neighborNodes[21] = new List<int> { 9, 22 };
        neighborNodes[22] = new List<int> { 21, 19, 23 };
        neighborNodes[23] = new List<int> { 22, 14 };

        // lookup table containing a node and its possible mills
        millsByNode = new Dictionary<int, List<int[]>>();

        availableStones = new Dictionary<FieldState, int>
        {
            { FieldState.Player, 9 },
            { FieldState.Enemy, 9 }
        };

        foreach (var mill in mills)
        {
            foreach (var node in mill)
            {
                if (!millsByNode.TryGetValue(node, out var list))
                {
                    list = new List<int[]>();
                    millsByNode[node] = list;
                }
                list.Add(mill);
            }
        }

        // fill board with nodes
        for (int i = 0; i < 24; i++)
        {
            GameBoard.Add(i, new BoardNode(neighborNodes[i]));
        }
    }

    public List<int> GetNeighbors(int key)
    {
        return neighborNodes[key];
    }

    public int[][] GetPossibleMills() { return mills; }

    public bool CheckForMill(int key, FieldState player)
    {
        // get all possible mills
        var possibleMills = millsByNode[key];

        bool CheckAllNodes(int[] mill)
        {
            foreach (int node in mill)
            {
                if (GameBoard[node].state != player)
                {
                    return false;
                }
            }
            return true;
        }

        foreach (var mill in possibleMills)
        {
            if (CheckAllNodes(mill))
            {
                return true;
            }
        }
        return false;
    }

    public bool AreNeighbors(int key1, int key2)
    {
        foreach (var node in neighborNodes[key1])
        {
            if (node == key2)
            {
                return true;
            }
        }
        return false;
    }

    public List<int> GetFieldsByState(FieldState state)
    {
        List<int> list = new List<int>();
        foreach (var point in gameBoard)
        {
            if (point.Value.state == state)
            {
                list.Add(point.Key);
            }
        }
        return list;
    }

    public List<int> GetMovableFields(FieldState state)
    {
        List<int> list = new List<int>();
        foreach (var point in gameBoard)
        {
            if (point.Value.state == state && gameBoard[point.Key].neighbors.Any(x => gameBoard[x].state == FieldState.Empty))
            {
                list.Add(point.Key);
            }
        }
        return list;
    }

    public Dictionary<int, int[]> GetAlmostMills(FieldState state)
    {
        Dictionary<int, int[]> dict = new Dictionary<int, int[]>();
        foreach (var mill in mills)
        {
            int[] fullNodes = new int[2];
            int emptyNode = new int();
            int index = 0;
            bool hasEmpty = false;
            foreach (int node in mill)
            {
                if (gameBoard[node].state == state && index < fullNodes.Length)
                {
                    fullNodes[index] = node;
                    index++;
                }
                else if (gameBoard[node].state == FieldState.Empty)
                {
                    emptyNode = node;
                    hasEmpty = true;
                }
            }
            if (index == 2 && hasEmpty && !dict.Keys.Contains(emptyNode))
            {
                dict.Add(emptyNode, fullNodes);
            }
        }
        return dict;
    }

    public List<int> GetPossibleMillFields(FieldState state)
    {
        List<int> list = new List<int>();
        foreach (var mill in mills)
        {
            List<int> fullNodes = new List<int>();
            List<int> emptyNodes = new List<int>();
            foreach (int node in mill)
            {
                if (gameBoard[node].state == state)
                {
                    fullNodes.Add(node);
                }
                else if (gameBoard[node].state == FieldState.Empty)
                {
                    emptyNodes.Add(node);
                }
            }
            if (fullNodes.Count == 2 && emptyNodes.Count == 1)
            {
                list.Add(emptyNodes[0]);
            }
        }
        return list;
    }

    public List<int> GetBlockedMillFields(FieldState state)
    {
        List<int> list = new List<int>();
        foreach (var mill in mills)
        {
            List<int> fullNodes = new List<int>();
            List<int> blockedNodes = new List<int>();
            foreach (int node in mill)
            {
                if (gameBoard[node].state == state)
                {
                    fullNodes.Add(node);
                }
                else if (gameBoard[node].state == FieldState.Player)
                {
                    blockedNodes.Add(node);
                }
            }
            if (fullNodes.Count == 2 && blockedNodes.Count == 1)
            {
                list.Add(blockedNodes[0]);
            }
        }
        return list;
    }

    public int CalcMoveDistance(int from, int to)
    {
        List<int> currentLayer = new List<int> { from };
        List<int> nextLayer = new List<int>();
        List<int> visitedNodes = new List<int>() { from };
        int distance = 0;

        while (currentLayer.Count > 0)
        {
            foreach (int node in currentLayer)
            {
                if (node == to)
                {
                    return distance;
                }

                foreach (int neighbor in neighborNodes[node])
                {
                    if (gameBoard[neighbor].state == FieldState.Empty && !visitedNodes.Contains(neighbor))
                    {
                        nextLayer.Add(neighbor);
                        visitedNodes.Add(neighbor);
                    }
                }
            }

            currentLayer = nextLayer;
            nextLayer = new List<int>();
            distance++;
        }

        return 0;
    }

    /// <summary>
    /// Get distance in moves between two fields.
    /// </summary>
    /// <param name="from">Start field ID.</param>
    /// <param name="to">Target field ID.</param>
    /// <returns>Move count. 0 if move is not possible.</returns>
    public List<int[]> GetMillsByPlayer(FieldState player)
    {
        List<int[]> mills = new List<int[]>();

        foreach (var possibleMill in mills)
        {
            bool full = true;
            foreach (int field in possibleMill)
            {
                if (gameBoard[field].state != player)
                {
                    full = false;
                }
            }
            if (full)
            {
                mills.Add(possibleMill);
            }
        }

        return mills;
    }

    public bool ExistFreeStones(FieldState player)
    {
        foreach (var field in GetFieldsByState(player))
        {
            if (CheckForMill(field, player))
            {
                return true;
            }
        }
        return true;
    }

    public bool HasOpenMills(FieldState player)
    {
        foreach (var mill in GetAlmostMills(player))
        {
            int emptyField = mill.Value.First(m => gameBoard[m].state == FieldState.Empty);
            if (GetNeighbors(emptyField).Any(n => gameBoard[n].state == player))
            {
                return true;
            }
        }
        return false;
    }

    public FieldDistance CalcShortestPath(int target, List<int> fields)
    {
        int field = 0;
        int minDistance = int.MaxValue;
        foreach(int f in fields)
        {
            int dist = CalcMoveDistance(f, target);
            if( dist < minDistance)
            {
                field = f;
                minDistance = dist;
            }
        }
        
        return new FieldDistance(field, minDistance);
    }
}