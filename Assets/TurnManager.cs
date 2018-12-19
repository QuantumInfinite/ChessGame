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
    bool playerTurn;

    public bool PlayerTurn {
        get {
            return playerTurn;
        }
    }

    public void EndTurn()
    {
        playerTurn = !playerTurn;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
