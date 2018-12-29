using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator_alt : MonoBehaviour {

    private static int currentPiece;
    private static bool canPlace = true;

    public static List<int> FindValidMoves(int pieceIndex, char[] board)
    {
        if (pieceIndex >= board.Length)
        {
            Debug.LogError("Piece Index Out of Range");
        }
        List<int> validMoves = new List<int>();
        //SquareScript[] currentBoard = BoardManager.Instance.board;
        currentPiece = pieceIndex;
        switch (char.ToUpper(board[pieceIndex]))
        {
            case 'P':
                if (char.IsLower(board[pieceIndex]))
                {
                    //one space forward
                    if (pieceIndex - 8 > 0 && board[pieceIndex - 8] == '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex - 8);
                    }
                    //two spaces if first turn
                    if (pieceIndex - 16 > 0 && BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[pieceIndex - 16] == '\0')
                    {
                        if (board[pieceIndex - 8] == '\0')
                        {
                            MarkMove(validMoves, board, pieceIndex - 16);
                        }
                    }
                    //attack right 
                    if (pieceIndex - 7 < board.Length && (pieceIndex + 1) % 8 != 0 && board[pieceIndex - 7] != '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex - 7);
                    }
                    //attack left
                    if (pieceIndex - 9 < board.Length && pieceIndex % 8 != 0 && board[pieceIndex - 9] != '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex - 9);
                    }
                }
                else
                {
                    //one space forward
                    if (pieceIndex + 8 < board.Length && board[pieceIndex + 8] == '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex + 8);
                    }
                    //two spaces if first turn
                    if (pieceIndex + 16 < board.Length && BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[pieceIndex + 16] == '\0')
                    {
                        if (board[pieceIndex + 8] == '\0')
                        {
                            MarkMove(validMoves, board, pieceIndex + 16);
                        }
                    }
                    //attack left
                    if (pieceIndex + 7 < board.Length && pieceIndex % 8 != 0 && board[pieceIndex + 7] != '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex + 7);
                    }
                    //attack right
                    if (pieceIndex + 9 < board.Length && (pieceIndex+1) % 8 != 0 && board[pieceIndex + 9] != '\0')
                    {
                        MarkMove(validMoves, board, pieceIndex + 9);
                    }
                }
                canPlace = true;
                break;
            case 'R':
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMove(validMoves, board,pieceIndex + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMove(validMoves, board, pieceIndex - i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((pieceIndex + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board, pieceIndex + i);
                        if ((pieceIndex + i + 1) % 8 == 0)
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
                    if (pieceIndex % 8 != 0)
                    {
                        MarkMove(validMoves, board, pieceIndex - i);
                        if ((pieceIndex - i) % 8 == 0)
                        {
                            break;
                        }
                    }
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                UnmarkMove(validMoves, board, pieceIndex);
                canPlace = true;
                break;
            case 'B':

                //up-left diagonal
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if (canPlace == true && i != 0)
                    {
                        MarkMove(validMoves, board, pieceIndex + i);
                    }
                    if ((pieceIndex + i + 1) % 8 == 0 && (pieceIndex + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, pieceIndex + i);
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
                        MarkMove(validMoves, board, pieceIndex - i);
                    }
                    if ((pieceIndex - i + 1) % 8 == 0 || canPlace == false)
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
                        MarkMove(validMoves, board, pieceIndex + i);
                    }
                    if ((pieceIndex + i + 1) % 8 == 0 || canPlace == false)
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
                        MarkMove(validMoves, board, pieceIndex - i);
                    }
                    if ((pieceIndex - i) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                UnmarkMove(validMoves, board, pieceIndex);
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
                    if ((pieceIndex - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        if (pieceIndex % 8 == 0)
                        {
                            rowsThrough++;
                        }
                        MarkMove(validMoves, board,pieceIndex - 6);
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
                    if ((pieceIndex + i + 1) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,pieceIndex + 6);
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
                    if ((pieceIndex + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (pieceIndex + 2) % 8 == 0)
                    {
                        rowsThrough = rowsThrough + 6;
                    }
                    else if (i == 9 && pieceIndex % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,pieceIndex + 10);
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
                    if ((pieceIndex - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (pieceIndex + 1) % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(validMoves, board,pieceIndex - 10);
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
                    if ((pieceIndex + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && pieceIndex % 8 == 0)
                    {
                        rowsThrough = 1;
                    }
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,pieceIndex + 15);
                        if (canPlace == false)
                        {
                            break;
                        }
                    }
                }
                canPlace = true;
                for (int j = 1; j < board.Length; j = j + 8)
                {
                    if (pieceIndex == j)
                    {
                        MarkMove(validMoves, board, pieceIndex + 15);
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
                    if ((pieceIndex - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 14 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,pieceIndex - 15);
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
                    if ((pieceIndex + i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && (pieceIndex + 1) % 8 == 0)
                    {
                        rowsThrough = rowsThrough + 6;
                    }
                    else if (i == 16 && pieceIndex % 8 == 0)
                    {
                        rowsThrough = 2;
                    }
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,pieceIndex + 17);
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
                    if ((pieceIndex - i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 16 && rowsThrough == 2)
                    {
                        MarkMove(validMoves, board,pieceIndex - 17);
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
                    MarkMove(validMoves, board, pieceIndex + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMove(validMoves, board, pieceIndex - i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                //right row
                for (int i = 1; i < board.Length; i++)
                {
                    if ((pieceIndex + 1) % 8 != 0)
                    {
                        MarkMove(validMoves, board,pieceIndex + i);
                        if ((pieceIndex + i + 1) % 8 == 0)
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
                    if (pieceIndex % 8 != 0)
                    {
                        MarkMove(validMoves, board,pieceIndex - i);
                        if ((pieceIndex - i) % 8 == 0)
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
                        MarkMove(validMoves, board, pieceIndex + i);
                    }
                    if ((pieceIndex + i + 1) % 8 == 0 && (pieceIndex + 1) % 8 != 0)
                    {
                        UnmarkMove(validMoves, board, pieceIndex + i);
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
                        MarkMove(validMoves, board, pieceIndex - i);
                    }
                    if ((pieceIndex - i + 1) % 8 == 0 || canPlace == false)
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
                        MarkMove(validMoves, board, pieceIndex + i);
                    }
                    if ((pieceIndex + i + 1) % 8 == 0 || canPlace == false)
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
                        MarkMove(validMoves, board, pieceIndex - i);
                    }
                    if ((pieceIndex - i) % 8 == 0 || canPlace == false)
                    {
                        break;
                    }
                }
                UnmarkMove(validMoves, board, pieceIndex);
                canPlace = true;
                break;
            case 'K':
                //left movement
                if (pieceIndex % 8 != 0)
                {
                    MarkMove(validMoves, board,pieceIndex - 1);
                    MarkMove(validMoves, board,pieceIndex + 7);
                    MarkMove(validMoves, board,pieceIndex - 9);
                }
                //right movement
                if ((pieceIndex + 1) % 8 != 0)
                {
                    MarkMove(validMoves, board,pieceIndex + 1);
                    MarkMove(validMoves, board,pieceIndex - 7);
                    MarkMove(validMoves, board,pieceIndex + 9);
                }
                //one space up
                MarkMove(validMoves, board,pieceIndex + 8);
                //one space down
                MarkMove(validMoves, board,pieceIndex - 8);
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
            {
                if (char.ToUpper(board[index]) == 'X')
                {
                    //Do nothing
                }
                else if (!(char.IsUpper(board[index]) == char.IsUpper(board[currentPiece])))
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