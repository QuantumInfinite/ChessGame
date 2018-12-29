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
        public Material BLOCK;

        public Material GetMaterial(PieceScript.Type pieceType, PieceScript.Team team)
        {
            switch (pieceType)
            {
                case PieceScript.Type.Pawn:
                    return team == PieceScript.Team.White ? white_pawn : black_pawn;
                case PieceScript.Type.Rook:
                    return team == PieceScript.Team.White ? white_rook : black_rook;
                case PieceScript.Type.Bishop:
                    return team == PieceScript.Team.White ? white_bishop : black_bishop;
                case PieceScript.Type.Knight:
                    return team == PieceScript.Team.White ? white_knight : black_knight;
                case PieceScript.Type.Queen:
                    return team == PieceScript.Team.White ? white_queen : black_queen;
                case PieceScript.Type.King:
                    return team == PieceScript.Team.White ? white_king : black_king;
                case PieceScript.Type.BLOCK:
                    return BLOCK;
                default:
                    return null;
            }
        }
    }

    [Header("Materials")]
    public BoardMaterials boardMaterials;
    public PieceMaterials pieceMaterials;
    public GameObject basePiecePrefab;

    [Header("Game Options")]
    public PieceScript.Team playerTeam;
    public bool playerOnlyTurns = false;

    [HideInInspector]
    public PieceScript.Team aiTeam;

    [Header("AI Settings")]
    public int movesAheadToSimulate = 0;

    [Header("Stuff ill move at some point")]
    public TextMesh outputBox;
    static TextMesh output;

    //Static Functions

    //Public Functions
    public void Output(string s)
    {
        output.text += s + "\n";
    }

    //Private functions

    //Keyword functions
    private void Start()
    {
        output = outputBox;
        output.text = "";
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        switch (playerTeam)
        {
            case PieceScript.Team.White:
                aiTeam = PieceScript.Team.Black;
                break;
            case PieceScript.Team.Black:
                aiTeam = PieceScript.Team.White;
                break;
        }
        
    }
}
