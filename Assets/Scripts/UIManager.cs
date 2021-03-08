using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Canvas SpeechUI;
    [SerializeField]
    Canvas PlacementUI;
    [SerializeField]
    ARPlaneManager aRPlaneManager;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpeechUI.gameObject.SetActive(false);
        PlacementUI.gameObject.SetActive(false);
        aRPlaneManager.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchSpeechUI()
    {
        SpeechUI.gameObject.SetActive(!SpeechUI.gameObject.activeInHierarchy);
    }
    public void SwitchPlacementUI()
    {
        PlacementUI.gameObject.SetActive(!PlacementUI.gameObject.activeInHierarchy);
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach(ARPlane plane in aRPlaneManager.trackables) {
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
    }
}
