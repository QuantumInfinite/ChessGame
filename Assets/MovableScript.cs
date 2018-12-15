using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableScript : MonoBehaviour {
    GameObject heldPiece;
    Vector2 heldPiecePrevPos;
    public GameObject[] board;
	void Update () {

		if (!heldPiece && Input.GetAxis("Fire1") != 0)
        {
            TryPickup();
        }
        else if (heldPiece)
        {
            MoveToCursor();
        }
	}

    void TryPickup()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "chessPiece")
        {
            heldPiece = hit.transform.gameObject;
            heldPiecePrevPos = heldPiece.transform.position;
            HighlightValidMoves();
        }
    }

    void MoveToCursor()
    {
        Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        heldPiece.transform.position = new Vector3(newPos.x, newPos.y, heldPiece.transform.position.z);

        if (Input.GetAxis("Fire1") == 0)
        {
            heldPiece.transform.position = new Vector3(
                Mathf.RoundToInt(heldPiece.transform.position.x),
                Mathf.RoundToInt(heldPiece.transform.position.y),
                heldPiece.transform.position.z
            );
            heldPiece = null;
        }
    }

    void HighlightValidMoves()
    {
        PieceScript pieceScript = heldPiece.GetComponent<PieceScript>();
        switch (pieceScript.pieceType)
        {
            case PieceScript.PieceType.Pawn:
                int initPos = (int) ((heldPiecePrevPos.y - 1) * 8 + (heldPiecePrevPos.x - 1));
                HighlightPos(initPos + 8);
                if ()
                {

                }
                break;
            case PieceScript.PieceType.Rook:
                break;
            case PieceScript.PieceType.Bishop:
                break;
            case PieceScript.PieceType.Knight:
                break;
            case PieceScript.PieceType.Queen:
                break;
            case PieceScript.PieceType.King:
                break;
        }
    }
    void HighlightPos(int index)
    {
        if (index >= 0 && index < board.Length)
        {
            //highlight
        }
    }
}
