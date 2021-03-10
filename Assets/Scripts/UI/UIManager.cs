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
    Canvas ManipUI;
    [SerializeField]
    Text ScaleText;
    [SerializeField]
    ARPlaneManager aRPlaneManager;
    [SerializeField]
    PlacementManager placementManager;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        SpeechUI.gameObject.SetActive(false);
        PlacementUI.gameObject.SetActive(false);
        ManipUI.gameObject.SetActive(false);
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

    public bool isUIactive()
    {
        if (ManipUI.gameObject.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    public void SwitchSpeechUI()
    {
        SpeechUI.gameObject.SetActive(!SpeechUI.gameObject.activeInHierarchy);
    }
    public void SwitchPlacementUI()
    {
        PlacementUI.gameObject.SetActive(!PlacementUI.gameObject.activeInHierarchy);
        aRPlaneManager.enabled = !aRPlaneManager.enabled;
        foreach (ARPlane plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(aRPlaneManager.enabled);
        }
    }
    public void SwitchManipUI()
    {
        ManipUI.gameObject.SetActive(!ManipUI.gameObject.activeInHierarchy);
    }

    public void UpdateManipUI()
    {
        ManipUI.GetComponentInChildren<Slider>().value = placementManager.lastSelectedObject.curScale;
    }

    public void ManipScale()
    {
        int offset = (int)ManipUI.GetComponentInChildren<Slider>().value;
        placementManager.ScaleSelectedObject(offset);
        ScaleText.text = string.Format("Scale:{0:0.0}", 1.0f + offset / 10f);
    }
}
