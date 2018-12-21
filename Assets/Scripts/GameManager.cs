using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [System.Serializable]
    public struct BoardMaterials
    {
        public Material white;
        public Material black;
        public Material highlight;
    }
    [System.Serializable]
    public struct PieceMaterials
    {
        public Material white_pawn;
        public Material white_rook;
        public Material white_bishop;
        public Material white_knight;
        public Material white_queen;
        public Material white_king;
        public Material black_pawn;
        public Material black_rook;
        public Material black_bishop;
        public Material black_knight;
        public Material black_queen;
        public Material black_king;

        public Material GetMaterial(PieceScript.PieceType pieceType, PieceScript.Team team)
        {
            switch (pieceType)
            {
                case PieceScript.PieceType.Pawn:
                    return team == PieceScript.Team.White ? white_pawn : black_pawn;
                case PieceScript.PieceType.Rook:
                    return team == PieceScript.Team.White ? white_rook : black_rook;
                case PieceScript.PieceType.Bishop:
                    return team == PieceScript.Team.White ? white_bishop : black_bishop;
                case PieceScript.PieceType.Knight:
                    return team == PieceScript.Team.White ? white_knight : black_knight;
                case PieceScript.PieceType.Queen:
                    return team == PieceScript.Team.White ? white_queen : black_queen;
                case PieceScript.PieceType.King:
                    return team == PieceScript.Team.White ? white_king : black_king;
                default:
                    return null;
            }
        }
    }

    [Header("Materials")]
    public BoardMaterials boardMaterials;
    public PieceMaterials pieceMaterials;
    public GameObject basePiecePrefab;


    [Header("Board Stuff")]
    public SquareScript[] board;
    public List<PieceScript> removedWhitePieces;
    public List<PieceScript> removedBlackPieces;

    [Header("Game Options")]
    public PieceScript.Team playerTeam;
    public bool playerOnlyTurns = false;
    //Static Functions
    public static int PositionToBoardIndex(Vector2 position)
    {
        return (int)((position.y - 1) * 8 + (position.x - 1));
    }

    //Public Functions
    public void RemovePiece(PieceScript piece)
    {
        print(piece.team + " " + piece.pieceType + " Removed from play");
        switch (piece.team)
        {
            case PieceScript.Team.White:
                GameManager.Instance.removedWhitePieces.Add(piece);
                break;
            case PieceScript.Team.Black:
                GameManager.Instance.removedBlackPieces.Add(piece);
                break;
        }
        piece.gameObject.SetActive(false);
    }

    //Private functions

    //Keyword functions
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        removedBlackPieces = new List<PieceScript>();
        removedWhitePieces = new List<PieceScript>();
    }
}
