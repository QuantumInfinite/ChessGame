using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MovableScript : MonoBehaviour {
    
	void Start () {
		
	}
    GameObject heldObject;

	void Update () {

		if (!heldObject && Input.GetAxis("Fire1") != 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "chessPiece")
            {
                heldObject = hit.transform.gameObject;
                
            }
        }
        if (heldObject)
        {
            Vector2 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            heldObject.transform.position = new Vector3(newPos.x, newPos.y, heldObject.transform.position.z);

            if (Input.GetAxis("Fire1") == 0)
            {
                heldObject.transform.position = new Vector3(
                    Mathf.RoundToInt(heldObject.transform.position.x),
                    Mathf.RoundToInt(heldObject.transform.position.y),
                    heldObject.transform.position.z
                );
                heldObject = null;
            }

        }
	}
}
