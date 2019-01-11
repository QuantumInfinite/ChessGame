using System.Collections;
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
    float thinkStartTime = 0.0f;
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
        thinkStartTime = Time.realtimeSinceStartup;

        if (GameManager.Instance.limitThinkTime)
        {
            //int index = 0;
            //while (Time.realtimeSinceStartup - thinkStartTime < 0.95f * GameManager.Instance.maxThinkTime && index <= maxThinkDepth)
            //{
            //    rootMoves = AlphaBetaPrune(BoardManager.Instance.boardChars, index, true, float.MinValue, float.MaxValue);
                
            //    index++;
            //}


            //DEBUG_OutputPaths("Iterative timed Alpha-Beta", rootMoves, Time.realtimeSinceStartup - thinkStartTime, index - 1);

            //thinkStartTime = Time.realtimeSinceStartup;
            rootMoves = AlphaBetaPruneTimed(BoardManager.Instance.boardChars, maxThinkDepth, true, float.MinValue, float.MaxValue);
            DEBUG_OutputPaths("Timed Alpha-Beta", rootMoves, Time.realtimeSinceStartup - thinkStartTime, maxThinkDepth);
        }
        else
        {
            rootMoves = AlphaBetaPrune(BoardManager.Instance.boardChars, maxThinkDepth, true, float.MinValue, float.MaxValue);
            DEBUG_OutputPaths("Alpha-Beta", rootMoves, Time.realtimeSinceStartup - thinkStartTime, maxThinkDepth);
        }




        thinkingStage = ThinkingStage.Done;
    }
    
    List<Move> AlphaBetaPrune(char[] currentBoard, int depth, bool AiTurn, float alpha, float beta)
    {
        List<Move> children = GenerateNextMoves(currentBoard, AiTurn);
        
        if (depth <= 0)
        {
            return Prioritize(children, AiTurn);
        }        
        
        float bestVal = (AiTurn) ? float.MinValue : float.MaxValue;

        foreach (Move child in children)
        {
            List<Move> childrensMoves = AlphaBetaPrune(child.newBoard, depth - 1, !AiTurn, alpha, beta);
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
    
    List<Move> AlphaBetaPruneTimed(char[] currentBoard, int depth, bool AiTurn, float alpha, float beta)
    {
        List<Move> children = GenerateNextMoves(currentBoard, AiTurn);
        if (depth <= 0 || (GameManager.Instance.limitThinkTime && Time.realtimeSinceStartup - thinkStartTime > 0.95f * GameManager.Instance.maxThinkTime))
        {
            return Prioritize(children, AiTurn);
        }

        float bestVal = (AiTurn) ? float.MinValue : float.MaxValue;

        foreach (Move child in children)
        {
            List<Move> childrensMoves = AlphaBetaPruneTimed(child.newBoard, depth - 1, !AiTurn, alpha, beta);
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
        BoardManager.Instance.MakeMove(nextMove);
    }
    /// <summary>
    /// Outputs paths for debugging
    /// </summary>
    /// <param name="funcName">the name of the algorthm</param>
    /// <param name="movesQueue">the list of moves</param>
    /// <param name="exeTime">time it took to execute</param>
    void DEBUG_OutputPaths(string funcName, List<Move> movesQueue, float exeTime, int depthReached)
    {
        if (movesQueue.Count == 0)
        {
            return;
        }
        string output = (funcName + " took " + exeTime + " to reach depth " + depthReached + " and decided on " + BoardManager.BoardIndexToCoordinate(movesQueue[0].from) + " -> " + BoardManager.BoardIndexToCoordinate(movesQueue[0].to) + "\n");
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


