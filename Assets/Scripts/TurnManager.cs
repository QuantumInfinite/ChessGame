using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private static TurnManager instance;
    public static TurnManager Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<TurnManager>();
            }
            return instance;
        }
    }
    PieceScript.Team currentTurn;

    int turnCounter;

    public bool IsPlayerTurn()
    {
        return GameManager.Instance.playerTeam == currentTurn;
    }

    public string CurrentTurn()
    {
        if (IsPlayerTurn())
        {
            return "Player: ";
        }
        else
        {
            return "A.I.:     ";
        }

    }
    public void EndTurn()
    {
        turnCounter++;
        currentTurn = (currentTurn == PieceScript.Team.White) ? PieceScript.Team.Black : PieceScript.Team.White;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        currentTurn = PieceScript.Team.White;
    }
}
