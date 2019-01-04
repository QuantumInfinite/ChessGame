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
    public static List<int> GetTeamPieceIndexes(char[] board, PieceScript.Team team)
    {
        List<int> pieces = new List<int>();
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == '\0')
            {
                continue;
            }
            
            if ((team == GameManager.Instance.playerTeam && char.IsUpper(board[i])) ||
                (team == GameManager.Instance.aiTeam && char.IsLower(board[i])))
            {
                pieces.Add(i);
            }
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
        if (board[from].LinkedPiece.type == PieceScript.Type.King && from == 4 && to == 6)
        {
            Castle(4, 7, true);
        }

        else if (board[from].LinkedPiece.type == PieceScript.Type.King && from == 4 && to == 1)
        {
            Castle(4, 0, false);
        }

        else if (board[from].LinkedPiece.type == PieceScript.Type.King && from == 60 && to == 62)
        {
            Castle(60, 63, true);
        }

        else if (board[from].LinkedPiece.type == PieceScript.Type.King && from == 60 && to == 57)
        {
            Castle(60, 56, false);
        }

        else if (from >= 0 && from < board.Length && to >= 0 && to < board.Length)
        {
            //Physical
            GameManager.Instance.Output(board[from].name + "->" + board[to].name);
            board[from].LinkedPiece.MoveToSquare(board[to]);

            //Virtual
            int x = boardChars.Length;
            boardChars[to] = boardChars[from];
            boardChars[from] = '\0';


            TurnManager.Instance.EndTurn();
        }
        else
        {
            Debug.LogError("Invalid move " + from + "->" + to);
        }
    }

    public void Castle(int piece1Index, int piece2Index, bool kingSide)
    {
        if (piece1Index >= 0 && piece1Index < board.Length && piece2Index >= 0 && piece2Index < board.Length)
        {
            //Log
            if (kingSide)
            {
                GameManager.Instance.Output("0-0");
            }
            else
            {
                GameManager.Instance.Output("0-0-0");
            }

            //Physical
            /*
            SquareScript s1 = board[piece1Index];
            PieceScript p1 = s1.LinkedPiece;
            SquareScript s2 = board[piece2Index];
            PieceScript p2 = s2.LinkedPiece;

            Vector2 p1Pos = p1.transform.position;
            p1.transform.position = p2.transform.position;
            p2.transform.position = p1Pos;

            p1.SetSquare(s2);
            p2.SetSquare(s1);

            s1.SetPiece(p2);
            s2.SetPiece(p1);*/

            if (piece2Index == 7)
            {
                board[4].LinkedPiece.MoveToSquare(board[6]);
                board[7].LinkedPiece.MoveToSquare(board[5]);
            }
            else if (piece2Index == 0)
            {
                board[4].LinkedPiece.MoveToSquare(board[2]);
                board[0].LinkedPiece.MoveToSquare(board[3]);
            }
            else if (piece2Index == 63)
            {
                board[60].LinkedPiece.MoveToSquare(board[62]);
                board[63].LinkedPiece.MoveToSquare(board[61]);
            }
            else if (piece2Index == 56)
            {
                board[60].LinkedPiece.MoveToSquare(board[58]);
                board[56].LinkedPiece.MoveToSquare(board[59]);
            }

            //Virtual
            char c1 = boardChars[piece1Index];
            boardChars[piece1Index] = boardChars[piece2Index];
            boardChars[piece2Index] = c1;

            TurnManager.Instance.EndTurn();
        }
        else
        {
            Debug.LogError("Invalid swap " + piece1Index + "->" + piece2Index);
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
    public static string BoardIndexToCoordinate(int index)
    {
        return "" + (char)(65 + index % 8) + (index / 8 + 1);
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
                    case PieceScript.Type.BLOCK:
                        newB[i] = 'X';
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
