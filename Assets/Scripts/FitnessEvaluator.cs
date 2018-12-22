using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessEvaluator : MonoBehaviour {

    public static float rawPieceValue = 1;

    public int pawnValue = 10, knightValue = 30, bishopValue = 30, rookValue = 50, queenValue = 90, kingValue = 900;
    public static int pawn, knight, bishop, rook, queen, king;

    public static float Evaluate(char[] board)
    {
        float score = 0;
        foreach (char piece in board)
        {
            if (piece != '\0')
            {
                score += (char.IsUpper(piece)) ? rawPieceValue : -rawPieceValue;               
            }
        }

        return score;
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
