using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript_alt : MonoBehaviour {

    //List<Move_alt> movesQueue;   

    int maxThinkDepth {
        get {
            return GameManager.Instance.movesAheadToSimulate;
        }
    }

    int numThinks;
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
    void Update () {
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

        float start = Time.realtimeSinceStartup;

        char[] currentBoard = BoardManager.Instance.boardChars;

        List<Move_alt> movesQueue = DeepThink(currentBoard, 0, true);
        rootMoves = Prioritize(movesQueue, true);

        thinkTime = Time.realtimeSinceStartup - start;

        thinkingStage = ThinkingStage.Done;
    }

    List<Move_alt> DeepThink( char[] currentBoard, int thinkIndex, bool AiTurn)
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
            numThinks++;
            List<int> possibleMoves = MoveValidator_alt.FindValidMoves(pieceIndex, currentBoard);
            
            if (possibleMoves.Count > 0)
            {
                foreach (int move in possibleMoves)
                {
                    movesQueue.Add(new Move_alt(currentBoard, pieceIndex, move));
                }
            }
        }
        movesQueue = Prioritize(movesQueue, AiTurn);


        //Do alpha-beta prune here


        if (thinkIndex >= maxThinkDepth)
        {
            return movesQueue;
        }

        //Recursive part
        foreach (Move_alt move in movesQueue)
        {
            List<Move_alt> nextMoves = DeepThink(move.newBoard, thinkIndex + 1, !AiTurn);
            if (nextMoves.Count > 0)
            {
                move.totalFitness = nextMoves[0].totalFitness;
            }
        }
        return movesQueue;
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
                    t += BoardManager.BoardIndexToCoordinate(move.from) + " -> " + BoardManager.BoardIndexToCoordinate(move.to) + " " + move.self_fitness + " " + move.totalFitness + "\n";
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

    List<Move_alt> Prioritize(List<Move_alt> list, bool maximise)
    {
        if (maximise)
        {
            list.Sort((y, x) => (x.totalFitness + x.self_fitness).CompareTo(y.totalFitness + y.self_fitness));
        }
        else
        {
            list.Sort((x, y) => (x.totalFitness - x.self_fitness).CompareTo(y.totalFitness - y.self_fitness));
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
    public float totalFitness = 0;

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

        totalFitness = self_fitness;
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
