using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator : MonoBehaviour {

    public static List<BoardSpace> MarkValidMoves(PieceScript heldPiece)
    {

        List<BoardSpace> validMoves = new List<BoardSpace>();
        BoardSpace[] board = GameManager.Instance.board;

        int initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));
        switch (heldPiece.pieceType)
        {
            case PieceScript.PieceType.Pawn:
                //one space forward
                MarkMove(validMoves, board,initPos + 8);

                //two spaces if first turn
                if (!heldPiece.HasMoved())
                {
                    MarkMove(validMoves, board,initPos + 16);
                }
                break;
            case PieceScript.PieceType.Rook:
                for (int i = 0; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMove(validMoves, board,initPos + i);
                    //backward column
                    MarkMove(validMoves, board,initPos - i);
                }
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((initPos + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                        if ((initPos + i + 1) % 8 == 0) break;
                    }
                }
                //left row
                for (int i = 1; i < board.Length; i++)
                {
                    if (initPos % 8 != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                        if ((initPos - i) % 8 == 0) break;
                    }
                }
                break;
            case PieceScript.PieceType.Bishop:

                //up-left diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board, initPos + i);
                    if ((initPos + i + 1) % 8 == 0 && (initPos + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, initPos + i);
                        break;
                    }
                }
                //down-right diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board,initPos - i);
                    if ((initPos - i + 1) % 8 == 0) break;
                }
                //up-right diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board,initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                //down-left diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board,initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                UnmarkMove(validMoves, board, initPos);
                break;
            case PieceScript.PieceType.Knight:
                int rowsThrough = 0;

                //one down two right
                for (int i = 0; i < 6; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        if (initPos % 8 == 0)
                            rowsThrough++;
                        MarkMove(validMoves, board,initPos - 6);
                    }
                }
                //one up two left
                for (int i = 0; i < 6; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos + i + 1) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos + 6);
                    }
                }
                //one up two right
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (initPos + 2) % 8 == 0)
                        rowsThrough = rowsThrough + 6;
                    else if (i == 9 && initPos % 8 == 0)
                        rowsThrough = 1;
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos + 10);
                    }
                }
                //one down two left
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (initPos + 1) % 8 == 0)
                        rowsThrough = 1;
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos - 10);
                    }
                }
                //two up one left
                for (int i = 0; i < 15; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && initPos % 8 == 0)
                        rowsThrough = 1;
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos + 15);
                    }
                }
                for (int j = 1; j < board.Length; j = j + 8)
                {
                    if (initPos == j)
                        MarkMove(validMoves, board,initPos + 15);
                }
                //two down one right
                for (int i = 0; i < 15; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos - 15);
                    }
                }
                //two up one right
                for (int i = 0; i < 17; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && (initPos + 1) % 8 == 0)
                        rowsThrough = rowsThrough + 6;
                    else if (i == 16 && initPos % 8 == 0)
                        rowsThrough = 2;
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos + 17);
                    }
                }
                //two down one left
                for (int i = 0; i < 17; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos - 17);
                    }
                }
                break;
            case PieceScript.PieceType.Queen:

                for (int i = 0; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMove(validMoves, board,initPos + i);
                    //backward column
                    MarkMove(validMoves, board,initPos - i);
                }
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((initPos + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board,initPos + i);
                        if ((initPos + i + 1) % 8 == 0) break;
                    }
                }
                //left row
                for (int i = 1; i < board.Length; i++)
                {
                    if (initPos % 8 != 0)
                    {
                        MarkMove(validMoves, board,initPos - i);
                        if ((initPos - i) % 8 == 0) break;
                    }
                }
                //up left diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board,initPos + i);
                    if ((initPos + i + 1) % 8 == 0 && (initPos + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, initPos + i);
                        break;
                    }
                }
                //down right diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board,initPos - i);
                    if ((initPos - i + 1) % 8 == 0) break;
                }
                //up right diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                   MarkMove(validMoves, board,initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                //down left diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board,initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                UnmarkMove(validMoves, board, initPos);
                break;
            case PieceScript.PieceType.King:
                //left movement
                if (initPos % 8 != 0)
                {
                    MarkMove(validMoves, board,initPos - 1);
                    MarkMove(validMoves, board,initPos + 7);
                    MarkMove(validMoves, board,initPos - 9);
                }
                //right movement
                if ((initPos + 1) % 8 != 0)
                {
                    MarkMove(validMoves, board,initPos + 1);
                    MarkMove(validMoves, board,initPos - 7);
                    MarkMove(validMoves, board,initPos + 9);
                }
                //one space up
                MarkMove(validMoves, board,initPos + 8);
                //one space down
                MarkMove(validMoves, board,initPos - 8);
                break;
        }
        return validMoves;
    }

    public static List<BoardSpace> MarkMove(List<BoardSpace> validMoves, BoardSpace[] board, int index)
    {
        if (index >= 0 && index < board.Length && !validMoves.Contains(board[index]))
        {
            validMoves.Add(board[index]);
        }
        return validMoves;
    }

    public static List<BoardSpace> UnmarkMove(List<BoardSpace> validMoves, BoardSpace[] board, int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.RemoveAll(x => x.position == board[index].position);
        }
        return validMoves;
    }
}
