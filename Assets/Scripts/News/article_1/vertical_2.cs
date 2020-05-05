using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vertical_2 : MonoBehaviour {

    newsNav_L vertical;
    SpriteRenderer spriteRender;

    // Use this for initialization
    void Start () {
        vertical = newsNav_L.FindObjectOfType<newsNav_L>();
        GetComponent<Text>().enabled = true;
        spriteRender = GameObject.Find("articleImageDisplay2_L").GetComponent<SpriteRenderer>();
        spriteRender.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (vertical.counter != 1)
        {
            GetComponent<Text>().enabled = false;
            spriteRender.enabled = false;
        }
        if (vertical.counter == 1 && vertical.expand == true)
        {
            GetComponent<Text>().enabled = true;
            spriteRender.enabled = true;
        }

    }
}
