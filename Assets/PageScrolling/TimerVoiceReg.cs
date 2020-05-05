using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;

public class TimerVoiceReg : MonoBehaviour {

	public Text timer;
	public float timeRemaining;
	public bool timerBool = false;

	public GameObject canvasObject;

	public bool zoomOutV;
	public bool zoomInV;

	KeywordRecognizer KeywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	// Use this for initialization
	void Start () {

		KinectManager manager = KinectManager.Instance;

		keywords.Add("thirty seconds", () => {
			ThirtySecCalled();
		});

		keywords.Add("one minute", () => {
			OneMinute();
		});


		keywords.Add("two minutes", () => {
			TwoMinutes();
		});

		keywords.Add("three minutes", () => {
			ThreeMinutes();
		});

		keywords.Add("five minutes", () => {
			FiveMinutes();
		});

		keywords.Add("fifteen minutes", () => {
			FifteenMinutes();
		});


		keywords.Add("thirty minutes", () => {
			ThirtyMinutes();
		});

		keywords.Add("one hour", () => {
			OneHour();
		});

		keywords.Add("two hours", () => {
			TwoHours();
		});

		keywords.Add("three hours", () => {
			ThreeHours();
		});

		keywords.Add("four hours", () => {
			FourHours();
		});

		keywords.Add("pause timer", () => {
			PauseTimerCalled();
		});

		keywords.Add("start timer", () => {
			StartTimerCalled();
		});

		keywords.Add("sketch mode", () => {
			SketchMode();
		});

		keywords.Add("go back home", () => {
			GoBack();
		});

		keywords.Add("zoom in", () => {
			ZoomIn();
			Debug.Log("penis");
		});

		keywords.Add("zoom out", () => {
			ZoomOut();
		});

		KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		KeywordRecognizer.Start();
		
	}

	void DisableCanvas(){
		canvasObject.GetComponent<Canvas> ().enabled = false;
	}

	void EnableCanvas(){
		canvasObject.GetComponent<Canvas> ().enabled = true;
	}

	void SketchMode()
	{
		//print("thirty Seconds");
		SceneManager.LoadScene("yufeng's package");
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void GoBack()
	{
		//print("1");
		SceneManager.LoadScene("Page animation scrolling + angle track2");
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void ThirtySecCalled()
	{
		print("thirty Seconds");
		timeRemaining = 30;
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void OneMinute()
	{
		print("1");
		timeRemaining = 60;
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void TwoMinutes()
	{
		print("2");
		timeRemaining = 120;
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void ThreeMinutes()
	{
		print("3");
		timeRemaining = 180;
		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void FiveMinutes()
	{
		timeRemaining = 300;
	}

	void FifteenMinutes()
	{
		timeRemaining = 900;
	}

	void ThirtyMinutes()
	{
		timeRemaining = 1800;
	}

	void OneHour()
	{
		timeRemaining = 3600;
	}

	void TwoHours()
	{
		timeRemaining = 7200;
	}

	void ThreeHours()
	{
		timeRemaining = 10800;
	}

	void FourHours()
	{
		timeRemaining = 14400;
	}

	void ZoomIn()
	{
		zoomInV = true;
	}

	void ZoomOut()
	{
		zoomOutV = true;
	}

	void StartTimerCalled()
	{
		print("Start");
		if (timeRemaining > 0) {
			timerBool = true;
		}

		//SceneManager.LoadScene("dickscene", LoadSceneMode.Additive);
	}

	void PauseTimerCalled()
	{
		print("Pause");
		timerBool = false;
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
	void Update () {

		if (timerBool == true)
			timerStart ();

		if (timeRemaining < 1) {
			timerBool = false;
		}

		/*
		if (Input.GetKeyDown (KeyCode.Z) && timeRemaining > 0) {
			timerBool = true;
		}

		if (Input.GetKeyDown (KeyCode.Keypad1)) {
			timeRemaining = 60;
		}

		if (Input.GetKeyDown (KeyCode.Keypad2)) {
			timeRemaining = 120;
		}

		if (Input.GetKeyDown (KeyCode.Keypad3)) {
			timeRemaining = 180;
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			timerBool = false;
		}

*/

		TimeDisplay ();

		zoomInV = false;
		zoomOutV = false;
	
	}


	public void timerStart(){
		timeRemaining -= Time.deltaTime;
	}

	public void TimeDisplay(){

		string hours = ((int)timeRemaining / 60 / 60).ToString ();
		string minutes = ((int)timeRemaining / 60).ToString ();
		string seconds = (timeRemaining % 60).ToString ("f2");

		timer.text = hours + ":" + minutes + ":" + seconds; //(int)timeRemaining;
	}




}
