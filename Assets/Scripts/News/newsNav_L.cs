using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newsNav_L : MonoBehaviour {

    public int counter;
    public bool expand;

	gestureScrolling gesutre;
	TimerVoiceReg voice;


    // Use this for initialization
    void Start () {

        counter = 0;
        expand = false; 

        GetComponent<Text>().enabled = false;

		gesutre = gestureScrolling.FindObjectOfType<gestureScrolling>();
		voice = TimerVoiceReg.FindObjectOfType<TimerVoiceReg>();

    }

    // Update is called once per frame
    void Update()
    {
		
        if (Input.GetKeyDown(KeyCode.Z))
        {
            expand = true;
            GetComponent<Text>().enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            expand = false;
            GetComponent<Text>().enabled = false;
            counter = 0;
        }

		if (voice.zoomInV == true)
		{
			expand = true;
			GetComponent<Text>().enabled = true;
			//voice.zoomInV = false;
		}
		if (voice.zoomOutV == true)
		{
			expand = false;
			GetComponent<Text>().enabled = false;
			counter = 0;
			//voice.zoomOutV = false;
		}



		if (gesutre.zoomIn == true)
		{
			expand = true;
			GetComponent<Text>().enabled = true;
		}
		if (gesutre.zoomOut == true)
		{
			expand = false;
			GetComponent<Text>().enabled = false;
			counter = 0;
		}


		if (gesutre.swipeDown == true)
		{
			Debug.Log ("down_2");
			if (counter < 3)
			{
				counter++;
			}

		}
		if (gesutre.swipeUp == true)
		{
			Debug.Log ("up_2");
			if (counter != 0)
			{
				counter--;
			}

		}


        if (Input.GetKeyDown(KeyCode.C))
        {
            if (counter < 3)
            {
                counter++;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
          
            if (counter != 0)
            {
                counter--;
            }
            
        }
        
    }
}
