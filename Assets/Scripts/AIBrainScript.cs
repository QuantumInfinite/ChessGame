using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript : MonoBehaviour {

    List<Move> movesQueue;

    List<PieceScript> myPieces;


    public int maxThinks = 1;
    public int minThinks = 1;
    int numThinks = 0;
	// Use this for initialization
	void Start () {
        movesQueue = new List<Move>();
    }

	

	// Update is called once per frame
	void Update () {
		//Thinking
        if (numThinks < maxThinks)
        {
            //For now until we figure out static variables
            //if (!TurnManager.Instance.IsPlayerTurn())
            {
                myPieces = GameManager.Instance.ActiveAIPieces();
                numThinks++;
                foreach (PieceScript piece in myPieces)
                {
                    List<SquareScript> possibleMoves = MoveValidator.FindValidMoves(piece);
                    if (possibleMoves.Count > 0)
                    {
                        foreach (SquareScript square in possibleMoves)
                        {
                            movesQueue.Add(new Move(GameManager.Instance.board.ToList(), piece, square));
                        }
                    }
                }
                Prioritize();
                //print("AI has " + movesQueue.Count + " possible moves");
            }
        }

        //Acting
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            if (numThinks >= minThinks)
            {
                if (movesQueue.Count > 0)
                {
                    MakeMove(movesQueue[0]);
                }
                numThinks = 0;
                TurnManager.Instance.EndTurn();
            }
        }
	}

    void Prioritize()
    {
        movesQueue.Sort((x, y) => x.postMoveFitness.CompareTo(y.postMoveFitness));
    }

    void MakeMove(Move nextMove)
    {
        print("moving " + nextMove.piece.team + " " + nextMove.piece.type + " from " + nextMove.piece.LinkedSquare.name + " to " + nextMove.square.name);
        nextMove.piece.MoveToSquare(nextMove.square);
    }
}

internal class Move
{
    public float preMoveFitness = float.MinValue;
    public float postMoveFitness = float.MinValue;

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
