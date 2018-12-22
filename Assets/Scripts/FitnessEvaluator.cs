using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessEvaluator : MonoBehaviour {

    public static float rawPieceValue = 1;

    public static float Evaluate(char[] board)
    {
        float score = 0;
        foreach (char piece in board)
        {
            if (piece != '\0')
            {
                //score += (char.IsUpper(piece)) ? -rawPieceValue : rawPieceValue;
                if (char.IsUpper(piece))
                {
                    score++;
                }
                else
                {
                    score--;
                }
            }
        }

        return score;
    }

}
