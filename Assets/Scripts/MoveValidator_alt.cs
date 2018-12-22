using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator_alt : MonoBehaviour {

    private static int currentPiece;
    private static bool canPlace = true;

    public static List<int> FindValidMoves(int pieceIndex, char[] board)
    {
        List<int> validMoves = new List<int>();
        //SquareScript[] currentBoard = BoardManager.Instance.board;
        currentPiece = pieceIndex;
        int initPos = pieceIndex;
        switch (char.ToUpper(board[pieceIndex]))
        {
            case 'P':
                if (char.IsLower(board[pieceIndex]))
                {
                    //one space forward
                    if (board[initPos - 8] == '\0')
                    {
                        MarkMove(validMoves, board, initPos - 8);
                    }
                    //two spaces if first turn
                    if (BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[initPos - 16] == '\0')
                    {
                        if (board[initPos - 8] == '\0')
                        {
                            MarkMove(validMoves, board, initPos - 16);
                        }
                    }
                    if (board[initPos - 7] != '\0')
                    {
                        MarkMove(validMoves, board, initPos - 7);
                    }
                    if (board[initPos - 9] != '\0')
                    {
                        MarkMove(validMoves, board, initPos - 9);
                    }
                }
                else
                {
                    //one space forward
                    if (board[initPos + 8] == '\0')
                    {
                        MarkMove(validMoves, board, initPos + 8);
                    }
                    //two spaces if first turn
                    if (BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[initPos + 16] == '\0')
                    {
                        if (board[initPos + 8] == '\0')
                        {
                            MarkMove(validMoves, board, initPos + 16);
                        }
                    }
                    if (board[initPos + 7] != '\0')
                    {
                        MarkMove(validMoves, board, initPos + 7);
                    }
                    if (board[initPos + 9] != '\0')
                    {
                        MarkMove(validMoves, board, initPos + 9);
                    }
                }
                canPlace = true;
                break;
            case 'R':
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMove(validMoves, board,initPos + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMove(validMoves, board, initPos - i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((initPos + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                        if ((initPos + i + 1) % 8 == 0)
                        {
                            break;
                        }
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //left row
                for (int i = 1; i < board.Length; i++)
                {
                    if (initPos % 8 != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                        if ((initPos - i) % 8 == 0)
                        {
                            break;
                        }
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                UnmarkMove(validMoves, board, initPos);
                canPlace = true;
                break;
            case 'B':

                //up-left diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                    }
                    if ((initPos + i + 1) % 8 == 0 && (initPos + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, initPos + i);
                        break;
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //down-right diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                    }
                    if ((initPos - i + 1) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //up-right diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                    }
                    if ((initPos + i + 1) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //down-left diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                    }
                    if ((initPos - i) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                UnmarkMove(validMoves, board, initPos);
                break;
            case 'N':
                int rowsThrough = 0;

                //one down two right
                for (int i = 0; i < 6; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        if (initPos % 8 == 0)
                        {
                            rowsThrough++;
                        }
                        MarkMove(validMoves, board,initPos - 6);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //one up two left
                for (int i = 0; i < 6; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos + i + 1) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos + 6);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //one up two right
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (initPos + 2) % 8 == 0)
                    {
                        rowsThrough = rowsThrough + 6;
                    }
                    else if (i == 9 && initPos % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos + 10);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //one down two left
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (initPos + 1) % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,initPos - 10);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //two up one left
                for (int i = 0; i < 15; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && initPos % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos + 15);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                for (int j = 1; j < board.Length; j = j + 8)
                {
                    if (initPos == j)
                    {
                        MarkMove(validMoves, board, initPos + 15);
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //two down one right
                for (int i = 0; i < 15; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos - 15);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //two up one right
                for (int i = 0; i < 17; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && (initPos + 1) % 8 == 0)
                    {
                        rowsThrough = rowsThrough + 6;
                    }
                    else if (i == 16 && initPos % 8 == 0)
                    {
                        rowsThrough = 2;
                    }
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos + 17);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                //two down one left
                for (int i = 0; i < 17; i++)
                {
                    if (i == 0)
                    {
                        rowsThrough = 0;
                    }
                    if ((initPos - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,initPos - 17);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                break;
            case 'Q':

                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMove(validMoves, board, initPos + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMove(validMoves, board, initPos - i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((initPos + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board,initPos + i);
                        if ((initPos + i + 1) % 8 == 0)
                        {
                            break;
                        }
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //left row
                for (int i = 1; i < board.Length; i++)
                {
                    if (initPos % 8 != 0)
                    {
                        MarkMove(validMoves, board,initPos - i);
                        if ((initPos - i) % 8 == 0)
                        {
                            break;
                        }
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //up left diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                    }
                    if ((initPos + i + 1) % 8 == 0 && (initPos + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, initPos + i);
                        break;
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //down right diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                    }
                    if ((initPos - i + 1) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //up right diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos + i);
                    }
                    if ((initPos + i + 1) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //down left diagonal
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, initPos - i);
                    }
                    if ((initPos - i) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                UnmarkMove(validMoves, board, initPos);
                canPlace = true;
                break;
            case 'K':
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
                canPlace = true;
                break;
        }
        return validMoves;
    }

    private static List<int> MarkMove(List<int> validMoves, char[] board, int index)
    {
        if (index >= 0 && index < board.Length && !validMoves.Contains(index))
        {
            if (board[index] == '\0')
            {
                validMoves.Add(index);
            }
            else
            {
                if (!(char.IsUpper(board[index]) && char.IsUpper(board[currentPiece])))
                {
	                validMoves.Add(index);
                }
                canPlace = false;
            }
        }
        return validMoves;
    }
                    

    private static List<int> UnmarkMove(List<int> validMoves, char[] board, int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.RemoveAll(item => item == index);
        }
        return validMoves;
    }
}