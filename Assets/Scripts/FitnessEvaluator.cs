﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FitnessEvaluator : MonoBehaviour {

    public static float rawPieceValue = 1;

    public int pawnValue = 10, knightValue = 30, bishopValue = 30, rookValue = 50, queenValue = 90, kingValue = 900;

    static int pawn, knight, bishop, rook, queen, king;

    static int[] pawnValuesBlack;
    static int[] pawnValuesWhite = {
        0,  0,  0,  0,  0,  0,  0,  0,
        50, 50, 50, 50, 50, 50, 50, 50,
        10, 10, 20, 30, 30, 20, 10, 10,
         5,  5, 10, 25, 25, 10,  5,  5,
         0,  0,  0, 20, 20,  0,  0,  0,
         5, -5,-10,  0,  0,-10, -5,  5,
         5, 10, 10,-20,-20, 10, 10,  5,
         0,  0,  0,  0,  0,  0,  0,  0
    };
    static int[] knightValuesBlack;
    static int[] knightValuesWhite = {
        -50,-40,-30,-30,-30,-30,-40,-50,
        -40,-20,  0,  0,  0,  0,-20,-40,
        -30,  0, 10, 15, 15, 10,  0,-30,
        -30,  5, 15, 20, 20, 15,  5,-30,
        -30,  0, 15, 20, 20, 15,  0,-30,
        -30,  5, 10, 15, 15, 10,  5,-30,
        -40,-20,  0,  5,  5,  0,-20,-40,
        -50,-40,-30,-30,-30,-30,-40,-50,
    };
    static int[] bishopValuesBlack;
    static int[] bishopValuesWhite = {
        -20,-10,-10,-10,-10,-10,-10,-20,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -10,  0,  5, 10, 10,  5,  0,-10,
        -10,  5,  5, 10, 10,  5,  5,-10,
        -10,  0, 10, 10, 10, 10,  0,-10,
        -10, 10, 10, 10, 10, 10, 10,-10,
        -10,  5,  0,  0,  0,  0,  5,-10,
        -20,-10,-10,-10,-10,-10,-10,-20,
    };
    static int[] rookValuesBlack;
    static int[] rookValuesWhite = {
        0,  0,  0,  0,  0,  0,  0,  0,
        5, 10, 10, 10, 10, 10, 10,  5,
       -5,  0,  0,  0,  0,  0,  0, -5,
       -5,  0,  0,  0,  0,  0,  0, -5,
       -5,  0,  0,  0,  0,  0,  0, -5,
       -5,  0,  0,  0,  0,  0,  0, -5,
       -5,  0,  0,  0,  0,  0,  0, -5,
        0,  0,  0,  5,  5,  0,  0,  0
    };
    static int[] queenValuesBlack;
    static int[] queenValuesWhite = {
        -20,-10,-10, -5, -5,-10,-10,-20,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -10,  0,  5,  5,  5,  5,  0,-10,
         -5,  0,  5,  5,  5,  5,  0, -5,
          0,  0,  5,  5,  5,  5,  0, -5,
        -10,  5,  5,  5,  5,  5,  0,-10,
        -10,  0,  5,  0,  0,  0,  0,-10,
        -20,-10,-10, -5, -5,-10,-10,-20
    };
    static int[] kingValuesBlack;
    static int[] kingValuesWhite = {
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -20,-30,-30,-40,-40,-30,-30,-20,
        -10,-20,-20,-20,-20,-20,-20,-10,
         20, 20,  0,  0,  0,  0, 20, 20,
         20, 30, 10,  0,  0, 10, 30, 20
    };

    public static float Evaluate(char[] board)
    {
        float score = 0;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] != '\0')
            {
                if (GameManager.Instance.usePositionalScore)
                {
                    score += GetValuePositional(board[i], i);
                }
                else
                {
                    score += GetValue(board[i]);   
                }
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

    static int GetValuePositional(char piece, int positionIndex)
    {
        int val = 0;
        if (char.IsLower(piece))
        {
            switch (char.ToLower(piece))
            {
                case 'p':
                    val = pawnValuesBlack[positionIndex];
                    break;
                case 'r':
                    val = rookValuesBlack[positionIndex];
                    break;
                case 'b':
                    val = bishopValuesBlack[positionIndex];
                    break;
                case 'n':
                    val = knightValuesBlack[positionIndex];
                    break;
                case 'q':
                    val = queenValuesBlack[positionIndex];
                    break;
                case 'k':
                    val = kingValuesBlack[positionIndex];
                    break;
            }
        }
        else
        {
            switch (char.ToLower(piece))
            {
                case 'p':
                    val = pawnValuesWhite[positionIndex];
                    break;
                case 'r':
                    val = rookValuesWhite[positionIndex];
                    break;
                case 'b':
                    val = bishopValuesWhite[positionIndex];
                    break;
                case 'n':
                    val = knightValuesWhite[positionIndex];
                    break;
                case 'q':
                    val = queenValuesWhite[positionIndex];
                    break;
                case 'k':
                    val = kingValuesWhite[positionIndex];
                    break;
            }
        }
        return val;
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

        pawnValuesBlack = pawnValuesWhite.Reverse().ToArray();
        bishopValuesBlack = bishopValuesWhite.Reverse().ToArray();
        rookValuesBlack = rookValuesWhite.Reverse().ToArray();
        knightValuesBlack = knightValuesWhite.Reverse().ToArray();
        queenValuesBlack = queenValuesWhite.Reverse().ToArray();
        kingValuesBlack = kingValuesWhite.Reverse().ToArray();


    }
}
