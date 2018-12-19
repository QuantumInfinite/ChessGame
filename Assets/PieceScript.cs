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
        }
        hasMoved = true;
    }



    public bool HasMoved() {
        return hasMoved;
    }

    void SetMaterial(PieceType pieceType, Team team)
    {
        rend.material = GameManager.Instance.pieceMaterials.GetMaterial(pieceType, team);
    }

    private void Start()
    {
        lastValidPosition = transform.position;
        rend = GetComponent<Renderer>();

        SetMaterial(pieceType, team);


    }
}
