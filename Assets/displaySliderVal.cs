using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class displaySliderVal : MonoBehaviour {
    public Slider target;
    public Text display;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        display.text = "" + Mathf.RoundToInt(target.value * 10);
	}
}
