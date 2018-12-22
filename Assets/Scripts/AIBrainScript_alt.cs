using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript_alt : MonoBehaviour {

    List<Move_alt> movesQueue;

    List<int> myPieces;


    public int thinkDepth;
    int numThinks = 0;
	// Use this for initialization
	void Start () {
        movesQueue = new List<Move_alt>();
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
        if (!TurnManager.Instance.IsPlayerTurn())
        {
            if (thinkingStage == ThinkingStage.Not)
            {
                Think();
            }
            else if (thinkingStage == ThinkingStage.Done)
            {
                Act();
            }
        }
    }
    void Think()
    {
        thinkingStage = ThinkingStage.Thinking;
        myPieces = new List<int>();
        char[] currentBoard = BoardManager.Instance.boardChars;
        for (int i = 0; i < currentBoard.Length; i++)
        {
            if (char.IsLower(currentBoard[i])){
                myPieces.Add(i);
            }
        }
        
        foreach (int pieceIndex in myPieces)
        {
            List<int> possibleMoves = MoveValidator_alt.FindValidMoves(pieceIndex, BoardManager.Instance.boardChars);

            if (possibleMoves.Count > 0)
            {
                foreach (int move in possibleMoves)
                {
                    movesQueue.Add(new Move_alt(currentBoard,pieceIndex, move));
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

                string t = "";
                foreach (Move_alt move in movesQueue)
                {
                    t += " " + move.fitness;
                }
                print(t);

                MakeMove(movesQueue[0]);
            }
            numThinks = 0;
            thinkingStage = ThinkingStage.Not;
            movesQueue.Clear();
        }
    }
    void Prioritize()
    {
        movesQueue.Sort((x, y) => x.fitness.CompareTo(y.fitness));
    }

    void MakeMove(Move_alt nextMove)
    {
        BoardManager.Instance.MakeMove(nextMove.piece, nextMove.square);
    }
}

internal class Move_alt
{
    public float fitness = 0;

    char[] oldBoard;
    char[] newBoard;

    public int piece;

    public int square;

    public Move_alt(char[] oldBoard, int pieceToMove, int squareToMoveTo)
    {
        this.oldBoard = oldBoard;
        piece = pieceToMove;
        square = squareToMoveTo;
        

        newBoard = new char[oldBoard.Length];
        for (int i = 0; i < newBoard.Length; i++)
        {
            if (i == squareToMoveTo)
            {
                newBoard[i] = oldBoard[pieceToMove];
            }
            else if (i == pieceToMove || oldBoard[i] == '\0')
            {
                newBoard[i] = '\0';
            }
            else
            {
                newBoard[i] = oldBoard[i];
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
