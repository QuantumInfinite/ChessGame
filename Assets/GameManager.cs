using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;
    public static GameManager Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    [System.Serializable]
    public struct BoardMaterials
    {
        public Material white;
        public Material black;
        public Material highlight;
    }
    public BoardMaterials boardMaterials;

    public BoardSpace[] board;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


}
