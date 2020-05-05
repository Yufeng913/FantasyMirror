using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class articleTitleScript : MonoBehaviour {

   
    private newsAPI news;
    Text articleTitleText;
    Text articleTitleText_L;

    Text articleTitleText1_L;
    Text articleTitleText2_L;
    Text articleTitleText3_L;
    Text articleTitleText4_L;

    // Use this for initialization
    void Start () {
     
        articleTitleText = GameObject.Find("articleTitleDisplay1").GetComponent<Text>();
        articleTitleText1_L = GameObject.Find("articleTitleDisplay1_L").GetComponent<Text>();
        articleTitleText2_L = GameObject.Find("articleTitleDisplay2_L").GetComponent<Text>();
        articleTitleText3_L = GameObject.Find("articleTitleDisplay3_L").GetComponent<Text>();
        articleTitleText4_L = GameObject.Find("articleTitleDisplay4_L").GetComponent<Text>();
        news = newsAPI.FindObjectOfType<newsAPI>();
    }
	
	// Update is called once per frame
	void Update () {

        if (news != null)
        {
            articleTitleText.text = news.articleTitle;
            articleTitleText1_L.text = news.articleTitle;
            articleTitleText2_L.text = news.articleTitle2;
            articleTitleText3_L.text = news.articleTitle3;
            articleTitleText4_L.text = news.articleTitle4;

        }
    }
}
