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

    private bool hasMoved = false;

    Vector2 lastValidPosition;
    public Vector2 LastValidPosition {
        get {
            return lastValidPosition;
        }

        set {
            lastValidPosition = value;
            hasMoved = true;
        }
    }

    public bool HasMoved() {
        return hasMoved;
    }

    private void Start()
    {
        lastValidPosition = transform.position;
    }
}
