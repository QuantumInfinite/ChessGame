using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript : MonoBehaviour {

    List<Move> movesQueue;

    List<PieceScript> myPieces;


    public int thinkDepth;
    int numThinks = 0;
	// Use this for initialization
	void Start () {
        movesQueue = new List<Move>();
    }

    enum ThinkingStage
    {
        Not, 
        Thinking,
        Done
    }
    ThinkingStage thinkingStage = ThinkingStage.Not;

	// Update is called once per frame
	void Update () {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            if (thinkingStage == ThinkingStage.Not)
            {
                Think();
            }
        }
        else
        {
            Act();
        }
	}
    void Think()
    {
        thinkingStage = ThinkingStage.Thinking;
        myPieces = GameManager.Instance.ActiveAIPieces();
        foreach (PieceScript piece in myPieces)
        {
            List<SquareScript> possibleMoves = MoveValidator.FindValidMoves(piece, GameManager.Instance.board);
            if (possibleMoves.Count > 0)
            {
                foreach (SquareScript square in possibleMoves)
                {
                    movesQueue.Add(new Move(GameManager.Instance.board, piece, square));
                }
            }
        }
        Prioritize();
        thinkingStage = ThinkingStage.Done;
    }
    void Act()
    {
        if (thinkingStage == ThinkingStage.Done)
        {
            if (movesQueue.Count > 0)
            {
                //print("AI has " + movesQueue.Count + " possible moves");
                MakeMove(movesQueue[0]);
            }
            numThinks = 0;
            thinkingStage = ThinkingStage.Not;
            movesQueue.Clear();
            TurnManager.Instance.EndTurn();
        }
    }
    void Prioritize()
    {
        movesQueue.Sort((x, y) => y.fitness.CompareTo(x.fitness));
    }

    void MakeMove(Move nextMove)
    {
        print("moving " + nextMove.piece.team + " " + nextMove.piece.type + " from " + nextMove.piece.LinkedSquare.name + " to " + nextMove.square.name);

        string t = "";
        foreach (Move move in movesQueue)
        {
            t += " " + move.fitness; 
        }
        print(t);
        nextMove.piece.MoveToSquare(nextMove.square);
    }
}

internal class Move
{
    public float fitness = 0;

    SquareScript[] oldBoard;
    char[] newBoard;

    public PieceScript piece;

    public SquareScript square;

    public Move(SquareScript[] oldBoard, PieceScript pieceToMove, SquareScript squareToMoveTo)
    {
        this.oldBoard = oldBoard;
        piece = pieceToMove;
        square = squareToMoveTo;

        int pieceIndex = GameManager.PositionToBoardIndex(piece.LinkedSquare.position);
        int squareIndex = GameManager.PositionToBoardIndex(square.position);

        newBoard = new char[oldBoard.Length];
        for (int i = 0; i < newBoard.Length; i++)
        {
            if (i == squareIndex)
            {
                newBoard[i] = ConvertToLetter(pieceToMove);
            }
            else if (i == pieceIndex || oldBoard[i].LinkedPiece == null)
            {
                newBoard[i] = '\0';
            }
            else
            {
                newBoard[i] = ConvertToLetter(oldBoard[i].LinkedPiece);
            }
        }
        fitness = FitnessEvaluator.Evaluate(newBoard);
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
                letter = 'k';
                break;
            case PieceScript.Type.Queen:
                letter = 'q';
                break;
            case PieceScript.Type.King:
                letter = 'x';
                break;
        }
        if (piece.team == GameManager.Instance.playerTeam)
        {
            letter = char.ToUpper(letter);
        }
        return letter;
    }
}
