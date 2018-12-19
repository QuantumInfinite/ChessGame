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
    BoardSpace currentSquare;

    public BoardSpace CurrentSquare {
        get {
            return currentSquare;
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

    public void MoveToPosition(Vector2 newPos)
    {
        transform.position = new Vector3(
            newPos.x,
            newPos.y,
            transform.position.z
        );
        if (lastValidPosition != newPos)
        {
            lastValidPosition = newPos;
            hasMoved = true;
        }
    }
    public void MoveToSquare(BoardSpace square)
    {
        transform.position = new Vector3(
            square.position.x,
            square.position.y,
            transform.position.z
        );
        if (lastValidPosition != square.position)
        {
            lastValidPosition = square.position;
            hasMoved = true;
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
            currentSquare = null;
            return;
        }
        if (currentSquare != null)
        {
            currentSquare.SetPiece(null);
        }
        currentSquare = square;

        if (currentSquare.CurrentPiece != this)
        {
            currentSquare.SetPiece(this);
        }
    }

    private void Awake()
    {
        lastValidPosition = transform.position;
        rend = GetComponent<Renderer>();
    }
}
