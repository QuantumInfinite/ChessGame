﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour {
    Material baseMaterial;
    Renderer rend;
    
    [ReadOnly] public Vector2 position;
    
    public bool spawnPieceAtStart;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.PieceType startingPieceType;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Team startingPieceTeam;

    [SerializeField][ReadOnly]
    PieceScript currentPiece;
    
    public PieceScript CurrentPiece {
        get {
            return currentPiece;
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
        if (spawnPieceAtStart)
        {
            GameObject startingPiece = GameObject.Instantiate(GameManager.Instance.basePiecePrefab, new Vector3(position.x, position.y, -1), Quaternion.Euler(0, 0, 180));
            currentPiece = startingPiece.GetComponent<PieceScript>();
            currentPiece.SetSquare(this);
            currentPiece.pieceType = startingPieceType;
            currentPiece.team = startingPieceTeam;
            currentPiece.SetMaterial(startingPieceType, startingPieceTeam);
        }
    }

    public void SetPiece(PieceScript piece)
    {
        if (piece == null)
        {
            currentPiece = null;
            return;
        }
        if (currentPiece != null)
        {
            currentPiece.SetSquare(null);
        }
        currentPiece = piece;

        if (currentPiece.CurrentSquare != this)
        {
            currentPiece.SetSquare(this);
        }

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
