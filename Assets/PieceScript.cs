using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour {

    public enum PieceType
    {
        Pawn,
        Rook,
        Bishop,
        Knight,
        Queen,
        King
    }
    public PieceType pieceType;

    public enum Team
    {
        White, 
        Black
    }
    public Team team;

    [SerializeField][ReadOnly]
    BoardSpace linkedSquare;

    public BoardSpace LinkedSquare {
        get {
            return linkedSquare;
        }
    }

    private bool hasMoved = false;

    Renderer rend;

    Vector2 lastValidPosition;
    
    public Vector2 LastValidPosition {
        get {
            return lastValidPosition;
        }

        set {
            lastValidPosition = value;
        }
    }

    public void MoveToSquare(BoardSpace square)
    {
        transform.position = new Vector3(
            square.position.x,
            square.position.y,
            transform.position.z
        );
        if (lastValidPosition != square.position) //Actually moved
        {
            lastValidPosition = square.position;
            hasMoved = true;

            if (square.LinkedPiece != null)//Had an occupent
            {
                GameManager.Instance.RemovePiece(square.LinkedPiece);
            }
            SetSquare(square);
            square.SetPiece(this);
        }
    }

    public bool HasMoved() {
        return hasMoved;
    }

    public void SetMaterial(PieceType pieceType, Team team)
    {
        rend.material = GameManager.Instance.pieceMaterials.GetMaterial(pieceType, team);
    }

    public void SetSquare(BoardSpace square)
    {
        if (square == null)
        {
            linkedSquare = null;
            return;
        }
        if (linkedSquare != null && linkedSquare != square)
        {
            linkedSquare.SetPiece(null);
        }
        linkedSquare = square;
    }
    private void Awake()
    {
        lastValidPosition = transform.position;
        rend = GetComponent<Renderer>();
    }
}
