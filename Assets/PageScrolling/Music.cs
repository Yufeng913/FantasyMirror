using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource audio;
    private bool begin;
    private int clipNum = 0;

    // Use this for initialization
    void Start()
    {
        begin = false;
        audio = FindObjectOfType<AudioSource>();
        audio.loop = false;
        clipNum = 0;

        keywords.Add("play", () => {
            playCalled();
            begin = true;
        });

        keywords.Add("stop", () => {
            pauseCalled();
        });

        keywords.Add("next", () => {
            nextCalled();
        });

        keywords.Add("previous", () => {
            previousCalled();
        });

        keywords.Add("volume up", () => {
            VolumeUpCalled();
        });

        keywords.Add("volume down", () => {
            VolumeDownCalled();
        });

        KeywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        KeywordRecognizer.OnPhraseRecognized += KeywordRecognizerOnPhraseRecognized;
        KeywordRecognizer.Start();
    }

    // shuffle
    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }

    private AudioClip NextClip()
    {
        //Debug.Log(clips[1 + clipNum]);
        return clips[1 + clipNum++];
    }

    private AudioClip PreviousClip()
    {
        Debug.Log(clips[1 + clipNum]);
        return clips[1 + clipNum--];
    }

    void playCalled()
    {
        print("music playing");
        audio.clip = clips[clipNum];
        audio.Play();
    }

    void pauseCalled()
    {
        print("music paused");
        audio.Pause();
    }

    void nextCalled()
    {
        print("next song");
        audio.clip = NextClip();
        audio.Play();
    }

    void previousCalled()
    {
        print("previous song");
        audio.clip = PreviousClip();
        audio.Play();
    }

    void VolumeUpCalled()
    {
        print("volume up");
        audio.volume = 1.0f;
    }

    void VolumeDownCalled()
    {
        print("volume down");
        audio.volume = 0.2f;
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
        if (clipNum >= clips.Length)
        {
            clipNum = 0;
        }
    }


    KeywordRecognizer KeywordRecognizer;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();



}

