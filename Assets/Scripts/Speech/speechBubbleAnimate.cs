using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speechBubbleAnimate : MonoBehaviour {

    private Animator animator;
    private Events startMessage;

    // Use this for initialization
    void Start () {

        animator = GetComponent<Animator>();

        startMessage = Events.FindObjectOfType<Events>();
    }
	
	// Update is called once per frame
	void Update () {

        if (startMessage.startBubbleAnimate == true)
        {
            animator.SetBool("speech", true);
        }

        if (startMessage.startBubbleAnimate == false)
        {
            animator.SetBool("speech", false);
        }

        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("speech", true);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetBool("speech", false);
        }
        


    }
}
