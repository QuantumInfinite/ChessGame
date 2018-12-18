using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSpace : MonoBehaviour {
    Material baseMaterial;
    Renderer Renderer;
    public Vector2 position;
	// Use this for initialization
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
