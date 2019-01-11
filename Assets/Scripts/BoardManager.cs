using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public List<Move> moveHistory = new List<Move>();
    public int enPassentIndex = -1;

    //public functions


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

    public void MakeMove(Move move)
    {
        while (moveHistory.Count > 5)
        {
            moveHistory.RemoveAt(0);
        }
        moveHistory.Add(move);
        if (char.ToUpper(move.movedPiece) == 'P')
        {
            enPassentIndex = move.to - 8;
        }
        //Castleing checks
        if (board[move.from].LinkedPiece.type == PieceScript.Type.King && move.from == 4 && move.to == 6)
        {
            Castle(4, 7, true);
        }

        else if (board[move.from].LinkedPiece.type == PieceScript.Type.King && move.from == 4 && move.to == 2)
        {
            Castle(4, 0, false);
        }

        else if (board[move.from].LinkedPiece.type == PieceScript.Type.King && move.from == 60 && move.to == 62)
        {
            Castle(60, 63, true);
        }

        else if (board[move.from].LinkedPiece.type == PieceScript.Type.King && move.from == 60 && move.to == 57)
        {
            Castle(60, 56, false);
        }
        //Not castling
        else if (move.from >= 0 && move.from < board.Length && move.to >= 0 && move.to < board.Length)
        {
            //Log
            LogMove(move);

            //Physical
            board[move.from].LinkedPiece.MoveToSquare(board[move.to]);
                //en-passant
            if (move.enpassentIndex >= 0 && board[move.enpassentIndex].LinkedPiece != null)
            {
                RemovePiece(board[move.enpassentIndex].LinkedPiece);
            }
                //Promotion
            if(move.promotionType != PieceScript.Type.Pawn && (move.to > 55 || move.to < 8))
            {
                RemovePiece(board[move.to].LinkedPiece);
                if (char.IsUpper(move.movedPiece))
                {
                    board[move.to].SpawnPiece(move.promotionType, GameManager.Instance.playerTeam);
                }
                else
                {
                    board[move.to].SpawnPiece(move.promotionType, GameManager.Instance.aiTeam);
                }

                //Virtual
                boardChars[move.to] = BoardManager.GetCharFromPieceScript(board[move.to].LinkedPiece);
                boardChars[move.from] = '\0';
            }
            else
            {
                //Virtual
                boardChars[move.to] = boardChars[move.from];
                boardChars[move.from] = '\0';
            }
            TurnManager.Instance.EndTurn();
        }
        else
        {
            Debug.LogError("Invalid move " + move.from + "->" + move.to);
        }

    }

    public void LogMove(Move move)
    {
        string output = "";
        bool isPawn = false || char.ToUpper(move.movedPiece) == 'P';

        //is not pawn and promotion is not happening
        if (!isPawn && move.promotionType == PieceScript.Type.Pawn)
        {
            output += GetCharFromPieceScript(board[move.from].LinkedPiece);
        }

        //capture
        if (board[move.to].LinkedPiece != null || move.enpassentIndex >= 0)
        {
            //pawn making capture
            if (isPawn)
            {
                output += char.ToLower(board[move.from].name[0]);
            }

            output += "x";
        }
        //position
        output += board[move.to].name.ToLower();

        //en passant
        if (move.enpassentIndex >= 0)
        {
            output += "e.p.";
        }

        //Promotion
        if (move.promotionType != PieceScript.Type.Pawn)
        {
            output +=move.newBoard[move.to];
        }


        GameManager.Output(output);
    }

    public void Castle(int piece1Index, int piece2Index, bool kingSide)
    {
        if (piece1Index >= 0 && piece1Index < board.Length && piece2Index >= 0 && piece2Index < board.Length)
        {
            //Log
            if (kingSide)
            {
                GameManager.Output("0-0");
            }
            else
            {
                GameManager.Output("0-0-0");
            }

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

    public void OverrideBoard(char[] newBoard)
    {
        for (int i = 0; i < Instance.boardChars.Length; i++)
        {
            Instance.boardChars[i] = newBoard[i];
            if(newBoard[i] != '\0')
            {
                if (char.IsUpper(newBoard[i]))
                {
                    Instance.board[i].SpawnPiece(GetPiecetypeFromChar(newBoard[i]), GameManager.Instance.playerTeam);
                }
                else
                {
                    Instance.board[i].SpawnPiece(GetPiecetypeFromChar(newBoard[i]), GameManager.Instance.aiTeam);
                }
            }
        }
        initialBoard = boardAsChars;
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
                newB[i] = GetCharFromPieceScript(b[i].LinkedPiece);
                if (b[i].LinkedPiece.team == GameManager.Instance.playerTeam)
                {
                    newB[i] = char.ToUpper(newB[i]);
                }
            }
        }
        return newB;
    }

    public static PieceScript.Type GetPiecetypeFromChar(char c)
    {
        switch (char.ToUpper(c))
        {
            case 'P':
                return PieceScript.Type.Pawn;
            case 'R':
                return PieceScript.Type.Rook;
            case 'B':
                return PieceScript.Type.Bishop;
            case 'Q':
                return PieceScript.Type.Queen;
            case 'N':
                return PieceScript.Type.Knight;
            case 'K':
                return PieceScript.Type.King;
        }
        return PieceScript.Type.BLOCK;
    }

    public static char GetCharFromPieceScript(PieceScript piece)
    {
        char t = '\0';
        switch (piece.type)
        {
            case PieceScript.Type.Pawn:
                t = 'p';
                break;
            case PieceScript.Type.Rook:
                t = 'r';
                break;
            case PieceScript.Type.Bishop:
                t = 'b';
                break;
            case PieceScript.Type.Knight:
                t = 'n';
                break;
            case PieceScript.Type.Queen:
                t = 'q';
                break;
            case PieceScript.Type.King:
                t = 'k';
                break;
            case PieceScript.Type.BLOCK:
                t = 'X';
                break;
        }

        return (piece.team == GameManager.Instance.playerTeam) ? char.ToUpper(t) : char.ToLower(t);
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
