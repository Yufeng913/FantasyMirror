using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

public class newsAPI : MonoBehaviour {

    public string url = "https://newsapi.org/v1/articles?source=reddit-r-all&apiKey=a7daf2dcc70946c4941610de6905c3a0";

    public string articleTitle;
    public string articleImage;
    public string articleSource;
    public string articleDescription;

    public string articleTitle2;
    public string articleImage2;
    public string articleSource2;
    public string articleDescription2;

    public string articleTitle3;
    public string articleImage3;
    public string articleSource3;
    public string articleDescription3;

    public string articleTitle4;
    public string articleImage4;
    public string articleSource4;
    public string articleDescription4;


    // Use this for initialization
    IEnumerator Start()
    {
        WWW request = new WWW(url);
        yield return request;

        if (request.error == null || request.error == "")
        {
            setNewsAttributes(request.text);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }




    void setNewsAttributes(string jsonString)
    {
        var newsJson = JSON.Parse(jsonString);

        articleSource = newsJson["source"].Value;
        articleTitle = newsJson["articles"][0]["title"].Value;
        articleImage = newsJson["articles"][0]["urlToImage"].Value;
        articleDescription = newsJson["articles"][0]["description"].Value;

        articleSource2 = newsJson["source"].Value;
        articleTitle2 = newsJson["articles"][1]["title"].Value;
        articleImage2 = newsJson["articles"][1]["urlToImage"].Value;
        articleDescription2 = newsJson["articles"][1]["description"].Value;

        articleSource3 = newsJson["source"].Value;
        articleTitle3 = newsJson["articles"][2]["title"].Value;
        articleImage3 = newsJson["articles"][2]["urlToImage"].Value;
        articleDescription3 = newsJson["articles"][2]["description"].Value;

        articleSource4 = newsJson["source"].Value;
        articleTitle4 = newsJson["articles"][3]["title"].Value;
        articleImage4 = newsJson["articles"][3]["urlToImage"].Value;
        articleDescription4 = newsJson["articles"][3]["description"].Value;

    }
}

//https://newsapi.org/
//my api for newsapi.org a7daf2dcc70946c4941610de6905c3a0
//https://newsapi.org/v1/articles?source=techcrunch&apiKey={a7daf2dcc70946c4941610de6905c3a0}



