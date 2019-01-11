using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour
{

    List<SquareScript> validMoves = new List<SquareScript>();

    PieceScript heldPiece;

    SquareScript[] board;
    bool checkedIfCheckmateThisTurn;
    private void Start()
    {
        board = BoardManager.Instance.board;
    }

    /// <summary>
    /// Runs every frame
    /// </summary>
    void Update()
    {
        if (!GameManager.Instance.ReadyToPlay)
        {
            return;
        }
        if (TurnManager.Instance.IsPlayerTurn() || GameManager.Instance.playerOnlyTurns)
        {
            if (!checkedIfCheckmateThisTurn )
            {
                int kingIndex = -1;
                for (int i  = 0; i  < BoardManager.Instance.boardChars.Length; i ++)
                {
                    if (BoardManager.Instance.boardChars[i] == 'K')
                    {
                        kingIndex = i;
                    }
                }
                List<Move> moves = AIBrainScript.GenerateNextMoves(BoardManager.Instance.boardChars, false);
                if (moves.Count == 0)
                {
                    GameManager.Instance.AIWin();
                }
                
                checkedIfCheckmateThisTurn = true;
            }
            //Methods dependent on input
            if (!heldPiece && Input.GetAxis("Fire1") != 0)
            {
                TryPickup();
            }
            else if (heldPiece)
            {
                MoveToCursor();

                if (Input.GetAxis("Fire1") == 0)//Not Holding
                {
                    DropPiece();
                }
            }
        }
        if (checkedIfCheckmateThisTurn)
        {
            checkedIfCheckmateThisTurn = false;
        }
    }

    /// <summary>
    /// Lets go of the piece that the player is holding on to
    /// </summary>
    void DropPiece()
    {
        Vector3 newPos = new Vector3(
            Mathf.RoundToInt(heldPiece.transform.position.x),
            Mathf.RoundToInt(heldPiece.transform.position.y),
            heldPiece.transform.position.z
        );

        //Actually move

        int indexOfThisMove = BoardManager.PositionToBoardIndex(newPos);
        if (indexOfThisMove >= 0 && indexOfThisMove < board.Length && validMoves.Contains(board[indexOfThisMove])) //Move is valid
        {
            BoardManager.Instance.MakeMove(new Move(BoardManager.Instance.boardChars, heldPiece.index, indexOfThisMove));
        }
        else //Not valid, return to last position
        {
            heldPiece.ResetToLast();
        }

        heldPiece = null;
        ClearValidMoves();
    }

    /// <summary>
    /// Picks up a piece on the board
    /// </summary>
    void TryPickup()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "chessPiece")
        {
            heldPiece = hit.transform.GetComponent<PieceScript>();

            if (heldPiece.team == GameManager.Instance.playerTeam)
            {
                ClearValidMoves();
                List<int> moves = MoveValidator.FindValidMoves(BoardManager.PositionToBoardIndex(hit.transform.position), BoardManager.BoardToCharArray(board));
                for (int i = 0; i < moves.Count; i++)
                {
                    validMoves.Add(board[moves[i]]);
                }
                ApplyHighlight();
            }
            else
            {
                heldPiece = null;
            }
        }
    }

    /// <summary>
    /// Checks if a certain move is valid
    /// </summary>
    /// <param name="newPos"> the position of the move to check </param>
    /// <returns> if the move is valid or not </returns>
    bool MoveIsValid(Vector2 newPos)
    {
        return validMoves.Find(item => item.position == newPos) != null;
    }

    /// <summary>
    /// Moves the piece the player is holding along with the cursor.
    /// </summary>
    void MoveToCursor()
    {
        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heldPiece.transform.position = new Vector3(newPos.x, newPos.y, heldPiece.transform.position.z);
    }

    /// <summary>
    /// Marks a move as valid
    /// </summary>
    /// <param name="index"> the index of the move to mark </param>
    void MarkMove(int index)
    {
        if (index >= 0 && index < board.Length && !validMoves.Contains(board[index]))
        {
            validMoves.Add(board[index]);
        }
    }

    /// <summary>
    /// Unmarks a move as valid
    /// </summary>
    /// <param name="index"> the index of the move to unmark </param>
    void UnmarkMove(int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.RemoveAll(x => x.position == board[index].position);
        }
    }

    /// <summary>
    /// Highlights the square of a valid move
    /// </summary>
    void ApplyHighlight()
    {
        foreach (SquareScript space in validMoves)
        {
            space.SetMaterial(GameManager.Instance.boardMaterials.highlight);
        }
    }

    /// <summary>
    /// Removes all valid moves
    /// </summary>
    void ClearValidMoves()
    {
        foreach (SquareScript index in validMoves)
        {
            index.ResetMaterial();
        }
        validMoves.Clear();
    }
}
