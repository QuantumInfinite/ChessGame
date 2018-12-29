using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FitnessEvaluator : MonoBehaviour {

    public static float rawPieceValue = 1;

    public int pawnValue = 10, knightValue = 30, bishopValue = 30, rookValue = 50, queenValue = 90, kingValue = 900;
    public static int pawn, knight, bishop, rook, queen, king;

    public static float Evaluate(char[] board)
    {
        float score = 0;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != '\0')
            {
                score += GetValue(board[i]);             
            }
        }
        return score;
    }

    static int GetValue(char piece)
    {
        int val = 0;
        switch (char.ToLower(piece))
        {
            case 'p':
                val = pawn;
                break;
            case 'r':
                val = rook;
                break;
            case 'b':
                val = bishop;
                break;
            case 'n':
                val = knight;
                break;
            case 'q':
                val = queen;
                break;
            case 'k':
                val = king;
                break;
        }
        return char.IsLower(piece) ? val : -val;
    }
    static char GetChar(PieceScript.Type piece)
    {
        switch (piece)
        {
            case PieceScript.Type.Pawn:
                return  'p';
            case PieceScript.Type.Rook:
                return  'r';
            case PieceScript.Type.Bishop:
                return  'b';
            case PieceScript.Type.Knight:
                return  'n';
            case PieceScript.Type.Queen:
                return  'q';
            case PieceScript.Type.King:
                return  'k';
        }
        return '\0';
    }

    private void Awake()
    {
        pawn = pawnValue;
        knight = knightValue;
        bishop = bishopValue;
        rook = rookValue;
        queen = queenValue;
        king = kingValue;
    }
}
