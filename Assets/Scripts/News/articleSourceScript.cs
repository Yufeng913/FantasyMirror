using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class articleSourceScript : MonoBehaviour
{


    private newsAPI news;
    Text articleSourceText;
    Text articleSourceText_L;
    Text articleSourceText2_L;
    Text articleSourceText3_L;
    Text articleSourceText4_L;

    // Use this for initialization
    void Start()
    {

        articleSourceText = GameObject.Find("articleSourceDisplay1").GetComponent<Text>();
        articleSourceText_L = GameObject.Find("articleSourceDisplay1_L").GetComponent<Text>();
        articleSourceText2_L = GameObject.Find("articleSourceDisplay2_L").GetComponent<Text>();
        articleSourceText3_L = GameObject.Find("articleSourceDisplay3_L").GetComponent<Text>();
        articleSourceText4_L = GameObject.Find("articleSourceDisplay4_L").GetComponent<Text>();
        news = newsAPI.FindObjectOfType<newsAPI>();
    }

    // Update is called once per frame
    void Update()
    {

        if (news != null)
        {
            articleSourceText.text = news.articleSource;
            articleSourceText_L.text = news.articleSource;
            articleSourceText2_L.text = news.articleSource2;
            articleSourceText3_L.text = news.articleSource3;
            articleSourceText4_L.text = news.articleSource4;

        }
    }
}
