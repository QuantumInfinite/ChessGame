using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerScript : MonoBehaviour {
    
    List<SquareScript> validMoves = new List<SquareScript>();

    PieceScript heldPiece;

    SquareScript[] board;

    private void Start()
    {
        board = BoardManager.Instance.board;
    }

    void Update ()
    {
        if (TurnManager.Instance.IsPlayerTurn() || GameManager.Instance.playerOnlyTurns)
        {
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
            //Remove this after debugging
            if (!heldPiece && Input.GetAxis("Fire2") != 0)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit) && hit.transform.tag == "chessPiece")
                {
                    ClearValidMoves();
                    List<int> moves = MoveValidator_alt.FindValidMoves(BoardManager.PositionToBoardIndex(hit.transform.position), BoardManager.BoardToCharArray(board));
                    for (int i = 0; i < moves.Count; i++)
                    {
                        validMoves.Add(board[moves[i]]);
                    }
                    ApplyHighlight();
                }
            }
            //END REMOVE
        }
	}

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
            BoardManager.Instance.MakeMove(heldPiece.index, indexOfThisMove);
        }
        else //Not valid, return to last position
        {
            heldPiece.ResetToLast();
        }

        heldPiece = null;
        ClearValidMoves();
    }

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
                List<int> moves = MoveValidator_alt.FindValidMoves(BoardManager.PositionToBoardIndex(hit.transform.position), BoardManager.BoardToCharArray(board));
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

    bool MoveIsValid(Vector2 newPos)
    {
        return validMoves.Find(item => item.position == newPos) != null;
    }

    void MoveToCursor()
    {
        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        heldPiece.transform.position = new Vector3(newPos.x, newPos.y, heldPiece.transform.position.z);
    }
    
    void MarkMove(int index)
    {
        if (index >= 0 && index < board.Length && !validMoves.Contains(board[index]))
        {
            validMoves.Add(board[index]);
        }
    }

    void UnmarkMove(int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.RemoveAll(x => x.position == board[index].position);
        }
    }

    void ApplyHighlight()
    {
        foreach (SquareScript space in validMoves)
        {
            space.SetMaterial(GameManager.Instance.boardMaterials.highlight);
        }        
    }

    void ClearValidMoves()
    {
        foreach (SquareScript index in validMoves)
        {
            index.ResetMaterial();
        }
        validMoves.Clear();
    }
}
