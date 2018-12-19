using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableScript : MonoBehaviour {
    
    List<BoardSpace> validMoves = new List<BoardSpace>();

    PieceScript heldPiece;

    BoardSpace[] board;

    private void Start()
    {
        board = GameManager.Instance.board;
    }

    void Update () {
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

    void DropPiece()
    {
        Vector3 newPos = new Vector3(
            Mathf.RoundToInt(heldPiece.transform.position.x),
            Mathf.RoundToInt(heldPiece.transform.position.y),
            heldPiece.transform.position.z
        );

        //Actually move
        BoardSpace newSquare = GetBoardPiece(newPos);
        newSquare.SetPiece(heldPiece);
        //heldPiece.SetSquare(newSquare);
        heldPiece.MoveToSquare(newSquare);

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
            validMoves = MoveValidator.MarkValidMoves(heldPiece);
            ApplyHighlight();
        }
    }

    bool MoveIsValid(Vector2 newPos)
    {
        return validMoves.Find(item => item.position == newPos) != null;
    }

    BoardSpace GetBoardPiece(Vector2 position)
    {
        int indexOfThisMove = (int)((position.y - 1) * 8 + (position.x - 1));
        int indexOfLastMove = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));
        if (validMoves.Contains(board[indexOfThisMove])) //Move is valid
        {
            return board[indexOfThisMove];
        }
        else //Not valid, return to last position
        {
            return board[indexOfLastMove];
        }
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
        foreach (BoardSpace space in validMoves)
        {
            space.SetMaterial(GameManager.Instance.boardMaterials.highlight);
        }        
    }

    void ClearValidMoves()
    {
        foreach (BoardSpace index in validMoves)
        {
            index.ResetMaterial();
        }
        validMoves.Clear();
    }
}
