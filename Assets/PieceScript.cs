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
    public Vector3 homePosition;


    public PieceType pieceType;

    private void Start()
    {
        homePosition = transform.position;
    }
}
