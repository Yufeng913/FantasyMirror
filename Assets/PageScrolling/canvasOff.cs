using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasOff : MonoBehaviour {

	public GameObject canvasObject;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			DisableCanvas ();	
		}

		if (Input.GetKeyDown (KeyCode.S)) {
			EnableCanvas ();	
		}
	}

	void DisableCanvas(){
		canvasObject.GetComponent<Canvas> ().enabled = false;
	}

	void EnableCanvas(){
		canvasObject.GetComponent<Canvas> ().enabled = true;
	}

}
