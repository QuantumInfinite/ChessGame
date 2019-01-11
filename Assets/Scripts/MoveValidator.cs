using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveValidator : MonoBehaviour
{

    private static int currentPiece;
    private static bool canPlace = true;
    private static int whiteKingIndex;
    private static int blackKingIndex;
    private static bool isPlayer;

    /// <summary>
    /// Finds each of the valid moves for a held piece
    /// </summary>
    /// <param name="pieceIndex"> the piece to find the held moves for </param>
    /// <param name="board"> the board to find the moves on </param>
    /// <returns> the list of valid moves the player can make with that piece </returns>
    public static List<int> FindValidMoves(int pieceIndex, char[] board)
    {
        if (pieceIndex >= board.Length)
        {
            Debug.LogError("Piece Index Out of Range");
        }
        List<int> validMoves = new List<int>();

        currentPiece = pieceIndex;
        whiteKingIndex = 0;
        blackKingIndex = 0;

        isPlayer = char.IsUpper(board[pieceIndex]);

        FindKingIndexes(board);

        switch (char.ToUpper(board[pieceIndex]))
        {
            case 'P':
                if (char.IsLower(board[pieceIndex]))
                {

                    //one space forward
                    if (pieceIndex - 8 > 0 && board[pieceIndex - 8] == '\0')
                    {
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 8);
                    }
                    //two spaces if first turn
                    if (pieceIndex - 16 > 0 && BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[pieceIndex - 16] == '\0')
                    {
                        if (board[pieceIndex - 8] == '\0')
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 16);
                        }
                    }
                    //attack right 
                    if (pieceIndex - 7 < board.Length && (pieceIndex + 1) % 8 != 0)
                    {
                        // SPACE IS EMPTY                  En-Passant
                        if (board[pieceIndex - 7] != '\0' || BoardManager.Instance.enPassentIndex == pieceIndex - 7)
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 7);
                        }
                    }
                    //attack left
                    if (pieceIndex - 9 < board.Length && pieceIndex % 8 != 0)
                    {
                        // SPACE IS EMPTY                  En-Passant
                        if (board[pieceIndex - 9] != '\0' || BoardManager.Instance.enPassentIndex == pieceIndex - 9)
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 9);
                        }
                    }
                }
                else
                {
                    //one space forward
                    if (pieceIndex + 8 < board.Length && board[pieceIndex + 8] == '\0')
                    {
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 8);
                    }
                    //two spaces if first turn
                    if (pieceIndex + 16 < board.Length && BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex] && board[pieceIndex + 16] == '\0')
                    {
                        if (board[pieceIndex + 8] == '\0')
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 16);
                        }
                    }
                    //attack left
                    if (pieceIndex + 7 < board.Length && pieceIndex % 8 != 0)
                    {
                        if (board[pieceIndex + 7] != '\0' || BoardManager.Instance.enPassentIndex == pieceIndex + 7)
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 7);
                        }
                    }
                    //attack right
                    if (pieceIndex + 9 < board.Length && (pieceIndex + 1) % 8 != 0)
                    {
                        if (board[pieceIndex + 9] != '\0' || BoardManager.Instance.enPassentIndex == pieceIndex + 9)
                        {
                            MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 9);
                        }
                    }
                }
                canPlace = true;
                break;
            case 'R':
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //forward column
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                    if (canPlace == true && i != 0 && (pieceIndex + i) < board.Length)
                    {
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 6);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 6);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 10);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 10);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 15);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 15);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 15);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 17);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 17);
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
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
                    if (canPlace == false)
                    {
                        break;
                    }
                }
                canPlace = true;
                for (int i = 8; i < board.Length; i = i + 8)
                {
                    //backward column
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + i);
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
                        MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - i);
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
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 1);
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 7);
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 9);
                }
                //right movement
                if ((pieceIndex + 1) % 8 != 0)
                {
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 1);
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 7);
                    MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 9);
                }
                //one space up
                MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex + 8);
                //one space down
                MarkMoveCheck(validMoves, board, pieceIndex, pieceIndex - 8);
                if (BoardManager.Instance.InitialBoard[pieceIndex] == board[pieceIndex])
                {
                    if (pieceIndex == 4 && board[pieceIndex + 1] == '\0' && board[pieceIndex + 2] == '\0' && board[pieceIndex + 3] == char.ToUpper('R'))
                    {
                        MarkMove(validMoves, board, pieceIndex + 2);
                    }
                    if (pieceIndex == 4 && board[pieceIndex - 1] == '\0' && board[pieceIndex - 2] == '\0' && board[pieceIndex - 3] == '\0' && board[pieceIndex - 4] == char.ToUpper('R'))
                    {
                        MarkMove(validMoves, board, pieceIndex - 2);
                    }
                }
                canPlace = true;
                break;
        }
        return validMoves;
    }

    /// <summary>
    /// Adds a move to the valid move list
    /// </summary>
    /// <param name="validMoves"> the list of valid moves </param>
    /// <param name="board"> the board to find the moves on </param>
    /// <param name="index"> the index of the piece to add </param>
    /// <returns> the list of moves with the move added </returns>
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

    /// <summary>
    /// Marks a move after checking to see if the resulting board is in check
    /// </summary>
    /// <param name="validMoves"> the list of valid moves the player can make </param>
    /// <param name="board"> the board to find the moves on </param>
    /// <param name="fromPosition"> the position the piece is moving from </param>
    /// <param name="toPosition"> the position the piece is moving to </param>
    private static void MarkMoveCheck(List<int> validMoves, char[] board, int fromPosition, int toPosition)
    {
        if (toPosition < board.Length && toPosition > -1)
        {
            char tempHolder;
            tempHolder = board[toPosition];
            board[toPosition] = board[fromPosition];
            board[fromPosition] = '\0';
            FindKingIndexes(board);
            if (isPlayer == true)
            {
                if (InCheck(board, whiteKingIndex) == false)
                {
                    board[fromPosition] = tempHolder;
                    tempHolder = board[fromPosition];
                    board[fromPosition] = board[toPosition];
                    board[toPosition] = tempHolder;
                    MarkMove(validMoves, board, toPosition);
                }
                else
                {
                    board[fromPosition] = tempHolder;
                    tempHolder = board[fromPosition];
                    board[fromPosition] = board[toPosition];
                    board[toPosition] = tempHolder;
                    if (board[toPosition] != '\0')
                    {
                        canPlace = false;
                    }
                }
            }
            else
            {
                if (InCheck(board, blackKingIndex) == false)
                {
                    board[fromPosition] = tempHolder;
                    tempHolder = board[fromPosition];
                    board[fromPosition] = board[toPosition];
                    board[toPosition] = tempHolder;
                    MarkMove(validMoves, board, toPosition);
                }
                else

                {
                    board[fromPosition] = tempHolder;
                    tempHolder = board[fromPosition];
                    board[fromPosition] = board[toPosition];
                    board[toPosition] = tempHolder;
                    if (board[toPosition] != '\0')
                    {
                        canPlace = false;
                    }
                }
            }
            FindKingIndexes(board);
        }
    }

    /// <summary>
    /// Removes a move from the list of valid moves
    /// </summary>
    /// <param name="validMoves"> the list of valid moves the player can make </param>
    /// <param name="board"> the board to find the moves on </param>
    /// <param name="index"> the index of the move to be removed </param>
    /// <returns> the list without the move to be removed </returns>
    private static List<int> UnmarkMove(List<int> validMoves, char[] board, int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.RemoveAll(item => item == index);
        }
        return validMoves;
    }

    /// <summary>
    /// Determines if the board is in check
    /// </summary>
    /// <param name="board"> the board to find check on </param>
    /// <param name="kingIndex"> the location of the king to determine check on </param>
    /// <returns> if the board is in check or not </returns>
    private static bool InCheck(char[] board, int kingIndex)
    {
        //check for attacking pawns
        if (isPlayer == false)
        {
            if ((kingIndex + 1) % 8 != 0 && (kingIndex - 7) >= 0 && board[kingIndex - 7] == 'P')
            {
                return true;
            }
            if ((kingIndex % 8) != 0 && (kingIndex - 9) >= 0 && board[kingIndex - 9] == 'P')
            {
                return true;
            }
        }
        else
        {
            if ((kingIndex % 8) != 0 && (kingIndex + 7) < board.Length && board[kingIndex + 7] == 'p')
            {
                return true;
            }
            if ((kingIndex + 1) % 8 != 0 && (kingIndex + 9) < board.Length && board[kingIndex + 9] == 'p')
            {
                return true;
            }
        }

        //check for attacking rows & columns

        //forward column
        for (int i = 8; i < board.Length; i = i + 8)
        {
            if ((kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex + i]) == 'R' || char.ToUpper(board[kingIndex + i]) == 'Q') && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //backward column
        for (int i = 8; i < board.Length; i = i + 8)
        {
            if ((kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex - i]) == 'R' || char.ToUpper(board[kingIndex - i]) == 'Q') && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        // right row
        for (int i = 1; i < board.Length; i++)
        {
            if ((kingIndex + 1) % 8 != 0 && board[kingIndex + i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex + i]) == 'R' || char.ToUpper(board[kingIndex + i]) == 'Q') && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex + i + 1) % 8 == 0)
            {
                break;
            }
        }
        //left row
        for (int i = 1; i < board.Length; i++)
        {
            if ((kingIndex % 8) != 0 && board[kingIndex - i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex - i]) == 'R' || char.ToUpper(board[kingIndex - i]) == 'Q') && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex - i) % 8 == 0)
            {
                break;
            }
        }

        //check for attacking diagonals

        //up-left diagonal
        for (int i = 0; i < board.Length; i = i + 7)
        {
            if (i != 0 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex + i]) == 'B' || char.ToUpper(board[kingIndex + i]) == 'Q') && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                    //Debug.Log("In Check from " + (kingIndex + i));
                }
                break;
            }
            if ((kingIndex + i) % 8 == 0)
            {
                break;
            }
        }
        //up-right diagonal
        for (int i = 0; i < board.Length; i = i + 9)
        {
            if (i != 0 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex + i]) == 'B' || char.ToUpper(board[kingIndex + i]) == 'Q') && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex + i) % 8 == 0)
            {
                break;
            }
        }
        //down-right diagonal
        for (int i = 0; i < board.Length; i = i + 7)
        {
            if (i != 0 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex - i]) == 'B' || char.ToUpper(board[kingIndex - i]) == 'Q') && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex - i + 1) % 8 == 0)
            {
                break;
            }
        }
        //down-left diagonal
        for (int i = 0; i < board.Length; i = i + 9)
        {
            if (i != 0 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if (((char.ToUpper(board[kingIndex - i]) == 'B' || char.ToUpper(board[kingIndex - i]) == 'Q') && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex - i) % 8 == 0)
            {
                break;
            }
        }
        int rowsThrough = 0;
        //two up one left
        for (int i = 0; i < 16; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if ((kingIndex + i) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 14 && kingIndex % 8 == 0)
            {
                rowsThrough = 1;
            }
            if (i == 15 && rowsThrough == 2 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex + i]) == 'N' && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //two up one right
        for (int i = 0; i < 18; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if ((kingIndex + i) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 17 && (kingIndex + 1) % 8 == 0)
            {
                rowsThrough = rowsThrough + 6;
            }
            else if (i == 17 && kingIndex % 8 == 0)
            {
                rowsThrough = 2;
            }
            if (i == 17 && rowsThrough == 2 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex + i]) == 'N' && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //one up two left
        for (int i = 0; i < 7; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if ((kingIndex + i + 1) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 6 && rowsThrough == 1 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex + i]) == 'N' && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //one up two right
        for (int i = 0; i < 11; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if ((kingIndex + i + 1) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 10 && rowsThrough == 1 && (kingIndex + i) < board.Length && board[kingIndex + i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex + i]) == 'N' && IsOtherTeam(board, kingIndex + i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //two down one right
        for (int i = 0; i < 16; i++)
        {
            if ((kingIndex - i) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if (i == 0 && (kingIndex + 1) % 8 == 0)
            {
                rowsThrough--;
            }
            if (i == 15 && rowsThrough == 2 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex - i]) == 'N' && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //two down one left
        for (int i = 0; i < 18; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if ((kingIndex - i) % 8 == 0)
            {
                rowsThrough++;
            }
            if (i == 17 && (kingIndex - i) % 8 == 0)
            {
                rowsThrough--;
            }
            if (i == 17 && rowsThrough == 2 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex - i]) == 'N' && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
        }
        //one down two right
        for (int i = 0; i < 7; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }

            if (i == 6 && rowsThrough == 1 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if (kingIndex % 8 == 0)
                {
                    rowsThrough++;
                }
                if ((char.ToUpper(board[kingIndex - i]) == 'N' && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex - i) % 8 == 0)
            {
                rowsThrough++;
            }
        }
        //one down two left
        for (int i = 0; i < 11; i++)
        {
            if (i == 0)
            {
                rowsThrough = 0;
            }
            if (i == 9 && (kingIndex + 1) % 8 == 0)
            {
                rowsThrough = 1;
            }
            if (i == 10 && rowsThrough == 1 && (kingIndex - i) > -1 && board[kingIndex - i] != '\0')
            {
                if ((char.ToUpper(board[kingIndex - i]) == 'N' && IsOtherTeam(board, kingIndex - i, kingIndex)))
                {
                    return true;
                }
                break;
            }
            if ((kingIndex - i) % 8 == 0)
            {
                rowsThrough++;
            }
        }
        return false;
    }

    /// <summary>
    /// Finds the location of the two kings
    /// </summary>
    /// <param name="board"> the board to find the kings on</param>
    private static void FindKingIndexes(char[] board)
    {
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 'K')
            {
                whiteKingIndex = i;
            }
            else if (board[i] == 'k')
            {
                blackKingIndex = i;
            }
        }
    }

    /// <summary>
    /// Determines if two pieces are on the same team
    /// </summary>
    /// <param name="board"> the board that the pieces are on </param>
    /// <param name="piece1"> the first piece to compare </param>
    /// <param name="piece2"> the second piece to compare </param>
    /// <returns> if the two pieces are on the same team or not </returns>
    private static bool IsOtherTeam(char[] board, int piece1, int piece2)
    {
        bool result;

        if (char.IsUpper(board[piece1]) != char.IsUpper(board[piece2]))
        {
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }
}