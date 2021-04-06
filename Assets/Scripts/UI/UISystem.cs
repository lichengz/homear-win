﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UISystem : MonoBehaviour
{
    public static UISystem Instance { get; private set; }
    // Swipe & Object Panel
    Vector2 startTouchPos, endTouchPos;
    bool swipeUpDetected;
    int swipeThreshold = 300;
    [SerializeField]
    RectTransform mainPanel, objectPanel;
    [SerializeField] Transform objectSlotContainer;
    [SerializeField] GameObject objectSlotPrefab;
    float animationDur = 0.25f;
    float screenWidth;
    float screenHeight;
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
    }

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
        foreach (Object placementObject in PlacementManager.Instance.inventory.objectList)
        {
            generateObjectSlotPrefab(placementObject).transform.SetParent(objectSlotContainer);
        }
    }

    GameObject generateObjectSlotPrefab(Object placementObject)
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
}
