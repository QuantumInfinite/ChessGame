﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableScript : MonoBehaviour {
    
    List<SquareScript> validMoves = new List<SquareScript>();

    PieceScript heldPiece;

    SquareScript[] board;

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

        int indexOfThisMove = GameManager.PositionToBoardIndex(newPos);
        if (indexOfThisMove >= 0 && indexOfThisMove < board.Length && validMoves.Contains(board[indexOfThisMove])) //Move is valid
        {
            heldPiece.MoveToSquare(board[indexOfThisMove]);
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
                validMoves = MoveValidator.MarkValidMoves(heldPiece);
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
