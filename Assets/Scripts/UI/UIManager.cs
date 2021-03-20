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
    Text speechText;
    [SerializeField]
    Canvas PlacementUI;
    [SerializeField]
    Canvas ManipUI;
    [SerializeField]
    Text ScaleText;
    [SerializeField]
    Canvas AnnotationUI;
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
        // SpeechUI.gameObject.SetActive(!SpeechUI.gameObject.activeInHierarchy);
        if (placementManager.lastSelectedObject == null)
        {
            return;
        }
        else
        {
            SpeechUI.gameObject.SetActive(!SpeechUI.gameObject.activeInHierarchy);
        }
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

    private void UpdateManipUI(PlacementObject selectedObject)
    {
        ManipUI.GetComponentInChildren<Slider>().value = selectedObject.curScale;
    }

    public void ManipScale()
    {
        int offset = (int)ManipUI.GetComponentInChildren<Slider>().value;
        placementManager.ScaleSelectedObject(offset);
        ScaleText.text = string.Format("Scale:{0:0.0}", 1.0f + offset / 10f);
    }
    private void UpdateAnnotationUI(PlacementObject.Annotation anno)
    {
        AnnotationUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(anno.isReminderActive);
        AnnotationUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(anno.isScheduleActive);
    }

    public void UpdateUIAccordingToSelectedObject(PlacementObject selectedObject)
    {
        if (selectedObject == null)
        {
            UpdateAnnotationUI(new PlacementObject.Annotation());
            return;
        }
        UpdateManipUI(selectedObject);
        UpdateAnnotationUI(selectedObject.annotation);
    }

    // ----------------- Speech -----------------------------
    public void UpdateSpeechPanel(string result)
    {
        speechText.text = result;
    }
    // ----------------- Speech -----------------------------
}
