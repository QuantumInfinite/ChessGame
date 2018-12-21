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

    bool thinking = false;
	// Update is called once per frame
	void Update () {
        if (TurnManager.Instance.IsPlayerTurn())
        {
            if (!thinking)
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
        thinking = true;
        myPieces = GameManager.Instance.ActiveAIPieces();
        foreach (PieceScript piece in myPieces)
        {
            List<SquareScript> possibleMoves = MoveValidator.FindValidMoves(piece, GameManager.Instance.board);
            if (possibleMoves.Count > 0)
            {
                foreach (SquareScript square in possibleMoves)
                {
                    movesQueue.Add(new Move(GameManager.Instance.board.ToList(), piece, square));
                }
            }
        }
        Prioritize();
    }
    void Act()
    {
        if (movesQueue.Count > 0)
        {
            //print("AI has " + movesQueue.Count + " possible moves");
            MakeMove(movesQueue[0]);
        }
        numThinks = 0;
        thinking = false;
        movesQueue.Clear();
        TurnManager.Instance.EndTurn();
    }
    void Prioritize()
    {
        movesQueue.Sort((x, y) => x.fitness.CompareTo(y.fitness));
    }

    void MakeMove(Move nextMove)
    {
        print("moving " + nextMove.piece.team + " " + nextMove.piece.type + " from " + nextMove.piece.LinkedSquare.name + " to " + nextMove.square.name);
        nextMove.piece.MoveToSquare(nextMove.square);
    }
}

internal class Move
{
    public float fitness = float.MinValue;

    List<SquareScript> oldBoard;
    List<SquareScript> newBoard;

    public PieceScript piece;

    public SquareScript square;

    public Move(List<SquareScript> oldBoard, PieceScript pieceToMove, SquareScript squareToMoveTo)
    {
        this.oldBoard = oldBoard;
        piece = pieceToMove;
        square = squareToMoveTo;


    }
}
