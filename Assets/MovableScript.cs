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
                for (int i = 1; i < board.Length; i++)
                    UnmarkMove(initPos);
                break;
            case PieceScript.PieceType.Bishop:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                for (int i = 0; i < board.Length; i = i + 7) 
                {
                    if ((initPos + i+1) % 8 == 0) break;
                    MarkMove(initPos + i);
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i+1) % 8 == 0) break; 
                }
                for (int i = 0; i < board.Length; i = i + 9) 
                {
                    MarkMove(initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                    UnmarkMove(initPos);
                break;
            case PieceScript.PieceType.Knight:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));
                int rowsThrough = 0;

                for (int i=0; i<6; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos-i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        if (initPos % 8 == 0)
                            rowsThrough++;
                        MarkMove(initPos - 6);
                    }
                }
                for (int i = 0; i < 6; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos + i+1) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 5 && rowsThrough == 1)
                    {
                        MarkMove(initPos + 6);
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    if (i == 0)
                        rowsThrough = 0;
                    if ((initPos +i) % 8 == 0)
                    {
                        rowsThrough++;
                    }
                    if (i == 9 && (initPos + 2) % 8 == 0)
                        rowsThrough = rowsThrough + 6;
                    else if (i == 9 && initPos % 8 == 0)
                        rowsThrough = 1;
                   if (i == 9 && rowsThrough == 1)
                    {
                        MarkMove(initPos + 10);
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
                        MarkMove(initPos - 10);
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
                        MarkMove(initPos + 15);
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
                        MarkMove(initPos - 15);
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
                        MarkMove(initPos + 17);
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
                        MarkMove(initPos - 17);
                    }
                }
                break;
            case PieceScript.PieceType.Queen:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                for (int i = 0; i < board.Length; i = i + 8)
                {
                    MarkMove(initPos + i);
                    MarkMove(initPos - i);
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    if ((initPos + i + 1) % 8 == 0) break;
                    MarkMove(initPos + i);
                }
                for (int i = 0; i < board.Length; i = i + 7)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(initPos + i);
                    if ((initPos + i + 1) % 8 == 0) break;
                }
                for (int i = 0; i < board.Length; i = i + 9)
                {
                    MarkMove(initPos - i);
                    if ((initPos - i) % 8 == 0) break;
                }
                for (int i = 1; i < board.Length; i++)
                    UnmarkMove(initPos);
                break;
            case PieceScript.PieceType.King:
                initPos = (int)((heldPiece.LastValidPosition.y - 1) * 8 + (heldPiece.LastValidPosition.x - 1));

                MarkMove(initPos + 1);
                MarkMove(initPos - 1);
                MarkMove(initPos + 7);
                MarkMove(initPos - 7);
                MarkMove(initPos + 8);
                MarkMove(initPos - 8);
                MarkMove(initPos + 9);
                MarkMove(initPos - 9);
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

    void UnmarkMove(int index)
    {
        if (index >= 0 && index < board.Length)
        {
            validMoves.Remove(board[index]);
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
