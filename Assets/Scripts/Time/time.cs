using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;


public class time : MonoBehaviour {

    Text timeText;

    Text dickTime;

    Text dickTime2;


    // Use this for initialization
    void Start () {

        timeText = GameObject.Find("timedisplay").GetComponent<Text>();

        dickTime = GameObject.Find("timedisplay").GetComponent<Text>();

        dickTime2 = GameObject.Find("timedisplay").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        //DateTime theTime = DateTime.Now;

        dickTime.text = DateTime.Now.ToLongTimeString();

        //float hour = theTime.Hour;
        //float minute = theTime.Minute;
        //float second = theTime.Second;

        //int theHour = Mathf.RoundToInt((float)hour);
        //int theMinute = Mathf.RoundToInt((float)minute);
        //int theSecond = Mathf.RoundToInt((float)second);


        //timeText.text = hour.ToString() + ":" + minute.ToString() + ":" + second.ToString();
            

    }

    private void timer1_Tick(object sender, EventArgs e)
    {
        dickTime2.text = DateTime.Now.ToLongTimeString();
        //timer1.Start();
    }
}
