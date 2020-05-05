using UnityEngine;
using System.Collections;
using SimpleJSON;

public class WeatherAPI : MonoBehaviour {
	
	public string url = "http://api.openweathermap.org/data/2.5/weather?q=Ottawa,CA&APPID=81f12442b3aa60f4ed8b21b63bd4fee0";

	public string city;
	public string country;
	public string weatherDescription;
	public float temp;
	public float temp_min;
	public float temp_max;
	public float rain;
	public float wind;
	public float clouds;
    public string weatherIcon;

    // Use this for initialization
    IEnumerator Start () {
		WWW request = new WWW(url);
		yield return request;

		if (request.error == null || request.error == "") 
		{
			setWeatherAttributes(request.text);
		} 
		else 
		{
			Debug.Log("Error: " + request.error);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void setWeatherAttributes(string jsonString) {
		var weatherJson = JSON.Parse(jsonString);
		city = weatherJson["name"].Value;
		country = weatherJson["sys"]["country"].Value;
		weatherDescription = weatherJson["weather"][0]["description"].Value;
		temp = weatherJson["main"]["temp"].AsFloat;
		temp_min = weatherJson["main"]["temp_min"].AsFloat;
		temp_max = weatherJson["main"]["temp_max"].AsFloat;
		rain = weatherJson["rain"]["3h"].AsFloat;
		clouds = weatherJson["clouds"]["all"].AsInt;
		wind = weatherJson["wind"]["speed"].AsFloat;
        weatherIcon = weatherJson["weather"][0]["icon"].Value;

    }
}

//EXAMPLE RESPONSE OBJECT
//{"coord":{"lon":139,"lat":35},
//	"sys":{"country":"JP","sunrise":1369769524,"sunset":1369821049},
//	"weather":[{"id":804,"main":"clouds","description":"overcast clouds","icon":"04n"}],
//	"main":{"temp":289.5,"humidity":89,"pressure":1013,"temp_min":287.04,"temp_max":292.04},
//	"wind":{"speed":7.31,"deg":187.002},
//	"rain":{"3h":0},
//	"clouds":{"all":92},
//	"dt":1369824698,
//	"id":1851632,
//	"name":"Shuzenji",
//	"cod":200}