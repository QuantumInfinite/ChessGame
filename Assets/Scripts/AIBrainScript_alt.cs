using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AIBrainScript_alt : MonoBehaviour {

    List<Move_alt> movesQueue;

    List<PieceScript> myPieces;


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
        myPieces = BoardManager.Instance.ActiveAIPieces();
        foreach (PieceScript piece in myPieces)
        {
            List<int> possibleMoves = MoveValidator_alt.FindValidMoves(BoardManager.PositionToBoardIndex(piece.position), BoardManager.BoardToCharArray(BoardManager.Instance.board));

            if (possibleMoves.Count > 0)
            {
                foreach (int move in possibleMoves)
                {
                    movesQueue.Add(new Move_alt(BoardManager.BoardToCharArray(BoardManager.Instance.board), BoardManager.PositionToBoardIndex(piece.position), move));
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
        movesQueue.Sort((x, y) => x.fitness.CompareTo(y.fitness));
    }

    void MakeMove(Move_alt nextMove)
    {

        string t = "";
        foreach (Move_alt move in movesQueue)
        {
            t += " " + move.fitness; 
        }
        print(t);
        BoardManager.Instance.board[nextMove.piece].LinkedPiece.MoveToSquare(BoardManager.Instance.board[nextMove.square]);
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
