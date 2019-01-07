﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript : MonoBehaviour
{

    //List<Move_alt> movesQueue;   

    int maxThinkDepth {
        get {
            return GameManager.Instance.movesAheadToSimulate;
        }
    }
    enum ThinkingStage
    {
        Not,
        Thinking,
        Done
    }
    ThinkingStage thinkingStage = ThinkingStage.Not;
    List<Move> rootMoves;

    // Called once
    private void Start()
    {
        rootMoves = new List<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            if (thinkingStage == ThinkingStage.Not)
            {
                Think();
            }
            else if (thinkingStage == ThinkingStage.Done)
            {
                Act(rootMoves);
            }
        }
    }
    /// <summary>
    /// Thinks about what moves to make
    /// </summary>
    void Think()
    {
        thinkingStage = ThinkingStage.Thinking;
        
        //MiniMax
        
        //float start = Time.realtimeSinceStartup;
        //char[] currentBoard = BoardManager.Instance.boardChars;

        //List<Move_alt> movesQueue = MiniMax(currentBoard, 0, true);
        //OutputPaths("MiniMax", movesQueue, Time.realtimeSinceStartup - start);
        //rootMoves = Prioritize(movesQueue, true);
        
        //AlphaBeta
        float startTime = Time.realtimeSinceStartup;

        rootMoves = AlphaBetaPrune(BoardManager.Instance.boardChars, 0, true, float.MinValue, float.MaxValue);
        DEBUG_OutputPaths("Alpha-Beta", rootMoves, Time.realtimeSinceStartup - startTime);
        
        //if (movesQueue[0].pathFitness != movesQueue1[0].pathFitness)
        //{
        //    Debug.LogError("Minimax and Alpha-Beta produced fitness different results");
        //}

        thinkingStage = ThinkingStage.Done;
    }

    List<Move> MiniMax(char[] currentBoard, int thinkIndex, bool AiTurn)
    {
        List<Move> movesQueue = GenerateNextMoves(currentBoard, AiTurn);
        
        if (thinkIndex >= maxThinkDepth)
        {
            return Prioritize(movesQueue, AiTurn);
        }

        //Recursive part
        foreach (Move move in movesQueue)
        {
            List<Move> nextMoves = MiniMax(move.newBoard, thinkIndex + 1, !AiTurn);
            if (nextMoves.Count > 0)
            {
                move.nextMove = nextMoves[0];
                move.pathFitness = nextMoves[0].pathFitness;
            }
        }
        return Prioritize(movesQueue, AiTurn);

    }

    List<Move> AlphaBetaPrune(char[] currentBoard, int thinkIndex, bool AiTurn, float alpha, float beta)
    {
        List<Move> children = GenerateNextMoves(currentBoard, AiTurn);

        if (thinkIndex >= maxThinkDepth)
        {
            return Prioritize(children, AiTurn);
        }        
        
        float bestVal = (AiTurn) ? float.MinValue : float.MaxValue;

        foreach (Move child in children)
        {
            List<Move> childrensMoves = AlphaBetaPrune(child.newBoard, thinkIndex + 1, !AiTurn, alpha, beta);
            if (childrensMoves.Count > 0)
            {
                child.nextMove = childrensMoves[0];
                child.pathFitness = child.nextMove.pathFitness;
                float value = child.pathFitness;
                if (AiTurn)
                {
                    bestVal = Mathf.Max(bestVal, value);
                    alpha = Mathf.Max(alpha, bestVal);
                }
                else
                {
                    bestVal = Mathf.Min(bestVal, value);
                    beta = Mathf.Min(beta, bestVal);
                }

                if (beta <= alpha)
                {
                    break;
                }
            }
        }        

        return Prioritize(children, AiTurn);
    }

    List<Move> GenerateNextMoves(char[] currentBoard, bool AiTurn)
    {
        List<Move> movesQueue = new List<Move>();

        List<int> myPieces;

        if (AiTurn)
        {
            myPieces = BoardManager.GetTeamPieceIndexes(currentBoard, GameManager.Instance.aiTeam);
        }
        else
        {
            myPieces = BoardManager.GetTeamPieceIndexes(currentBoard, GameManager.Instance.playerTeam);
        }

        foreach (int pieceIndex in myPieces)
        {
            List<int> possibleMoves = MoveValidator.FindValidMoves(pieceIndex, currentBoard);
            
            foreach (int move in possibleMoves)
            {
                movesQueue.Add(new Move(currentBoard, pieceIndex, move));
            }            
        }
        return movesQueue = Prioritize(movesQueue, AiTurn);
    }

    void Act(List<Move> movesQueue)
    {
        if (thinkingStage == ThinkingStage.Done)
        {
            if (movesQueue.Count > 0)
            {
                MakeMove(movesQueue[0]);
            }
            else
            {
                Debug.LogAssertion("AI has no valid moves");
            }
            thinkingStage = ThinkingStage.Not;

            movesQueue.Clear();
        }
    }
    /// <summary>
    /// Sorts the given list max to min if maximise, min to max if !maximise
    /// Sort function ensures that the moves are in the correct order.
    /// (x.from + x.to) / 100 just adds a small amount to each to help with ordering
    /// </summary>
    /// <param name="list"></param>
    /// <param name="maximise"></param>
    /// <returns></returns>
    List<Move> Prioritize(List<Move> list, bool maximise)
    {
        if (maximise)
        {
            list.Sort(
                (y, x) => (
                    x.pathFitness + x.self_fitness
                ).CompareTo(
                    y.pathFitness + y.self_fitness
                )
            );
        }
        else
        {
            list.Sort(
                (x,y) => (
                    x.pathFitness + x.self_fitness
                ).CompareTo(
                    y.pathFitness + y.self_fitness
                )
            );
        }
        return list;
    }
    /// <summary>
    /// Instructs the board manager to make the given move
    /// </summary>
    /// <param name="nextMove">Move to make</param>
    void MakeMove(Move nextMove)
    {
        BoardManager.Instance.MakeMove(nextMove.from, nextMove.to);
    }
    /// <summary>
    /// Outputs paths for debugging
    /// </summary>
    /// <param name="funcName">the name of the algorthm</param>
    /// <param name="movesQueue">the list of moves</param>
    /// <param name="exeTime">time it took to execute</param>
    void DEBUG_OutputPaths(string funcName, List<Move> movesQueue, float exeTime)
    {
        string output = (funcName + " took " + exeTime + " and decided on " + BoardManager.BoardIndexToCoordinate(movesQueue[0].from) + " -> " + BoardManager.BoardIndexToCoordinate(movesQueue[0].to) + "\n");
        foreach (Move move in movesQueue)
        {
            output += BoardManager.BoardIndexToCoordinate(move.from) + " -> " + BoardManager.BoardIndexToCoordinate(move.to) + " ";
            Move ptr = move;
            while (ptr.nextMove != null)
            {
                ptr = ptr.nextMove;
                output += ", " + BoardManager.BoardIndexToCoordinate(ptr.from) + " -> " + BoardManager.BoardIndexToCoordinate(ptr.to);
            }
            output += " : " + move.self_fitness + " " + move.pathFitness + "\n";
        }
        print(output);
    }

}

internal class Move
{
    public float self_fitness = 0;
    public float pathFitness = 0;

    //public char[] oldBoard;
    public char[] newBoard;

    public int from;

    public int to;

    public Move nextMove;
    //public Move_alt(char[] oldBoard, int pieceToMove, int squareToMoveTo) : this(oldBoard, pieceToMove, squareToMoveTo, null) { }

    public Move(char[] oldBoard, int pieceToMove, int squareToMoveTo)
    {
        //this.oldBoard = oldBoard;
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

        //Remove old piece
        newBoard[pieceToMove] = '\0';

        //Set Self Fitness
        self_fitness = FitnessEvaluator.Evaluate(newBoard);

        pathFitness = self_fitness;
    }
}
