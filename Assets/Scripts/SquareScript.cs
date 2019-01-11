using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareScript : MonoBehaviour
{
    Material baseMaterial;
    Renderer rend;

    [ReadOnly] public Vector2 position;

    public bool spawnPieceAtStart;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Type startingPieceType;
    [ConditionalHide("spawnPieceAtStart", true)]
    public PieceScript.Team startingPieceTeam;

    [SerializeField]
    [ReadOnly]
    PieceScript linkedPiece;

    /// <summary>
    /// Gets the piece linked to the script
    /// </summary>
    public PieceScript LinkedPiece {
        get {
            return linkedPiece;
        }
    }

    /// <summary>
    /// Runs once before Start
    /// </summary>
    void Awake()
    {
        rend = GetComponent<Renderer>();
        baseMaterial = rend.material;
        position = transform.position;
    }

    /// <summary>
    /// Runs once when the game begins
    /// </summary>
    private void Start()
    {
        if (!GameManager.Instance.useStringForBoardInput && spawnPieceAtStart)
        {
            SpawnPiece(startingPieceType, startingPieceTeam);
        }
    }

    /// <summary>
    /// Links a square to a piece
    /// </summary>
    /// <param name="piece"> the piece to link to the square </param>
    public void SetPiece(PieceScript piece)
    {
        linkedPiece = piece;
    }

    /// <summary>
    /// Spawn a piece and put it on the square
    /// </summary>
    /// <param name="type"> which type of piece should be spawned </param>
    /// <param name="team"> which team the spawned piece should be on </param>
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

    /// <summary>
    /// Sets the material of the square
    /// </summary>
    /// <param name="mat"> the material to set to the square </param>
    public void SetMaterial(Material mat)
    {
        rend.material = mat;
    }

    /// <summary>
    /// Resets the material of the board to its default
    /// </summary>
    public void ResetMaterial()
    {
        rend.material = baseMaterial;
    }

}
