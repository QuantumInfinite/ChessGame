﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public float self_fitness = 0;
    public float pathFitness = 0;

    //public char[] oldBoard;
    public char[] newBoard;
    public int from;

    public int to;

    public Move nextMove;

    public int enpassentIndex = -1;
    public PieceScript.Type promotionType = PieceScript.Type.Pawn;

    public char movedPiece {
        get {
            return newBoard[to];
        }
    }

    /// <summary>
    /// Moves a piece to another place on the board
    /// </summary>
    /// <param name="oldBoard"> the board before the piece is moved </param>
    /// <param name="pieceToMove"> the piece to be moved on the board </param>
    /// <param name="squareToMoveTo"> yhe square to move the piece to </param>
    public Move(char[] oldBoard, int pieceToMove, int squareToMoveTo)
    {
        from = pieceToMove;
        to = squareToMoveTo;

        newBoard = new char[oldBoard.Length];


        for (int i = 0; i < newBoard.Length; i++)
        {
            if (i == squareToMoveTo)
            {
                //moves piece to new place
                newBoard[i] = oldBoard[pieceToMove];
            }
            else
            {
                //Just copy it over
                newBoard[i] = oldBoard[i];
            }
        }

        //Enpassant check
        if (to == BoardManager.Instance.enPassentIndex && char.ToUpper(oldBoard[from]) == 'P')
        {
            if (from > to && to + 8 < newBoard.Length)
            {
                //Moving downward
                newBoard[to + 8] = '\0';
                enpassentIndex = to + 8;
            }
            else if (to - 8 >= 0)
            {
                //Moving upward
                newBoard[to - 8] = '\0';
                enpassentIndex = to - 8;
            }
        }

        //Promotion Check
        if ((char.ToUpper(movedPiece) == 'P') && (to > 55 || to < 8))
        {
            //Promoted
            if (TurnManager.Instance.IsPlayerTurn())
            {
                //Prompt player for which piece they want
            }
            else
            {
                //Choose queen, as statistcally it is the best choice
                newBoard[to] = (GameManager.Instance.playerTeam == PieceScript.Team.White) ? 'q' : 'Q';
                promotionType = PieceScript.Type.Queen;
            }
        }

        //Remove old piece
        newBoard[pieceToMove] = '\0';


        //Set Self Fitness
        self_fitness = FitnessEvaluator.Evaluate(newBoard);

        pathFitness = self_fitness;
    }
}
