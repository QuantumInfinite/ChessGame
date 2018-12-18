using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableScript : MonoBehaviour {
    [System.Serializable]
    public struct BoardMaterials
    {
        public Material white;
        public Material black;
        public Material highlight;
    }
    public BoardMaterials boardMaterials;

    List<BoardSpace> validMoves = new List<BoardSpace>();

    PieceScript heldPiece;

    public BoardSpace[] board;

	void Update () {
        print(heldPiece == null);
		if (!heldPiece && Input.GetAxis("Fire1") != 0)
        {
            TryPickup();
        }
        else if (heldPiece)
        {

            MoveToCursor();

            if (Input.GetAxis("Fire1") == 0)//Not Holding
            {
                Vector3 newPos = new Vector3(
                    Mathf.RoundToInt(heldPiece.transform.position.x),
                    Mathf.RoundToInt(heldPiece.transform.position.y),
                    heldPiece.transform.position.z
                );

                if (MoveIsValid(newPos))
                {
                    heldPiece.MoveToPosition(newPos);
                }
                else
                {
                    heldPiece.MoveToPosition(heldPiece.LastValidPosition);
                }

                heldPiece = null;
                ClearValidMoves();
            }
        }
	}

    void TryPickup()
    {
        print("attempting PIckup");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "chessPiece")
        {
            print("Pickup successful");
            heldPiece = hit.transform.GetComponent<PieceScript>();
            MarkValidMoves();
        }
    }

    bool MoveIsValid(Vector2 newPos)
    {
        foreach (BoardSpace item in validMoves)
        {
            if (item.position == newPos)
            {
                return true;
            }
        }
        return false;
    }
    void MoveToCursor()
    {
        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        heldPiece.transform.position = new Vector3(newPos.x, newPos.y, heldPiece.transform.position.z);
    }

    void MarkValidMoves()
    {
        switch (heldPiece.pieceType)
        {
            case PieceScript.PieceType.Pawn:
                int initPos = (int) ((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                MarkMove(initPos + 8);

                if (!heldPiece.HasMoved())
                {
                    MarkMove(initPos + 16);
                }
                break;
            case PieceScript.PieceType.Rook:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                for (int i = 0; i < board.Length; i = i + 8)
                {
                    MarkMove(initPos + i);
                    MarkMove(initPos - i);
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(initPos + i);
                    if ((initPos + i+1) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i ) % 8 == 0) break;
                }
                break;
            case PieceScript.PieceType.Bishop:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                MarkMove(1);
                break;
            case PieceScript.PieceType.Knight:
                break;
            case PieceScript.PieceType.Queen:
                break;
            case PieceScript.PieceType.King:
                break;
        }
        ApplyHighlight();
    }
    
    void MarkMove(int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.Add(board[index]);
        }
    }

    void ApplyHighlight()
    {
        foreach (BoardSpace index in validMoves)
        {
            index.SetMaterial(boardMaterials.highlight);
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
