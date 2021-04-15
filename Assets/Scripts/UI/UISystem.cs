using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UISystem : MonoBehaviour
{
    public static UISystem Instance { get; private set; }
    [SerializeField] UserPool userPool;
    // Swipe & Object Panel
    Vector2 startTouchPos, endTouchPos;
    bool swipeUpDetected;
    int swipeThreshold = 300;
    [SerializeField]
    RectTransform mainPanel, objectPanel;
    [SerializeField] Transform objectSlotContainer;
    [SerializeField] GameObject objectSlotPrefab;
    [SerializeField] Transform ARNoteBody;
    int ARNoteIndex = 0;
    float animationDur = 0.25f;
    float screenWidth;
    float screenHeight;

    // Annotation
    bool isAnnotationMenuOpen;
    [SerializeField] RectTransform annotationMenu;
    [SerializeField] GameObject ARNote;
    [SerializeField] GameObject ARNoteModel;
    [SerializeField] Material leftArrow;
    [SerializeField] Material rightArrow;
    [SerializeField] TextMeshPro userTMPro;
    [SerializeField] TextMeshPro noteTMPro;
    [SerializeField] GameObject noteSlotPrefab;

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
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        mainPanel.anchoredPosition = new Vector2(-screenWidth, 0);
        objectPanel.anchoredPosition = new Vector2(0, -screenHeight);
    }
    // Start is called before the first frame update
    void Start()
    {
        InitObjectPanel();
        mainPanel.DOAnchorPos(Vector2.zero, animationDur);
        InitARNote();
    }

    // Update is called once per frame
    void Update()
    {
        //Swipe Check
        SwipeCheck();
        if (swipeUpDetected)
        {
            swipeUpDetected = false;
            ShowObjectPanel();
        }

        UpdateARNoteRotation();
        SwitchUser();
    }

    public void SwitchUser()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UserManager.curUser = userPool.userList[0];
            Debug.Log("Logged in as user1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UserManager.curUser = userPool.userList[1];
            Debug.Log("Logged in as user2");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UserManager.curUser = userPool.userList[2];
            Debug.Log("Logged in as user3");
        }
    }

    public bool isUIactive()
    {
        if (ARNote.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    // ----------------- Hack -----------------------------
    public void ARNoteHack()
    {
        UserManager.curUser = userPool.userList[0];
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.1...");
        UserManager.curUser = userPool.userList[1];
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.1...");
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.2...");
        UserManager.curUser = userPool.userList[2];
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.1...");
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.2...");
        SpeechManager.Instance.onFinalSpeechResult.Raise("Broadcasting NO.3...");
    }
    // ----------------- Hack -----------------------------

    // ----------------- Object Panel -----------------------------
    void SwipeCheck()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            endTouchPos = Input.mousePosition;
            if (endTouchPos.y - startTouchPos.y > swipeThreshold)
            {
                swipeUpDetected = true;
            }
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPos = Input.GetTouch(0).position;
            if (endTouchPos.y - startTouchPos.y > swipeThreshold)
            {
                swipeUpDetected = true;
            }
        }
#endif
    }

    public void ShowObjectPanel()
    {
        mainPanel.DOAnchorPos(new Vector2(-screenWidth, 0), animationDur);
        objectPanel.DOAnchorPos(Vector2.zero - new Vector2(0, Screen.height * 0.1f), animationDur);
    }

    public void DismissObjectPanel()
    {
        mainPanel.DOAnchorPos(Vector2.zero, animationDur);
        objectPanel.DOAnchorPos(new Vector2(0, -Screen.height), animationDur);
    }

    public void InitObjectPanel()
    {
        foreach (Transform child in objectSlotContainer)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (PlacedObject placementObject in PlacementManager.Instance.inventory.objectList)
        {
            generateObjectSlotPrefab(placementObject).transform.SetParent(objectSlotContainer);
        }
    }

    GameObject generateObjectSlotPrefab(PlacedObject placementObject)
    {
        GameObject go = Instantiate(objectSlotPrefab, Vector3.zero, Quaternion.identity);
        if (placementObject.prefab != null)
        {
            go.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>().sprite = placementObject.icon;
            go.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = placementObject.text;
            go.GetComponent<Button>().onClick.AddListener(() => PlacementManager.Instance.ChangePrefabTo(placementObject.text));
        }
        return go;
    }
    // ----------------- Object Panel -----------------------------

    // ----------------- Annotation Panel -----------------------------
    public void switchAnnotationMenu()
    {
        if (isAnnotationMenuOpen)
        {
            annotationMenu.DOAnchorPos(new Vector2(0, -500), animationDur);
            isAnnotationMenuOpen = false;
            if (ARNote.activeInHierarchy) StartCoroutine(DismissARNote());
        }
        else
        {
            annotationMenu.DOAnchorPos(Vector2.zero, animationDur);
            isAnnotationMenuOpen = true;
        }

    }

    public void SwitchARNote()
    {
        if (!ARNote.activeInHierarchy)
        {
            StartCoroutine(ShowARNote());
        }
        else
        {
            StartCoroutine(DismissARNote());
        }

    }
    private IEnumerator ShowARNote()
    {
        ARNote.SetActive(true);
        ARNoteBody.gameObject.SetActive(true);
        ARNote.transform.DOScale(new Vector3(1, 1, 1), animationDur);
        yield return new WaitForSeconds(animationDur);
        UpdateARNote();
    }

    private IEnumerator DismissARNote()
    {
        ARNoteBody.gameObject.SetActive(false);
        ARNote.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), animationDur);
        yield return new WaitForSeconds(animationDur);
        ARNote.SetActive(false);
        foreach (Transform child in ARNoteBody)
        {
            Destroy(child.gameObject);
        }
    }

    public void UpdateARNoteRotation()
    {
        // var cameraForward = Camera.main.transform.forward;
        // var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        // ARNote.transform.rotation = Quaternion.LookRotation(cameraBearing);
        if (PlacementManager.Instance.lastSelectedObject != null)
        {
            ARNote.transform.position = PlacementManager.Instance.lastSelectedObject.transform.position + new Vector3(0, 0.5f, 0);
            ARNote.transform.LookAt(Camera.main.transform);
            ARNote.transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
    }

    private void InitARNote()
    {
        ARNote.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        ARNote.SetActive(false);
    }

    public void UpdateARNoteIndex(bool increase)
    {
        int cap = PlacementManager.Instance.lastSelectedObject.annotation.reminder.dict.Count;
        if (increase)
        {
            if (ARNoteIndex < cap - 1) ARNoteIndex++;
            UpdateARNote();
        }
        else
        {
            if (ARNoteIndex > 0) ARNoteIndex--;
            UpdateARNote();
        }
    }

    public void UpdateARNote()
    {
        int cap = PlacementManager.Instance.lastSelectedObject.annotation.reminder.dict.Count;
        if (cap == 0) return;
        IEnumerator enumerator = PlacementManager.Instance.lastSelectedObject.annotation.reminder.dict.Keys.GetEnumerator();
        for (int i = 0; i <= ARNoteIndex; i++)
        {
            enumerator.MoveNext();
        }
        User cur = (User)enumerator.Current;
        userTMPro.text = cur.name;
        foreach (Transform child in ARNoteBody)
        {
            Destroy(child.gameObject);
        }
        foreach (string str in PlacementManager.Instance.lastSelectedObject.annotation.reminder.dict[cur])
        {
            GameObject noteSlot = Instantiate(noteSlotPrefab, Vector3.zero, Quaternion.identity);
            noteSlot.transform.SetParent(ARNoteBody);
            noteSlot.transform.localPosition = new Vector3(noteSlot.transform.position.x, noteSlot.transform.position.y, -0.1f);
             noteSlot.transform.localRotation = ARNoteBody.transform.localRotation;
            // noteSlot.GetComponent<Text>().text = str;
            noteSlot.GetComponent<TextMeshPro>().text = str;
        }
        leftArrow.SetColor("_TintColor", ARNoteIndex == 0 ? Color.grey : Color.black);
        rightArrow.SetColor("_TintColor", ARNoteIndex == cap - 1 ? Color.grey : Color.black);
    }
    // ----------------- Annotation Panel -----------------------------
}
