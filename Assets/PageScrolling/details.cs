using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class details : MonoBehaviour {

    public bool Toggle;
    public GameObject canvasObject;

    // Use this for initialization
    void Start () {
        Toggle = true;
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Toggle = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Toggle = false;
        }
    }

    void DisableCanvas()
    {
        canvasObject.GetComponent<Canvas>().enabled = false;
    }
}
