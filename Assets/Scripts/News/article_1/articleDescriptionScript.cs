using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class articleDescriptionScript : MonoBehaviour
{


    private newsAPI news;
    Text articleDescriptionText;

    Text articleDescriptionText2;
    Text articleDescriptionText3;
    Text articleDescriptionText4;

    // Use this for initialization
    void Start()
    {

        articleDescriptionText = GameObject.Find("articleDescriptionDisplay1_L").GetComponent<Text>();

        articleDescriptionText2 = GameObject.Find("articleDescriptionDisplay2_L").GetComponent<Text>();
        articleDescriptionText3 = GameObject.Find("articleDescriptionDisplay3_L").GetComponent<Text>();
 articleDescriptionText4 = GameObject.Find("articleDescriptionDisplay4_L").GetComponent<Text>();

        news = newsAPI.FindObjectOfType<newsAPI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (news != null)
        {
            articleDescriptionText.text = news.articleDescription;

            articleDescriptionText2.text = news.articleDescription2;
           articleDescriptionText3.text = news.articleDescription3;
            articleDescriptionText4.text = news.articleDescription4;


        }
    }
}
