using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public SquareScript[] board;
    char[] boardAsChars;
    public char[] boardChars {
        get {
            if (boardAsChars == null)
            {
                boardAsChars = BoardToCharArray(board);
            }
            return boardAsChars;
        }
        set {
            boardAsChars = value;
        }
    }
    public List<PieceScript> removedWhitePieces;
    public List<PieceScript> removedBlackPieces;
    public List<PieceScript> activeWhitePieces;
    public List<PieceScript> activeBlackPieces;

    private static BoardManager instance;
    public static BoardManager Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<BoardManager>();
            }
            return instance;
        }
    }
    char[] initialBoard;

    public char[] InitialBoard {
        get {
            if (initialBoard == null)
            {
                initialBoard = BoardToCharArray(board);
            }
            return initialBoard;
        }
    }
    //public functions
    public List<PieceScript> GetActiveAIPieces()
    {
        switch (GameManager.Instance.playerTeam)
        {
            case PieceScript.Team.White:
                return activeBlackPieces;
            case PieceScript.Team.Black:
                return activeWhitePieces;
        }
        return null;
    }
    public List<int> GetTeamPieceIndexes(PieceScript.Team team)
    {
        List<int> pieces = new List<int>();
        switch (team)
        {
            case PieceScript.Team.White:
                for (int i = 0; i < activeWhitePieces.Count; i++)
                {
                    pieces.Add(activeWhitePieces[i].index);
                }
                break;
            case PieceScript.Team.Black:
                for (int i = 0; i < activeBlackPieces.Count; i++)
                {
                    pieces.Add(activeBlackPieces[i].index);
                }
                break;
        }
        return pieces;
    }
    /// <summary>
    /// Add a game piece to the board
    /// </summary>
    /// <param name="piece">piece to add</param>
    public void RegisterPiece(PieceScript piece)
    {
        switch (piece.team)
        {
            case PieceScript.Team.White:
                instance.activeWhitePieces.Add(piece);
                break;
            case PieceScript.Team.Black:
                instance.activeBlackPieces.Add(piece);
                break;
        }
    }
    /// <summary>
    /// Remove a game piece from play
    /// </summary>
    /// <param name="piece">piece to remove</param>
    public void RemovePiece(PieceScript piece)
    {
        switch (piece.team)
        {
            case PieceScript.Team.White:
                instance.removedWhitePieces.Add(piece);
                instance.activeWhitePieces.Remove(piece);
                break;
            case PieceScript.Team.Black:
                instance.removedBlackPieces.Add(piece);
                instance.activeBlackPieces.Remove(piece);
                break;
        }
        piece.gameObject.SetActive(false);
    }

    public void MakeMove(int from, int to)
    {        
        if (from >= 0 && from < board.Length && to >= 0 && to < board.Length) 
        {
            //Physical
            print("moving from " + board[from].name + " to " + board[to].name);
            board[from].LinkedPiece.MoveToSquare(board[to]);

            //Virtual
            int x = boardChars.Length;
            boardChars[to] = boardChars[from];
            boardChars[from] = '\0';

            TurnManager.Instance.EndTurn();
        }
    }

    //Public static Functions
    public static int PositionToBoardIndex(Vector2 position)
    {
        return (int)((position.y - 1) * 8 + (position.x - 1));
    }
    public static Vector2 BoardIndexToPosition(int index)
    {
        return new Vector2((index % 8) + 1, (index / 8) + 1);
    }

    public static char[] BoardToCharArray(SquareScript[] b)
    {
        char[] newB = new char[b.Length];
        for (int i = 0; i < newB.Length; i++)
        {
            if (b[i].LinkedPiece == null)
            {
                newB[i] = '\0';
            }
            else
            {
                switch (b[i].LinkedPiece.type)
                {
                    case PieceScript.Type.Pawn:
                        newB[i] = 'p';
                        break;
                    case PieceScript.Type.Rook:
                        newB[i] = 'r';
                        break;
                    case PieceScript.Type.Bishop:
                        newB[i] = 'b';
                        break;
                    case PieceScript.Type.Knight:
                        newB[i] = 'n';
                        break;
                    case PieceScript.Type.Queen:
                        newB[i] = 'q';
                        break;
                    case PieceScript.Type.King:
                        newB[i] = 'k';
                        break;
                }
                if (b[i].LinkedPiece.team == GameManager.Instance.playerTeam)
                {
                    newB[i] = char.ToUpper(newB[i]);
                }
            }
        }
        return newB;
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        removedBlackPieces = new List<PieceScript>();
        removedWhitePieces = new List<PieceScript>();
        activeBlackPieces = new List<PieceScript>();
        activeWhitePieces = new List<PieceScript>();
    }

    private void Update()
    {
        if (initialBoard == null)
        {
            initialBoard = BoardToCharArray(board);
        }
        if (boardAsChars == null)
        {
            boardAsChars = BoardToCharArray(board);
        }
        
    }

}
