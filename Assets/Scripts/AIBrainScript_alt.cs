using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript_alt : MonoBehaviour
{

    //List<Move_alt> movesQueue;   

    int maxThinkDepth {
        get {
            return GameManager.Instance.movesAheadToSimulate;
        }
    }

    int numThinks;
    int numThinks1;
    enum ThinkingStage
    {
        Not,
        Thinking,
        Done
    }
    ThinkingStage thinkingStage = ThinkingStage.Not;
    List<Move_alt> rootMoves;
    private void Start()
    {
        rootMoves = new List<Move_alt>();
    }
    // Update is called once per frame
    void Update()
    {
        List<Move_alt> movesQueue = rootMoves;
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            if (thinkingStage == ThinkingStage.Not)
            {
                Think();
            }
            else if (thinkingStage == ThinkingStage.Done)
            {
                Act(movesQueue);
            }
        }
    }
    float thinkTime = 0.0f;
    void Think()
    {
        thinkingStage = ThinkingStage.Thinking;

        thinkTime = 0.0f;
        numThinks = 0;
        numThinks1 = 0;

        //MiniMax
        float start = Time.realtimeSinceStartup;

        char[] currentBoard = BoardManager.Instance.boardChars;

        List<Move_alt> movesQueue = MiniMax(currentBoard, 0, true);
        rootMoves = Prioritize(movesQueue, true);

        thinkTime = Time.realtimeSinceStartup - start;
        print("MiniMax took " + thinkTime + ", analysed " + numThinks + " moves, and decided on " + BoardManager.BoardIndexToCoordinate(movesQueue[0].from) + " -> " + BoardManager.BoardIndexToCoordinate(movesQueue[0].to));

        //AlphaBeta
        float start1 = Time.realtimeSinceStartup;

        char[] currentBoard1 = BoardManager.Instance.boardChars;

        List<Move_alt> movesQueue1 = AlphaBetaPrune(currentBoard1, 0, true, float.MinValue, float.MaxValue);
        //rootMoves = Prioritize(movesQueue, true);

        thinkTime = Time.realtimeSinceStartup - start1;
        print("AlphaBeta took " + thinkTime + ", analysed " + numThinks1 + " moves, and decided on " + BoardManager.BoardIndexToCoordinate(movesQueue1[0].from) + " -> " + BoardManager.BoardIndexToCoordinate(movesQueue1[0].to));


        thinkingStage = ThinkingStage.Done;
    }

    List<Move_alt> MiniMax(char[] currentBoard, int thinkIndex, bool AiTurn)
    {
        List<Move_alt> movesQueue = new List<Move_alt>();

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
            List<int> possibleMoves = MoveValidator_alt.FindValidMoves(pieceIndex, currentBoard);

            if (possibleMoves.Count > 0)
            {
                foreach (int move in possibleMoves)
                {
                    numThinks++;
                    movesQueue.Add(new Move_alt(currentBoard, pieceIndex, move));

                }
            }
        }
        movesQueue = Prioritize(movesQueue, AiTurn);


        //Do alpha-beta prune here
        int best = (AiTurn) ? int.MinValue : int.MaxValue;


        if (thinkIndex >= maxThinkDepth)
        {
            return movesQueue;
        }

        //Recursive part
        foreach (Move_alt move in movesQueue)
        {
            List<Move_alt> nextMoves = MiniMax(move.newBoard, thinkIndex + 1, !AiTurn);
            if (nextMoves.Count > 0)
            {
                move.pathFitness = nextMoves[0].pathFitness;
            }
        }
        return movesQueue;

    }

    List<Move_alt> AlphaBetaPrune(char[] currentBoard, int thinkIndex, bool AiTurn, float alpha, float beta)
    {
        List<Move_alt> children = GenerateNextMoves(currentBoard, AiTurn);

        if (thinkIndex >= maxThinkDepth)
        {
            return children;
        }        
        
        float bestVal = (AiTurn) ? float.MinValue : float.MaxValue;

        foreach (Move_alt child in children)
        {
            List<Move_alt> childrensMoves = AlphaBetaPrune(child.newBoard, thinkIndex + 1, !AiTurn, alpha, beta);
            if (childrensMoves.Count > 0)
            {
                child.pathFitness = childrensMoves[0].pathFitness;
                float value = childrensMoves[0].pathFitness;
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

    List<Move_alt> GenerateNextMoves(char[] currentBoard, bool AiTurn)
    {
        List<Move_alt> movesQueue = new List<Move_alt>();

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
            numThinks1++;
            List<int> possibleMoves = MoveValidator_alt.FindValidMoves(pieceIndex, currentBoard);

            if (possibleMoves.Count > 0)
            {
                foreach (int move in possibleMoves)
                {
                    movesQueue.Add(new Move_alt(currentBoard, pieceIndex, move));
                }
            }
        }
        return movesQueue = Prioritize(movesQueue, AiTurn);
    }

    void Act(List<Move_alt> movesQueue)
    {
        if (thinkingStage == ThinkingStage.Done)
        {
            if (movesQueue.Count > 0)
            {
                //print("AI has " + movesQueue.Count + " possible moves");

                string t = numThinks + " moves analysed in " + thinkTime + " seconds \n";
                foreach (Move_alt move in movesQueue)
                {
                    t += BoardManager.BoardIndexToCoordinate(move.from) + " -> " + BoardManager.BoardIndexToCoordinate(move.to) + " " + move.self_fitness + " " + move.pathFitness + "\n";
                }
                print(t);

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
    /// Sort function ensures that 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="maximise"></param>
    /// <returns></returns>
    List<Move_alt> Prioritize(List<Move_alt> list, bool maximise)
    {
        if (maximise)
        {
            list.Sort(
                (y, x) => (
                    x.pathFitness + x.self_fitness + ((x.from + x.to) / 100)
                ).CompareTo(
                    y.pathFitness + y.self_fitness + ((y.from + y.to) / 100)
                )
            );
        }
        else
        {
            list.Sort(
                (x,y) => (
                    x.pathFitness + x.self_fitness + ((x.from + x.to) / 100)
                ).CompareTo(
                    y.pathFitness + y.self_fitness + ((y.from + y.to) / 100)
                )
            );
        }
        return list;
    }

    void MakeMove(Move_alt nextMove)
    {
        BoardManager.Instance.MakeMove(nextMove.from, nextMove.to);
    }
}

internal class Move_alt
{
    public float self_fitness = 0;
    public float pathFitness = 0;

    public char[] oldBoard;
    public char[] newBoard;

    public int from;

    public int to;

    public Move_alt(char[] oldBoard, int pieceToMove, int squareToMoveTo)
    {
        this.oldBoard = oldBoard;
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

    public char ConvertToLetter(PieceScript piece)
    {
        char letter = new char();
        switch (piece.type)
        {
            case PieceScript.Type.Pawn:
                letter = 'p';
                break;
            case PieceScript.Type.Rook:
                letter = 'r';
                break;
            case PieceScript.Type.Bishop:
                letter = 'b';
                break;
            case PieceScript.Type.Knight:
                letter = 'n';
                break;
            case PieceScript.Type.Queen:
                letter = 'q';
                break;
            case PieceScript.Type.King:
                letter = 'k';
                break;
        }
        if (piece.team == GameManager.Instance.playerTeam)
        {
            letter = char.ToUpper(letter);
        }
        return letter;
    }
}
