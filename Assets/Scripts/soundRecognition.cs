using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;

public class soundRecognition : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        keywords.Add("filters", () => {
            FiltersCalled();
        });

		keywords.Add("hello", () => {
			HelloCalled();
		});


		keywords.Add("companion go away", () => {
			ByeCalled();
		});

		keywords.Add("jump", () => {
			JumpCalled();
		});
        KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
        KeywordRecognizer.Start();
    }



    void FiltersCalled()
    {
        print("filters");
        //SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
    }

	void JumpCalled()
	{
		print("jump");
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void HelloCalled()
	{
		print("Hello");
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void ByeCalled()
	{
		print("companion come");
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

    void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;

        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);

    }


    KeywordRecognizer KeywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();



}

