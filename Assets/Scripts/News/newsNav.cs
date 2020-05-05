using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class newsNav : MonoBehaviour {

	gestureScrolling gesture;
	TimerVoiceReg voice;

    // Use this for initialization
    void Start () {

        GetComponent<Text>().enabled = true;

		gesture = gestureScrolling.FindObjectOfType<gestureScrolling>();
		voice = TimerVoiceReg.FindObjectOfType<TimerVoiceReg>();

    }

    // Update is called once per frame
    void Update()
    {
		if (voice.zoomInV == true)
		{
			GetComponent<Text>().enabled = false;
			//voice.zoomInV = false;
		}
		if (voice.zoomOutV == true)
		{
			GetComponent<Text>().enabled = true;
			//voice.zoomOutV = false;
		}


		if (gesture.zoomIn == true)
		{
			GetComponent<Text>().enabled = false;
		}
		if (gesture.zoomOut == true)
		{
			GetComponent<Text>().enabled = true;
		}


        if (Input.GetKeyDown(KeyCode.Z))
        {
            GetComponent<Text>().enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            GetComponent<Text>().enabled = true;
        }
        
    }
}
