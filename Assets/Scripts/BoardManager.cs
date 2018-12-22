using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public SquareScript[] board;
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

    public List<PieceScript> ActiveAIPieces()
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
    public void RemovePiece(PieceScript piece)
    {
        print(piece.team + " " + piece.type + " Removed from play");
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

    public static int PositionToBoardIndex(Vector2 position)
    {
        return (int)((position.y - 1) * 8 + (position.x - 1));
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
                newB[i] = ConvertToLetter(b[i].LinkedPiece);
            }
        }
        return newB;
    }
    private static char ConvertToLetter(PieceScript piece)
    {
        char letter = new char();
        switch (piece.type)
        {
            case PieceScript.Type.Pawn:
                letter = 'p';
                break;
            case PieceScript.Type.Rook:
                letter = 'r';
                break;
            case PieceScript.Type.Bishop:
                letter = 'b';
                break;
            case PieceScript.Type.Knight:
                letter = 'n';
                break;
            case PieceScript.Type.Queen:
                letter = 'q';
                break;
            case PieceScript.Type.King:
                letter = 'k';
                break;
        }
        if (piece.team == GameManager.Instance.playerTeam)
        {
            letter = char.ToUpper(letter);
        }
        return letter;
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
}
