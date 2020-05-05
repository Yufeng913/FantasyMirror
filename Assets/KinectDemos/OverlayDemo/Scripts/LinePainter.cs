using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Windows.Speech;
using System.Linq;
public class LinePainter : MonoBehaviour 
{
	[Tooltip("Line renderer used for the line drawing.")]
	public LineRenderer linePrefab;

	[Tooltip("GUI-Text to display information messages.")]
	public GUIText infoText;


	private HandOverlayer handOverlayer = null;
	private List<GameObject> linesDrawn = new List<GameObject>();
	private LineRenderer currentLine;
	private int lineVertexIndex = 2;

	KeywordRecognizer KeywordRecognizer;
	Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

	void Start()
	{
		handOverlayer = GetComponent<HandOverlayer>();

		keywords.Add("undo last line", () => {
			DeleteLastLine();
		});

		KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
		KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
		KeywordRecognizer.Start();
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
		if(Input.GetKeyDown(KeyCode.U))
		{
			// U-key means Undo
			DeleteLastLine();
		}

		/*
		if (Input.GetKeyDown (KeyCode.R)) {
			DeleteLastLine ();
			DeleteLastLine2 ();
			DeleteLastLine3 ();
			DeleteLastLine4 ();

			DeleteLastLine5 ();
			DeleteLastLine6 ();
			DeleteLastLine7 ();
			DeleteLastLine8 ();

			DeleteLastLine9 ();
			DeleteLastLine10 ();
			DeleteLastLine11 ();
			DeleteLastLine12 ();

			DeleteLastLine13 ();
			DeleteLastLine14 ();
			DeleteLastLine15 ();
			DeleteLastLine16 ();
		}
		*/

		// display info message when a user is detected
		KinectManager manager = KinectManager.Instance;
		if(manager && manager.IsInitialized() && manager.IsUserDetected())
		{
			if(infoText)
			{
				infoText.text = "Grip hand to start drawing. Press [U] to undo the last line.";
			}
		}

		
		if(currentLine == null &&
		   (handOverlayer && (handOverlayer.GetLastHandEvent() == InteractionManager.HandEventType.Grip)))
		{
			// start drawing lines
			currentLine = Instantiate(linePrefab).GetComponent<LineRenderer>();
			currentLine.name = "Line" + linesDrawn.Count;
			currentLine.transform.parent = transform;

			Vector3 cursorPos = handOverlayer.GetCursorPos();
			cursorPos.z = Camera.main.nearClipPlane;
			
			Vector3 cursorSpacePos = Camera.main.ViewportToWorldPoint(cursorPos);
			currentLine.SetPosition(0, cursorSpacePos);
			currentLine.SetPosition(1, cursorSpacePos);

			lineVertexIndex = 2;
			linesDrawn.Add(currentLine.gameObject);

			StartCoroutine(DrawLine());
		}
		
		if (currentLine != null &&
		    (handOverlayer != null && (handOverlayer.GetLastHandEvent() == InteractionManager.HandEventType.Release)))
		{
			// end drawing lines
			currentLine = null;
		}
	}

	// undo the last drawn line
	public void DeleteLastLine()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-1];
			print ("undo last line");
			linesDrawn.RemoveAt(linesDrawn.Count-1);
			Destroy(goLastLine);
		}
	}

	/*

	public void DeleteLastLine2()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine2 = linesDrawn[linesDrawn.Count-2];

			linesDrawn.RemoveAt(linesDrawn.Count-2);
			Destroy(goLastLine2);
		}
	}

	public void DeleteLastLine3()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine3 = linesDrawn[linesDrawn.Count-3];

			linesDrawn.RemoveAt(linesDrawn.Count-3);
			Destroy(goLastLine3);
		}
	}

	public void DeleteLastLine4()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine4= linesDrawn[linesDrawn.Count-4];

			linesDrawn.RemoveAt(linesDrawn.Count-4);
			Destroy(goLastLine4);
		}
	}

	//
	//
	//

	public void DeleteLastLine5()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine5 = linesDrawn[linesDrawn.Count-5];

			linesDrawn.RemoveAt(linesDrawn.Count-5);
			Destroy(goLastLine5);
		}
	}


	public void DeleteLastLine6()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine6 = linesDrawn[linesDrawn.Count-6];

			linesDrawn.RemoveAt(linesDrawn.Count-6);
			Destroy(goLastLine6);
		}
	}

	public void DeleteLastLine7()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine7 = linesDrawn[linesDrawn.Count-7];

			linesDrawn.RemoveAt(linesDrawn.Count-7);
			Destroy(goLastLine7);
		}
	}

	public void DeleteLastLine8()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine8 = linesDrawn[linesDrawn.Count-8];

			linesDrawn.RemoveAt(linesDrawn.Count-8);
			Destroy(goLastLine8);
		}
	}

	// undo the last drawn line
	public void DeleteLastLine9()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine9 = linesDrawn[linesDrawn.Count-9];

			linesDrawn.RemoveAt(linesDrawn.Count-9);
			Destroy(goLastLine9);
		}
	}


	public void DeleteLastLine10()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-10];

			linesDrawn.RemoveAt(linesDrawn.Count-10);
			Destroy(goLastLine);
		}
	}

	public void DeleteLastLine11()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-11];

			linesDrawn.RemoveAt(linesDrawn.Count-11);
			Destroy(goLastLine);
		}
	}

	public void DeleteLastLine12()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-12];

			linesDrawn.RemoveAt(linesDrawn.Count-12);
			Destroy(goLastLine);
		}
	}

	//
	//
	//

	public void DeleteLastLine13()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-13];

			linesDrawn.RemoveAt(linesDrawn.Count-13);
			Destroy(goLastLine);
		}
	}


	public void DeleteLastLine14()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-14];

			linesDrawn.RemoveAt(linesDrawn.Count-14);
			Destroy(goLastLine);
		}
	}

	public void DeleteLastLine15()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-15];

			linesDrawn.RemoveAt(linesDrawn.Count-15);
			Destroy(goLastLine);
		}
	}

	public void DeleteLastLine16()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine = linesDrawn[linesDrawn.Count-16];

			linesDrawn.RemoveAt(linesDrawn.Count-16);
			Destroy(goLastLine);
		}
	}

	/*
	public void ResetSketch()
	{
		if (linesDrawn.Count > 0)
		{
			GameObject goLastLine18 = linesDrawn[linesDrawn.Count-18];
			GameObject goLastLine17 = linesDrawn[linesDrawn.Count-17];
			GameObject goLastLine16 = linesDrawn[linesDrawn.Count-16];
			GameObject goLastLine15 = linesDrawn[linesDrawn.Count-15];
			GameObject goLastLine14 = linesDrawn[linesDrawn.Count-14];
			GameObject goLastLine13 = linesDrawn[linesDrawn.Count-13];
			GameObject goLastLine12 = linesDrawn[linesDrawn.Count-12];
			GameObject goLastLine11 = linesDrawn[linesDrawn.Count-11];
			GameObject goLastLine10 = linesDrawn[linesDrawn.Count-10];

			GameObject goLastLine9 = linesDrawn[linesDrawn.Count-9];
			GameObject goLastLine8 = linesDrawn[linesDrawn.Count-8];
			GameObject goLastLine7 = linesDrawn[linesDrawn.Count-7];
			GameObject goLastLine6 = linesDrawn[linesDrawn.Count-6];
			GameObject goLastLine5 = linesDrawn[linesDrawn.Count-5];
			GameObject goLastLine4 = linesDrawn[linesDrawn.Count-4];
			GameObject goLastLine3 = linesDrawn[linesDrawn.Count-3];
			GameObject goLastLine2 = linesDrawn[linesDrawn.Count-2];
			GameObject goLastLine1 = linesDrawn[linesDrawn.Count-1];


			linesDrawn.RemoveAt(linesDrawn.Count-4);

			Destroy(goLastLine1);
			Destroy(goLastLine2);
			Destroy(goLastLine3);
			Destroy(goLastLine4);
			Destroy(goLastLine5);
			Destroy(goLastLine6);
			Destroy(goLastLine7);
			Destroy(goLastLine8);
			Destroy(goLastLine9);

			Destroy(goLastLine10);
			Destroy(goLastLine11);
			Destroy(goLastLine12);
			Destroy(goLastLine13);
			Destroy(goLastLine14);
			Destroy(goLastLine15);
			Destroy(goLastLine16);
			Destroy(goLastLine17);
			Destroy(goLastLine18);
		}
	}
	*/

	// continue drawing line
	IEnumerator DrawLine()
	{
		while(handOverlayer && (handOverlayer.GetLastHandEvent() == InteractionManager.HandEventType.Grip))
		{
			yield return new WaitForEndOfFrame();

			if (currentLine != null)
			{
				lineVertexIndex++;
				currentLine.SetVertexCount(lineVertexIndex);

				Vector3 cursorPos = handOverlayer.GetCursorPos();
				cursorPos.z = Camera.main.nearClipPlane;

				Vector3 cursorSpacePos = Camera.main.ViewportToWorldPoint(cursorPos);
				currentLine.SetPosition(lineVertexIndex - 1, cursorSpacePos);
			}
		}
	}

}
