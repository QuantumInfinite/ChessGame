using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.UI;

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
    public GameObject MainMenu;
    public InputField inputField;

    [Header("Game Options")]
    public PieceScript.Team playerTeam;
    public bool playerOnlyTurns = false;
    public bool inputBoard = true;
    [ConditionalHide("inputBoard", true)]
    public string input = "";


    [HideInInspector]
    public PieceScript.Team aiTeam;

    [Header("AI Settings")]
    public int movesAheadToSimulate = 0;

    public bool limitThinkTime;
    [ConditionalHide("limitThinkTime", true)]
    public float maxThinkTime = 1.0f;

    public bool usePositionalScore = false;

    [Header("Output")]
    public TextMesh outputBox;
    static TextMesh output;

    //Static Functions

    //Public Functions
    public static void Output(string s)
    {
        output.text += TurnManager.Instance.CurrentTurn() +  s + "\n";
    }
    public void ApplyInput()
    {
        if (inputBoard && input.Length != 0)
        {
            List<char> chars = new List<char>();

            List<string> inputParse = input.Split(',').ToList();

            inputParse.RemoveAll(x => x == ",");

            List<char> inputAsChars = inputParse.SelectMany(item => item.ToCharArray()).ToList();

            Regex rx = new Regex("[kqnbrpKQNBRP]");
            foreach (char c in inputAsChars)
            {
                if (rx.IsMatch(c.ToString()))
                {
                    chars.Add(c);
                }
                else
                {
                    chars.Add('\0');
                }
            }
            if (chars.Count == BoardManager.Instance.boardChars.Length)
            {
                BoardManager.Instance.OverrideBoard(chars.ToArray());
            }

        }
    }
    public void Begin()
    {
        MainMenu.SetActive(false);
        string basic = "R,N,B,Q,K,B,N,R,P,P,P,P,P,P,P,P, , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , , ,p,p,p,p,p,p,p,p,r,n,b,q,k,b,n,r";
        print(inputField.text);
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
