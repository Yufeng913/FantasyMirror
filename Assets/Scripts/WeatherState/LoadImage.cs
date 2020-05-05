using UnityEngine;
using System.Collections;

public class LoadImage : MonoBehaviour
{

    private WeatherAPI weather;
    public bool knowsWeather;
    private float time;

    void Start()
    {
        weather = WeatherAPI.FindObjectOfType<WeatherAPI>();
        time = 0.1f;
    }

    void Update()
    {
        if (time >= 0)
        {
            time -= Time.deltaTime;
            return;
        }
        else
        {

         if (!knowsWeather && weather.temp != 0)
                {
                    Debug.Log(weather.weatherIcon);
                    StartCoroutine(loadSpriteIMG());

                }
            
            time = 10.0f;
        }

    
        
    }

    IEnumerator loadSpriteIMG()
    {
        string weatherIconCode = weather.weatherIcon;
        string weatherWebsite = "http://openweathermap.org/img/w/";
        string iconFormat = ".png";

        string URL = weatherWebsite + weatherIconCode + iconFormat;

        //CHECK YOUR INTERNET CONNECTION
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No Connection Internet");
            yield return null;
        }
        else
        {
            var www = new WWW(URL);
            Debug.Log("Download image on progress");
            yield return www;

            if (string.IsNullOrEmpty(www.text))
                Debug.Log("Download failed");
            else
            {
                Debug.Log("Download Succes");

                Texture2D texture = new Texture2D(1, 1);                //CREATE TEXTURE WIDTH = 1 AND HEIGHT =1
                www.LoadImageIntoTexture(texture);                      //LOAD DOWNLOADED TEXTURE TO VARIABEL TEXTURE
                Sprite sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height),      //LOAD TEXTURE FROM POINT (0,0) TO TEXTURE(WIDTH, HEIGHT)
                    Vector2.one / 2);                                       //SET PIVOT TO CENTER

                GetComponent<SpriteRenderer>().sprite = sprite;       //CHANGE CURRENT SPRITE
            }

        }
       // yield return new WaitForSeconds(25.0f);
    }
}