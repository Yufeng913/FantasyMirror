using UnityEngine;
using System.Collections;

public class articleImageScript_3 : MonoBehaviour
{

    private newsAPI news;
    private float time;
    SpriteRenderer spriteRender;
	gestureScrolling gesture;
	TimerVoiceReg voice;

    void Start()
    {
        news = newsAPI.FindObjectOfType<newsAPI>();
        time = 1.0f;
        spriteRender = GameObject.Find("articleImageDisplay3_L").GetComponent<SpriteRenderer>();
        spriteRender.enabled = false;
		gesture = gestureScrolling.FindObjectOfType<gestureScrolling>();
		voice = TimerVoiceReg.FindObjectOfType<TimerVoiceReg>();
    }

    void Update()
    {

        time -= Time.deltaTime;

        if (time < 0)
        {
            if (news != null)
            {
                StartCoroutine(loadSpriteIMG());
            }

            time = 1000.0f;

        }

		if (voice.zoomInV == true)
		{
			spriteRender.enabled = true;

			Debug.Log("z");
		}
		if (voice.zoomOutV == true)
		{
			spriteRender.enabled = false;
		}


		if (gesture.zoomIn == true)
		{
			spriteRender.enabled = true;

			Debug.Log("z");
		}
		if (gesture.zoomOut == true)
		{
			spriteRender.enabled = false;
		}


        if (Input.GetKeyDown(KeyCode.Z))
        {
            spriteRender.enabled = true;
    
            Debug.Log("z");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            spriteRender.enabled = false;
        }

    }

    IEnumerator loadSpriteIMG()
    {
        string URL = news.articleImage3;

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

                
                GetComponent<SpriteRenderer>().sprite = sprite;
                //CHANGE CURRENT SPRITE
            }

        }
        // yield return new WaitForSeconds(25.0f);
    }
}