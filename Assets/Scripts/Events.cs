using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;


public class Events : MonoBehaviour
{

    public TextAsset randomQuotesTextFile;
    public string randomSpeechDatabase;
    public List<string> randomQuote;

    public string speechMessage = "";

    public bool showText = false;
    public bool deleteText = false;


    private float timeLeft;
    private float counterLength = 0.0f;

    private int sizeOfList;

    private float timeRandomSpeech;

    public bool startBubbleAnimate;

    // Use this for initialization
    void Start()
    {


        randomSpeechDatabase = randomQuotesTextFile.text;
        randomQuote = new List<string>();
        randomQuote.AddRange(randomSpeechDatabase.Split("\n"[0]));

        int sizeOfList = randomQuote.Count;

        timeRandomSpeech = UnityEngine.Random.Range(8, 15);

        timeLeft = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {


        timeRandomSpeech -= Time.deltaTime;


        if (timeRandomSpeech < 0)
        {
            startBubbleAnimate = true;
            showMsg();
            timeRandomSpeech = UnityEngine.Random.Range(8, 15);
        }





        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");

            //showMsg();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            deleteText = true;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            deleteText = true;
            timeLeft = 4.0f;
            startBubbleAnimate = false;
        }
    }

    void showMsg()
    {

        int randomIndex = UnityEngine.Random.Range(0, 20);
        Debug.Log(randomIndex);
        //assign text
        speechMessage = randomQuote[randomIndex];

        foreach (char letter in speechMessage.ToCharArray())
        {
            counterLength++;
        }

        //  timeLeft = counterLength * 0.5f;
        timeLeft = 4.0f;

        showText = true;

    }


}
