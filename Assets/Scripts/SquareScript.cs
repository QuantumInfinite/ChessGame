﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour {
    Material baseMaterial;
    Renderer rend;
    
    [ReadOnly] public Vector2 position;
    
    public bool spawnPieceAtStart;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Type startingPieceType;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Team startingPieceTeam;

    [SerializeField][ReadOnly]
    PieceScript linkedPiece;
    
    public PieceScript LinkedPiece {
        get {
            return linkedPiece;
        }
    }

    void Awake()
    {
        rend = GetComponent<Renderer>();
        baseMaterial = rend.material;
        position = transform.position;
    }

    private void Start()
    {
        if (!GameManager.Instance.useStringForBoardInput && spawnPieceAtStart)
        {
            SpawnPiece(startingPieceType, startingPieceTeam);
        }
    }

    public void SetPiece(PieceScript piece)
    {
        linkedPiece = piece;
    }
    
    public void SpawnPiece(PieceScript.Type type, PieceScript.Team team)
    {
        GameObject startingPiece = GameObject.Instantiate(GameManager.Instance.basePiecePrefab, new Vector3(position.x, position.y, -1), Quaternion.Euler(0, 0, 180));
        linkedPiece = startingPiece.GetComponent<PieceScript>();
        linkedPiece.SetSquare(this);
        linkedPiece.name = (team + " " + type);
        linkedPiece.type = type;
        linkedPiece.team = team;
        linkedPiece.SetMaterial(type, team);
        BoardManager.Instance.RegisterPiece(linkedPiece);
    }
	
	public void SetMaterial(Material mat)
    {
        rend.material = mat;
    }

    public void ResetMaterial()
    {
        rend.material = baseMaterial;
    }
    
}
