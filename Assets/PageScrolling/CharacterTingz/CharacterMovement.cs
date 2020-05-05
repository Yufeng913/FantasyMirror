using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour {


	private Rigidbody2D rBody;
	private Vector2 velocity;
	private Vector2 target = new Vector2(122, 250);

	public float rotation = 1.0f;
	private float rSwitch = 5;

	bool enter;
	bool Toggle;
	bool leave;
	bool idle;
	bool backIn;

	public int speed = 100;

	// Use this for initialization
	void Start () {
		rBody = GetComponent<Rigidbody2D>();
		velocity = new Vector2(Random.Range(0, 100), 100);

		enter = true;
		Toggle = false;
		leave = false;
		idle = true;
		backIn = false;


		keywords.Add("companion hide", () => {
			leave = true;
		});

		keywords.Add("go away companion", () => {
			leave = true;
		});

		keywords.Add("leave me alone", () => {
			leave = true;
		});

		keywords.Add("the companion is kind of annoying", () => {
			leave = true;
		});

		keywords.Add("companion come back", () => {
			idle = false;
			Toggle = true;
			enter = true;
			leave = false;
		});

		keywords.Add("where are you companion", () => {
			idle = false;
			Toggle = true;
			enter = true;
			leave = false;
		});

		keywords.Add("hi companion", () => {
			idle = false;
			Toggle = true;
			enter = true;
			leave = false;
		});

		KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		KeywordRecognizer.Start();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (Toggle == true)
		{
			rotation += Time.deltaTime * rSwitch;
			transform.rotation = Quaternion.Euler(0, 0, rotation);

			if (rotation >= 30)
				rotation = 30;

			if (rotation >= -30)
				rotation = -30;

			if (enter == false)
			{
				if (rBody.position.x >= 1030 || rBody.position.x <= 50)
				{
					velocity.x = -velocity.x;
					rSwitch *= -1;
				}

				if (rBody.position.y >= 1500 || rBody.position.y <= 350)
				{
					velocity.y = -velocity.y;
					rSwitch *= -1;
				}

				if(rBody.position.x >= 350 && rBody.position.y >= 1000 && rBody.position.x <= 500 && rBody.position.y <= 1900)
				{
					velocity.y = -velocity.y;
					rSwitch *= -1;
				}

				rBody.MovePosition(rBody.position + velocity * Time.fixedDeltaTime);
			}

			if (enter)
			{
				if (rBody.position.y >= 600)
				{
					enter = false;
				}

				rBody.MovePosition(rBody.position + velocity * Time.fixedDeltaTime);
			}
		}

		if (Toggle == false)
		{
			//rBody.MovePosition(rBody.position + new Vector2(0, 0) * Time.fixedDeltaTime);
			//rBody.transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target, 3 * Time.deltaTime);
		}

		if(leave == true)
		{
			rBody.MovePosition(rBody.position + new Vector2(Random.Range(0, 100), -170) * Time.fixedDeltaTime);

			if (rBody.position.y <= -150)
			{
				idle = true;
			}

		}

		if(idle == true)
		{
			rBody.MovePosition(rBody.position + new Vector2(0, 0) * Time.fixedDeltaTime);
		}

		Debug.Log(rBody.position);
	}

	void KeywordRecognizerOnPhraseRecognized(PhraseRecognizedEventArgs args)
	{
		System.Action keywordAction;

		if (keywords.TryGetValue(args.text, out keywordAction))
		{
			keywordAction.Invoke();
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Toggle = !Toggle;
		}

		rBody.transform.position = Vector2.MoveTowards(new Vector2(rBody.transform.position.x, rBody.transform.position.y), target, 3 * Time.deltaTime);
	}


	KeywordRecognizer KeywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

}

