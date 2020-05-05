using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Windows.Speech;
using System.Linq;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Mask))]
[RequireComponent(typeof(ScrollRect))]

public class gestureScrolling : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler 
{

	public bool swipeUp;
	public bool swipeDown;

	public bool zoomOut;
	public bool zoomIn;

	[Tooltip("Camera used for screen-to-world calculations. This is usually the main camera.")]
	public Camera screenCamera;

	[Tooltip("Whether the presentation slides can be changed with gestures (SwipeLeft & SwipeRight).")]
	public bool slideChangeWithGestures = true;
	[Tooltip("Whether the presentation slides can be changed with keys (PgDown & PgUp).")]
	public bool slideChangeWithKeys = true;
	[Tooltip("Speed of spinning, when presentation slides change.")]
	public int spinSpeed = 5;

	[Tooltip("List of the presentation slides.")]
	public List<Texture> slideTextures;
	[Tooltip("List of the side planes, comprising the presentation cube.")]
	public List<GameObject> cubeSides;


	//private int maxSides = 0;
	private int maxTextures = 0;
	private int side = 0;
	private int tex = 0;
	private bool isSpinning = false;

	private int[] hsides = { 0, 1, 2, 3 };  // left, front, right, back
	private int[] vsides = { 4, 1, 5, 3};  // up, front, down, back

	private CubeGestureListener gestureListener;
	private Quaternion initialRotation;
	private int stepsToGo = 0;

	private float rotationStep;
	private Vector3 rotationAxis;


	/// <summary>
	/// ////////////////////////////
	/// </summary>
	/// 
	///  [Tooltip("Set starting page index - starting from 0")]
	public int startingPage = 0;
	[Tooltip("Threshold time for fast swipe in seconds")]
	public float fastSwipeThresholdTime = 0.3f;
	[Tooltip("Threshold time for fast swipe in (unscaled) pixels")]
	public int fastSwipeThresholdDistance = 100;
	[Tooltip("How fast will page lerp to target position")]
	public float decelerationRate = 10f;
	[Tooltip("Button to go to the previous page (optional)")]
	public GameObject prevButton;
	[Tooltip("Button to go to the next page (optional)")]
	public GameObject nextButton;
	[Tooltip("Sprite for unselected page (optional)")]
	public Sprite unselectedPage;
	[Tooltip("Sprite for selected page (optional)")]
	public Sprite selectedPage;
	[Tooltip("Container with page images (optional)")]
	public Transform pageSelectionIcons;

	// fast swipes should be fast and short. If too long, then it is not fast swipe
	private int _fastSwipeThresholdMaxLimit;

	private ScrollRect _scrollRectComponent;
	private RectTransform _scrollRectRect;
	private RectTransform _container;

	private bool _horizontal;

	// number of pages in container
	private int _pageCount;
	private int _currentPage;

	// whether lerping is in progress and target lerp position
	private bool _lerp;
	private Vector2 _lerpTo;

	// target position of every page
	private List<Vector2> _pagePositions = new List<Vector2>();

	// in draggging, when dragging started and where it started
	private bool _dragging;
	private float _timeStamp;
	private Vector2 _startPosition;

	// for showing small page icons
	private bool _showPageSelection;
	private int _previousPageSelectionIndex;
	// container with Image components - one Image for each page
	private List<Image> _pageSelectionImages;

	//
	//
	//
	public Text jumpJackText;
	int jumpCounter = 0;

	//
	//
	//
	public Text timer;
	float timeRemaining = 60;
	public bool timerBool = false;

	//
	//
	//
	public Text instructions;

	//
	//
	//
	KeywordRecognizer KeywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();


	void Start() 
	{
		// hide mouse cursor
		//Cursor.visible = false;

		// by default set the main-camera to be screen-camera
		if (screenCamera == null) 
		{
			screenCamera = Camera.main;
		}

		// calculate max slides and textures
		//maxSides = cubeSides.Count;
		maxTextures = slideTextures.Count;

		initialRotation = screenCamera ? Quaternion.Inverse(screenCamera.transform.rotation) * transform.rotation : transform.rotation;
		isSpinning = false;

		tex = 0;
		side = hsides[1];

		if(side < cubeSides.Count && cubeSides[side] && cubeSides[side].GetComponent<Renderer>())
		{
			cubeSides[side].GetComponent<Renderer>().material.mainTexture = slideTextures[tex];
		}

		// get the gestures listener
		gestureListener = CubeGestureListener.Instance;


		_scrollRectComponent = GetComponent<ScrollRect>();
		_scrollRectRect = GetComponent<RectTransform>();
		_container = _scrollRectComponent.content;
		_pageCount = _container.childCount;

		// is it horizontal or vertical scrollrect
		if (_scrollRectComponent.horizontal && !_scrollRectComponent.vertical) {
			_horizontal = true;
		} else if (!_scrollRectComponent.horizontal && _scrollRectComponent.vertical) {
			_horizontal = false;
		} else {
			Debug.LogWarning("Confusing setting of horizontal/vertical direction. Default set to horizontal.");
			_horizontal = true;
		}

		_lerp = false;

		// init
		SetPagePositions();
		SetPage(startingPage);
		InitPageSelection();
		SetPageSelection(startingPage);

		// prev and next buttons
		if (nextButton)
			nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });

		if (prevButton)
			prevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });

		//
		//
		//

		//jumpJackText = GetComponent<Text> ();

		//
		//
		//
		keywords.Add("restart jump timer", () => {
			GoCalled();
		});
		KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		KeywordRecognizer.Start();
	}



	void GoCalled()
	{
		
			timerBool = false;
			timeRemaining = 60;
			jumpCounter = 0;
			jumpJackText.text = jumpCounter.ToString();

		//print("Go");
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

	void Update() 
	{
		swipeDown = false;
		swipeUp = false;
		zoomOut = false;
		zoomIn = false;

		// dont run Update() if there is no gesture listener
		if(!gestureListener)
			return;

		if(!isSpinning)
		{
			if(slideChangeWithKeys)
			{
				if (Input.GetKeyDown (KeyCode.PageDown) && timeRemaining > 0) {
					//RotateLeft();
					IncrementScore ();
					timerBool = true;
				}
				else if (Input.GetKeyDown (KeyCode.PageUp)) {
					timerBool = false;
					timeRemaining = 60;
					jumpCounter = 0;
					jumpJackText.text = jumpCounter.ToString();
				}
			}

			if(slideChangeWithGestures && gestureListener)
			{
				if (gestureListener.IsSwipeLeft ())
					//RotateLeft();
					NextScreen ();
				else if (gestureListener.IsSwipeRight ())
					//RotateRight();
					PreviousScreen ();
				if (gestureListener.IsSwipeDown ()) {

					swipeDown = true;
					Debug.Log ("down");
				}

				if (gestureListener.IsSwipeUp ()) {

					swipeUp = true;
					Debug.Log ("up");

				}

				if (gestureListener.IsZoomOut ()) {

					zoomOut = true;
					Debug.Log ("zoomout");

				}

				if (gestureListener.IsZoomIn ()) {

					zoomIn = true;
					Debug.Log ("zoomin");

				}


				//else if(gestureListener.IsSwipeUp())
					//RotateUp();

			}

			if (slideChangeWithGestures && gestureListener && timeRemaining > 0) {
				if (gestureListener.IsJump ()) {
					IncrementScore ();
					timerBool = true;
				}
			}
		}
		else
		{
			// spin the presentation
			if(stepsToGo > 0)
			{
				//if(Time.realtimeSinceStartup >= nextStepTime)
				{
					if(screenCamera)
						transform.RotateAround(transform.position, screenCamera.transform.TransformDirection(rotationAxis), rotationStep);
					else
						transform.Rotate(rotationAxis * rotationStep, Space.World);

					stepsToGo--;
					//nextStepTime = Time.realtimeSinceStartup + Time.deltaTime;
				}
			}
			else
			{
				Quaternion cubeRotation = Quaternion.Euler(rotationAxis * rotationStep * 90f / spinSpeed) * initialRotation;
				transform.rotation = screenCamera ? screenCamera.transform.rotation * cubeRotation : cubeRotation;
				isSpinning = false;
			}
		}

		//

		if (_lerp) {
			// prevent overshooting with values greater than 1
			float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
			_container.anchoredPosition = Vector2.Lerp(_container.anchoredPosition, _lerpTo, decelerate);
			// time to stop lerping?
			if (Vector2.SqrMagnitude(_container.anchoredPosition - _lerpTo) < 0.25f) {
				// snap to target and stop lerping
				_container.anchoredPosition = _lerpTo;
				_lerp = false;
				// clear also any scrollrect move that may interfere with our lerping
				_scrollRectComponent.velocity = Vector2.zero;
			}

			// switches selection icon exactly to correct page
			if (_showPageSelection) {
				SetPageSelection(GetNearestPage());
			}
		}

		TimeDisplay ();
		Instructions ();

		if (timerBool == true)
			timeRemaining -= Time.deltaTime;

	}

	public void timerStart(){
		//timeRemaining = 60;
		timeRemaining -= Time.deltaTime;
	}

	public void IncrementScore(){
		jumpCounter++;
		jumpJackText.text = jumpCounter.ToString();
	}

	public void TimeDisplay(){
		if (timeRemaining > 0) {
			timer.text = "Time remaining " + (int)timeRemaining;
		} else {
			timer.text = "Time's up, your score is " + jumpCounter;
		}
	}

	public void Instructions ()
	{
		if (timeRemaining == 60) {
			instructions.text = "Jump to start the timer";
		}
		if (timeRemaining < 60 && timeRemaining > 1) {
			instructions.text = "Jump as fast as you can!";
		} if (timeRemaining < 0.01) {
			instructions.text = "Great Job!";
		}
				
	

	}


	// rotates cube left
	private void RotateLeft()
	{
		// set the next texture slide
		tex = (tex + 1) % maxTextures;

		// rotate hsides left
		SetSides(ref hsides, hsides[1], hsides[2], hsides[3], hsides[0]);
		SetSides(ref vsides, -1, hsides[1], -1, hsides[3]);
		side = hsides[1];

		// set the slide on the selected side
		if(side < cubeSides.Count && cubeSides[side] && cubeSides[side].GetComponent<Renderer>())
		{
			cubeSides[side].GetComponent<Renderer>().material.mainTexture = slideTextures[tex];
		}

		// rotate the presentation
		isSpinning = true;
		initialRotation = screenCamera ? Quaternion.Inverse(screenCamera.transform.rotation) * transform.rotation : transform.rotation;

		rotationStep = spinSpeed; // new Vector3(0, spinSpeed, 0);
		rotationAxis = Vector3.up;

		stepsToGo = 90 / spinSpeed;
		//nextStepTime = 0f;
	}

	// rotates cube right
	private void RotateRight()
	{
		// set the previous texture slide
		if(tex <= 0)
			tex = maxTextures - 1;
		else
			tex -= 1;

		// rotate hsides right
		SetSides(ref hsides, hsides[3], hsides[0], hsides[1], hsides[2]);
		SetSides(ref vsides, -1, hsides[1], -1, hsides[3]);
		side = hsides[1];

		// set the slide on the selected side
		if(side < cubeSides.Count && cubeSides[side] && cubeSides[side].GetComponent<Renderer>())
		{
			cubeSides[side].GetComponent<Renderer>().material.mainTexture = slideTextures[tex];
		}

		// rotate the presentation
		isSpinning = true;
		initialRotation = screenCamera ? Quaternion.Inverse(screenCamera.transform.rotation) * transform.rotation : transform.rotation;

		rotationStep = -spinSpeed; // new Vector3(0, -spinSpeed, 0);
		rotationAxis = Vector3.up;

		stepsToGo = 90 / spinSpeed;
		//nextStepTime = 0f;
	}

	// rotates cube up
	private void RotateUp()
	{
		// set the next texture slide
		tex = (tex + 1) % maxTextures;

		// rotate vsides up
		SetSides(ref vsides, vsides[1], vsides[2], vsides[3], vsides[0]);
		SetSides(ref hsides, -1, vsides[1], -1, vsides[3]);
		side = hsides[1];

		// set the slide on the selected side
		if(side < cubeSides.Count && cubeSides[side] && cubeSides[side].GetComponent<Renderer>())
		{
			cubeSides[side].GetComponent<Renderer>().material.mainTexture = slideTextures[tex];
		}

		// rotate the presentation
		isSpinning = true;
		initialRotation = screenCamera ? Quaternion.Inverse(screenCamera.transform.rotation) * transform.rotation : transform.rotation;

		rotationStep = spinSpeed; // new Vector3(spinSpeed, 0, 0);
		rotationAxis = Vector3.right;

		stepsToGo = 90 / spinSpeed;
		//nextStepTime = 0f;
	}

	// sets values of sides' array
	private void SetSides(ref int[] sides, int v0, int v1, int v2, int v3)
	{
		if(v0 >= 0)
		{
			sides[0] = v0;
		}

		if(v1 >= 0)
		{
			sides[1] = v1;
		}

		if(v2 >= 0)
		{
			sides[2] = v2;
		}

		if(v3 >= 0)
		{
			sides[3] = v3;
		}
	}

	private void SetPagePositions() {
		int width = 0;
		int height = 0;
		int offsetX = 0;
		int offsetY = 0;
		int containerWidth = 0;
		int containerHeight = 0;

		if (_horizontal) {
			// screen width in pixels of scrollrect window
			width = (int)_scrollRectRect.rect.width;
			// center position of all pages
			offsetX = width / 2;
			// total width
			containerWidth = width * _pageCount;
			// limit fast swipe length - beyond this length it is fast swipe no more
			_fastSwipeThresholdMaxLimit = width;
		} else {
			height = (int)_scrollRectRect.rect.height;
			offsetY = height / 2;
			containerHeight = height * _pageCount;
			_fastSwipeThresholdMaxLimit = height;
		}

		// set width of container
		Vector2 newSize = new Vector2(containerWidth, containerHeight);
		_container.sizeDelta = newSize;
		Vector2 newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
		_container.anchoredPosition = newPosition;

		// delete any previous settings
		_pagePositions.Clear();

		// iterate through all container childern and set their positions
		for (int i = 0; i < _pageCount; i++) {
			RectTransform child = _container.GetChild(i).GetComponent<RectTransform>();
			Vector2 childPosition;
			if (_horizontal) {
				childPosition = new Vector2(i * width - containerWidth / 2 + offsetX, 0f);
			} else {
				childPosition = new Vector2(0f, -(i * height - containerHeight / 2 + offsetY));
			}
			child.anchoredPosition = childPosition;
			_pagePositions.Add(-childPosition);
		}
	}

	//------------------------------------------------------------------------
	private void SetPage(int aPageIndex) {
		aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
		_container.anchoredPosition = _pagePositions[aPageIndex];
		_currentPage = aPageIndex;
	}

	//------------------------------------------------------------------------
	private void LerpToPage(int aPageIndex) {
		aPageIndex = Mathf.Clamp(aPageIndex, 0, _pageCount - 1);
		_lerpTo = _pagePositions[aPageIndex];
		_lerp = true;
		_currentPage = aPageIndex;
	}

	//------------------------------------------------------------------------
	private void InitPageSelection() {
		// page selection - only if defined sprites for selection icons
		_showPageSelection = unselectedPage != null && selectedPage != null;
		if (_showPageSelection) {
			// also container with selection images must be defined and must have exatly the same amount of items as pages container
			if (pageSelectionIcons == null || pageSelectionIcons.childCount != _pageCount) {
				Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
				_showPageSelection = false;
			} else {
				_previousPageSelectionIndex = -1;
				_pageSelectionImages = new List<Image>();

				// cache all Image components into list
				for (int i = 0; i < pageSelectionIcons.childCount; i++) {
					Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
					if (image == null) {
						Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
					}
					_pageSelectionImages.Add(image);
				}
			}
		}
	}

	//------------------------------------------------------------------------
	private void SetPageSelection(int aPageIndex) {
		// nothing to change
		if (_previousPageSelectionIndex == aPageIndex) {
			return;
		}

		// unselect old
		if (_previousPageSelectionIndex >= 0) {
			_pageSelectionImages[_previousPageSelectionIndex].sprite = unselectedPage;
			_pageSelectionImages[_previousPageSelectionIndex].SetNativeSize();
		}

		// select new
		_pageSelectionImages[aPageIndex].sprite = selectedPage;
		_pageSelectionImages[aPageIndex].SetNativeSize();

		_previousPageSelectionIndex = aPageIndex;
	}

	//------------------------------------------------------------------------
	private void NextScreen() {
		LerpToPage(_currentPage + 1);
	}

	//------------------------------------------------------------------------
	private void PreviousScreen() {
		LerpToPage(_currentPage - 1);
	}

	//------------------------------------------------------------------------
	private int GetNearestPage() {
		// based on distance from current position, find nearest page
		Vector2 currentPosition = _container.anchoredPosition;

		float distance = float.MaxValue;
		int nearestPage = _currentPage;

		for (int i = 0; i < _pagePositions.Count; i++) {
			float testDist = Vector2.SqrMagnitude(currentPosition - _pagePositions[i]);
			if (testDist < distance) {
				distance = testDist;
				nearestPage = i;
			}
		}

		return nearestPage;
	}

	//------------------------------------------------------------------------
	public void OnBeginDrag(PointerEventData aEventData) {
		// if currently lerping, then stop it as user is draging
		_lerp = false;
		// not dragging yet
		_dragging = false;
	}

	//------------------------------------------------------------------------
	public void OnEndDrag(PointerEventData aEventData) {
		// how much was container's content dragged
		float difference;
		if (_horizontal) {
			difference = _startPosition.x - _container.anchoredPosition.x;
		} else {
			difference = - (_startPosition.y - _container.anchoredPosition.y);
		}

		// test for fast swipe - swipe that moves only +/-1 item
		if (Time.unscaledTime - _timeStamp < fastSwipeThresholdTime &&
			Mathf.Abs(difference) > fastSwipeThresholdDistance &&
			Mathf.Abs(difference) < _fastSwipeThresholdMaxLimit) {
			if (difference > 0) {
				NextScreen();
			} else {
				PreviousScreen();
			}
		} else {
			// if not fast time, look to which page we got to
			LerpToPage(GetNearestPage());
		}

		_dragging = false;
	}

	//------------------------------------------------------------------------
	public void OnDrag(PointerEventData aEventData) {
		if (!_dragging) {
			// dragging started
			_dragging = true;
			// save time - unscaled so pausing with Time.scale should not affect it
			_timeStamp = Time.unscaledTime;
			// save current position of cointainer
			_startPosition = _container.anchoredPosition;
		} else {
			if (_showPageSelection) {
				SetPageSelection(GetNearestPage());
			}
		}
	}



}
