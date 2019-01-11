using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceScript : MonoBehaviour
{

    public enum Type
    {
        Pawn,
        Rook,
        Bishop,
        Knight,
        Queen,
        King,
        BLOCK
    }
    public Type type;

    public enum Team
    {
        White,
        Black
    }
    public Team team;

    [SerializeField]
    [ReadOnly]
    SquareScript linkedSquare;

    public SquareScript LinkedSquare {
        get {
            return linkedSquare;
        }
    }

    private bool isVulnerable = false;
    public bool IsVulnerable {
        get {
            return isVulnerable;
        }
        set {
            isVulnerable = value;
        }
    }
    public Vector2 position {
        get {
            if (linkedSquare != null)
            {
                return linkedSquare.position;
            }
            return new Vector2();
        }
    }

    /// <summary>
    /// Gets the index of the piece in the board array 
    /// </summary>
    public int index {
        get {
            return BoardManager.PositionToBoardIndex(position);
        }
    }

    int moves = 0;

    Renderer rend;

    SquareScript startingSquare;

    /// <summary>
    /// The last square the piece was on
    /// </summary>
    SquareScript lastValidSquare;
    public SquareScript LastValidSquare {
        get {
            return lastValidSquare;
        }

        set {
            lastValidSquare = value;
        }
    }

    /// <summary>
    /// Moves the piece back to its last square
    /// </summary>
    public void ResetToLast()
    {
        MoveToSquare(lastValidSquare);
    }

    /// <summary>
    /// Moves the piece to a different square
    /// </summary>
    /// <param name="square"> the square to move the piece to </param>
    public void MoveToSquare(SquareScript square)
    {
        transform.position = new Vector3(
            square.position.x,
            square.position.y,
            transform.position.z
        );
        if (lastValidSquare != square) //Actually moved
        {
            lastValidSquare = square;
            moves++;

            if (square.LinkedPiece != null)//Had an occupent
            {
                BoardManager.Instance.RemovePiece(square.LinkedPiece);
                square.SetPiece(null);
            }
            SetSquare(square);
            square.SetPiece(this);
        }
    }

    /// <summary>
    /// Determines if the piece has moved or not
    /// </summary>
    /// <returns></returns>
    public bool HasMoved()
    {
        return moves != 0;
    }

    /// <summary>
    /// Sets a material to the piece
    /// </summary>
    /// <param name="pieceType"> the type of the piece the material is applied to </param>
    /// <param name="team"> the team of the piece the material is applied to </param>
    public void SetMaterial(Type pieceType, Team team)
    {
        rend.material = GameManager.Instance.pieceMaterials.GetMaterial(pieceType, team);
    }

    /// <summary>
    /// Sets a piece on a square
    /// </summary>
    /// <param name="square"> the square to set the piece on </param>
    public void SetSquare(SquareScript square)
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

    /// <summary>
    /// Checks if the piece can perform en passant or not
    /// </summary>
    /// <returns> if the piece can perform en passant </returns>
    public bool EnpassentCheck()
    {
        //Actually needs another check, this move can only occour immediatly after the double step forward. 
        return (type == Type.Pawn && moves == 1 && Vector2.Distance(startingSquare.position, transform.position) == 2);
    }

    private void Awake()
    {
        lastValidSquare = BoardManager.Instance.board[BoardManager.PositionToBoardIndex(transform.position)];
        startingSquare = lastValidSquare;
        rend = GetComponent<Renderer>();
    }
}
