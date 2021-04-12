using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
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
    Text reminderText;
    [SerializeField]
    ARPlaneManager aRPlaneManager;
    [SerializeField]
    GameObject scheduleSlotPrefab;
    [SerializeField]
    Transform scheduleSlotContainer;
    [SerializeField]
    GameObject SchedulePanel;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
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
        SpeechUI.gameObject.SetActive(false);
        PlacementUI.gameObject.SetActive(false);
        ManipUI.gameObject.SetActive(false);
        // aRPlaneManager.enabled = false;
        UpdateAnnotationUI(new PlacementObject.Annotation());
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
        if (PlacementManager.Instance.lastSelectedObject == null)
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
        // aRPlaneManager.enabled = !aRPlaneManager.enabled;
        // foreach (ARPlane plane in aRPlaneManager.trackables)
        // {
        //     plane.gameObject.SetActive(aRPlaneManager.enabled);
        // }
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
        PlacementManager.Instance.ScaleSelectedObject(offset);
        ScaleText.text = string.Format("Scale:{0:0.0}", 1.0f + offset / 10f);
    }
    // ----------------- Annotation -----------------------------
    public void UpdateAnnotationUI(PlacementObject.Annotation anno)
    {
        AnnotationUI.transform.GetChild(0).GetChild(0).gameObject.SetActive(anno.isReminderActive);
        AnnotationUI.transform.GetChild(0).GetChild(1).gameObject.SetActive(anno.isScheduleActive);
        //reminderText.text = anno.reminder.dict[UserManager.curUser].reminders[0];
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
    // ----------------- Annotation -----------------------------

    // ----------------- Speech -----------------------------
    public void UpdateSpeechPanel(string result)
    {
        speechText.text = result;
    }
    // ----------------- Speech -----------------------------

    // ----------------- Schedule -----------------------------
    public void InitSchedulePanel()
    {
        if (SchedulePanel.activeInHierarchy)
        {
            return;
        }
        foreach (Transform child in scheduleSlotContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
        SchedulePanel.SetActive(true);
        Schedule scheduleOfSelectedObject = PlacementManager.Instance.lastSelectedObject.annotation.schedule;
        PlacementManager.Instance.lastSelectedObject.UpdateAnnotaionStatus();
        for (int i = 0; i < UserManager.numOfScheduleSlots; i++)
        {
            GameObject slot = Instantiate(scheduleSlotPrefab, Vector3.zero, Quaternion.identity);
            int tmp = i;
            slot.GetComponent<Button>().onClick.AddListener(() => InsertScheduleSlot(tmp));
            slot.transform.SetParent(scheduleSlotContainer);
            if (scheduleOfSelectedObject.slots[i].index == -1)
            {
                slot.GetComponentInChildren<Text>().text = "Empty";
            }
            else
            {
                slot.GetComponentInChildren<Text>().text = scheduleOfSelectedObject.slots[i].name;
            }
        }
    }

    public void UpdateSchedulePanel()
    {
        Schedule scheduleOfSelectedObject = PlacementManager.Instance.lastSelectedObject.annotation.schedule;
        PlacementManager.Instance.lastSelectedObject.UpdateAnnotaionStatus();
        for (int i = 0; i < UserManager.numOfScheduleSlots; i++)
        {
            GameObject slot = scheduleSlotContainer.GetChild(i).gameObject;
            if (scheduleOfSelectedObject.slots[i].index == -1)
            {
                slot.GetComponentInChildren<Text>().text = "Empty";
            }
            else
            {
                slot.GetComponentInChildren<Text>().text = scheduleOfSelectedObject.slots[i].name;
            }
        }
    }

    public void InsertScheduleSlot(int index)
    {
        Debug.Log("Trying to insert at slot " + index);
        PlacementManager.Instance.lastSelectedObject.annotation.schedule.InsertSlot(index);
        PlacementManager.Instance.lastSelectedObject.UpdateAnnotaionStatus();
        UpdateSchedulePanel();
    }
    // ----------------- Schedule -----------------------------
}
