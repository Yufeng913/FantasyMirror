using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class speechAnimate : MonoBehaviour
{

    private Events messages;
    private string textMessage;
    private bool isRunning = false;



    // Use this for initialization
    void Start()
    {
       
     


    }

    private void autoTypeRun()
    {
        if (isRunning == false)
        {
            StartCoroutine(AutoType());
        }
        
    }

    private void Update()
    {
        messages = Events.FindObjectOfType<Events>();
        textMessage = messages.speechMessage;

        if (messages.showText == true)
        {
            autoTypeRun();
        }
        if (messages.deleteText == true)
        {
            GameObject.Find("Message").GetComponent<GUIText>().text = ""; //clear text
            messages.deleteText = false; 
        }
    
    }


    // Update is called once per frame
    private IEnumerator AutoType()
    {
        foreach (char letter in textMessage.ToCharArray())
        {
            isRunning = true;
            float textSpeed = 0.03f;

            GetComponent<GUIText>().text += letter; //for unity 5x
                                                    // guiText.text += letter;  // for unity 4x
            yield return new WaitForSeconds(textSpeed);
            isRunning = false;
            messages.showText = false;
        }
    }

}