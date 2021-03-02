using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TextSpeech;
using UnityEngine.UI;
using UnityEngine.Android;

public class SpeechManager : MonoBehaviour
{
    const string LANG_CODE = "en-US";
    [SerializeField]
    Text speechText;
    AudioClip myAudioClip;
    void Start() {
        Setup(LANG_CODE);
        #if UNITY_ANDROID
            SpeechToText.instance.onPartialResultsCallback = OnPartialSpeechResult;
        #endif
        SpeechToText.instance.onResultCallback = OnFinalSpeechResult;
        TextToSpeech.instance.onStartCallBack = OnSpeakStart;
        TextToSpeech.instance.onDoneCallback = OnSpeakStop;

        Checkpermission();
    }
    void Checkpermission() {
        #if UNITY_ANDROID
            if(!Permission.HasUserAuthorizedPermission(Permission.Microphone)) {
                Permission.RequestUserPermission(Permission.Microphone);
            }
        #endif
    }

    #region Text to Speech
    public void StartSpeaking(string msg) {
        TextToSpeech.instance.StartSpeak(msg);
    }
    public void StopSpeaking() {
        TextToSpeech.instance.StopSpeak();
    }
    void OnSpeakStart() {
        Debug.Log("Speaking started...");
    }
    void OnSpeakStop() {
        Debug.Log("Speaking stopped...");
    }
    #endregion

    #region Speech to Text
    public void StartListening() {
        //myAudioClip =  Microphone.Start(null, false, 100, 44100);
        SpeechToText.instance.StartRecording();
        Debug.Log("Listening started...");
    }
    public void StopListening() {
        SpeechToText.instance.StopRecording();
        int lastTime = Microphone.GetPosition(null);
        //SavWav.Save("myfile", SavWav.TrimSilence(myAudioClip, (float)lastTime / 60f));
        Debug.Log("Listening stopped..." + lastTime + " seconds");
    }
    void OnFinalSpeechResult(string result) {
        speechText.text = result;
    }
    void OnPartialSpeechResult(string result) {
        speechText.text = result;
    }
    #endregion
    
    void Setup(string code) {
        SpeechToText.instance.Setting(code);
        TextToSpeech.instance.Setting(code, 1, 1);
    }
}
