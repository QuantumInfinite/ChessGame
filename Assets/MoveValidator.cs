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

                MarkMove(validMoves, board, initPos + 8);

                if (!heldPiece.HasMoved())
                {
                    MarkMove(validMoves, board, initPos + 16);
                }
                break;
            case PieceScript.PieceType.Rook:
                for (int i = 0; i < board.Length; i = i + 8)
                {
                    MarkMove(validMoves, board, initPos + i);
                    MarkMove(validMoves, board, initPos - i);
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(validMoves, board, initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                break;
            case PieceScript.PieceType.Bishop:

                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if ((initPos + i + 1) % 8 == 0) break;
                    MarkMove(validMoves, board, initPos + i);
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board, initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                UnmarkMove(validMoves, board, initPos);
                break;
            case PieceScript.PieceType.Knight:
                int rowsThrough = 0;

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
                        MarkMove(validMoves, board, initPos - 6);
                    }
                }
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
                        MarkMove(validMoves, board, initPos + 6);
                    }
                }
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
                        MarkMove(validMoves, board, initPos + 10);
                    }
                }
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
                        MarkMove(validMoves, board, initPos - 10);
                    }
                }
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
                        MarkMove(validMoves, board, initPos + 15);
                    }
                }
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
                        MarkMove(validMoves, board, initPos - 15);
                    }
                }
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
                        MarkMove(validMoves, board, initPos + 17);
                    }
                }
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
                        MarkMove(validMoves, board, initPos - 17);
                    }
                }
                break;
            case PieceScript.PieceType.Queen:

                for (int i = 0; i < board.Length; i = i + 8)
                {
                    MarkMove(validMoves, board, initPos + i);
                    MarkMove(validMoves, board, initPos - i);
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(validMoves, board, initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if ((initPos + i + 1) % 8 == 0) break;
                    MarkMove(validMoves, board, initPos + i);
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board, initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(validMoves, board, initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                UnmarkMove(validMoves, board, initPos);
                break;
            case PieceScript.PieceType.King:
                MarkMove(validMoves, board, initPos + 1);
                MarkMove(validMoves, board, initPos - 1);
                MarkMove(validMoves, board, initPos + 7);
                MarkMove(validMoves, board, initPos - 7);
                MarkMove(validMoves, board, initPos + 8);
                MarkMove(validMoves, board, initPos - 8);
                MarkMove(validMoves, board, initPos + 9);
                MarkMove(validMoves, board, initPos - 9);
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
