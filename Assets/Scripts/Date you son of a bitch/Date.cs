using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Date : MonoBehaviour {

    Text dateTime;

    // Use this for initialization
    void Start () {

        dateTime = GameObject.Find("datedisplay").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        DateTime theTime = DateTime.Now;

        dateTime.text = DateTime.Now.ToLongDateString();

    }
}
