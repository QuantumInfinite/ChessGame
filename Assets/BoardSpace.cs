using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour {
    Material baseMaterial;
    Renderer Renderer;
    
    [ReadOnly] public Vector2 position;
    
    public bool spawnPieceAtStart;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.PieceType startingPieceType;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Team startingPieceTeam;

    PieceScript currentPiece;

    public PieceScript CurrentPiece {
        get {
            return currentPiece;
        }
    }

    public void SetPiece(PieceScript piece)
    {
        currentPiece = piece;
    }
    
    void Start () {
        Renderer = GetComponent<Renderer>();
        baseMaterial = Renderer.material;
        position = transform.position;
	}
	
	public void SetMaterial(Material mat)
    {
        Renderer.material = mat;
    }

    public void ResetMaterial()
    {
        Renderer.material = baseMaterial;
    }
    
}
