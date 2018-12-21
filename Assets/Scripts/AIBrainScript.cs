using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript : MonoBehaviour {

    List<Move> movesQueue;

    List<PieceScript> myPieces;

	// Use this for initialization
	void Start () {
        movesQueue = new List<Move>();
    }

	bool locked = false;

	// Update is called once per frame
	void Update () {
		//Thinking
        if (!locked)
        {
            myPieces = GameManager.Instance.ActiveAIPieces();
            locked = true;
            foreach (PieceScript piece in myPieces)
            {
                List<SquareScript> possibleMoves = MoveValidator.FindValidMoves(piece);
                print(piece.team + " " + piece.type + "has " + possibleMoves.Count + " possible moves");
            }
        }

        //Acting
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            if (movesQueue.Count > 0)
            {
                MakeMove(movesQueue[0]);
            }

            TurnManager.Instance.EndTurn();
        }
	}

    void Prioritize()
    {
        movesQueue.Sort((x, y) => x.postMoveFitness.CompareTo(y.postMoveFitness));
    }

    void MakeMove(Move nextMove)
    {

    }
}

internal class Move : MonoBehaviour
{
    public int preMoveFitness = int.MinValue;
    public int postMoveFitness = int.MinValue;
    public Move(List<SquareScript> oldBoard, PieceScript pieceToMove, SquareScript squareToMoveTo)
    {

    }
}
