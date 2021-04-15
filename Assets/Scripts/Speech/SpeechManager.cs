using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;
using HomeAR.Events;

public class SpeechManager : MonoBehaviour
{
    public static SpeechManager Instance { get; private set; }
    const string LANG_CODE = "en-US";
    // [SerializeField]
    // Text speechText;
    AudioClip myAudioClip;
    // [SerializeField]
    // PlacementManager placementManager;
    [SerializeField]
    public StringEvent onFinalSpeechResult;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        Setup(LANG_CODE);
#if UNITY_ANDROID
        SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
#endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;

        Checkpermission();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F1))
        {
            onFinalSpeechResult.Raise("Broadcasting NO.1...");
            Debug.Log("Sent speech msg.");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            onFinalSpeechResult.Raise("Broadcasting NO.2...");
            Debug.Log("Sent speech msg.");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            onFinalSpeechResult.Raise("Broadcasting NO.3..");
            Debug.Log("Sent speech msg.");
        }
#endif
    }

    void Checkpermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
#endif
    }

    #region Text to Speech
    public void StartSpeaking(string msg)
    {
        TextToSpeech.instance.StartSpeak(msg);
    }
    public void StopSpeaking()
    {
        TextToSpeech.instance.StopSpeak();
    }
    void OnSpeakStart()
    {
        Debug.Log("Speaking started...");
    }
    void OnSpeakStop()
    {
        Debug.Log("Speaking stopped...");
    }
    #endregion

    #region Speech to Text
    public void StartListening()
    {
        //myAudioClip =  Microphone.Start(null, false, 100, 44100);
        SpeechToText.instance.StartRecording();
        Debug.Log("Listening started...");
    }
    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
        int lastTime = Microphone.GetPosition(null);
        //SavWav.Save("myfile", SavWav.TrimSilence(myAudioClip, (float)lastTime / 60f));
        Debug.Log("Listening stopped..." + lastTime + " seconds");
    }
    void OnFinalSpeechResult(string result)
    {
        onFinalSpeechResult.Raise(result);
        // speechText.text = result;
        // if (placementManager.lastSelectedObject != null)
        // {
        //     placementManager.lastSelectedObject.annotation.reminderText = result;
        // }
    }
    void OnPartialSpeechResult(string result)
    {
        onFinalSpeechResult.Raise(result);
        // speechText.text = result;
    }
    #endregion

    void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
        TextToSpeech.instance.Setting(code, 1, 1);
    }
}
