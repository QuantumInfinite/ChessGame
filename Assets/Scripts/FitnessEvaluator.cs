﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FitnessEvaluator : MonoBehaviour
{
    public static float rawPieceValue = 1;

    public int pawnValue = 10, knightValue = 30, bishopValue = 30, rookValue = 50, queenValue = 90, kingValue = 900;

    static int pawn, knight, bishop, rook, queen, king;

    /// <summary>
    /// Evaluates the numerical value of each space on the board
    /// </summary>
    /// <param name="board"> the board to find the numerical value of </param>
    /// <returns> the value of the board </returns>
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
    /// <summary>
    /// Gets the numerical value of a piece on the board
    /// </summary>
    /// <param name="piece"> the piece to find the value of </param>
    /// <returns> the numerical value of the piece</returns>
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
