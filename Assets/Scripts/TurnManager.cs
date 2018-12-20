using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {
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
    
    int turnCounter;
    bool isPlayerTurn;

    public bool IsPlayerTurn {
        get {
            return isPlayerTurn;
        }
    }

    public void EndTurn()
    {
        turnCounter++;
        isPlayerTurn = !isPlayerTurn;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
