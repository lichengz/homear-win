using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    GameObject placePrefab;
    [SerializeField]
    PlacementObject[] placedObjects;
    PlacementObject lastSelectedObject;
    [SerializeField]
    Color activeColor = Color.red;
    [SerializeField]
    Color inactiveColor = Color.gray;
    [SerializeField]
    Camera arCamera;
    ARRaycastManager aRRaycastManager;
    Vector2 touchPosition = default;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject PlacedPrefab
    {
        get
        {
            return placePrefab;
        }
        set
        {
            placePrefab = value;
        }
    }

    [SerializeField]
    Button CubeButton;
    [SerializeField]
    Button BallButton;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        CubeButton.onClick.AddListener(() => ChangePrefabTo("Cube"));
        BallButton.onClick.AddListener(() => ChangePrefabTo("Ball"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchPosition = touch.position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    // Select objects
                    Ray ray = arCamera.ScreenPointToRay(touchPosition);
                    RaycastHit hitObject;
                    if (Physics.Raycast(ray, out hitObject))
                    {
                        PlacementObject placementObject = hitObject.transform.GetComponent<PlacementObject>();
                        if (placementObject != null)
                        {
                            ChangeSelectedObject(placementObject);
                            // No need to create new object, since I'm just selecting existing one.
                            break;
                        }
                    }

                    // Place objects on AR plane
                    if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
                    {
                        var hitPose = hits[0].pose;
                        lastSelectedObject = Instantiate(placePrefab, hitPose.position, hitPose.rotation).GetComponent<PlacementObject>();
                        placedObjects = FindObjectsOfType<PlacementObject>();
                        ChangeSelectedObject(lastSelectedObject);
                        Debug.Log("Place instantiated");
                    }
                    break;

                case TouchPhase.Moved:
                    if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
                    {
                        // Dragging
                        var hitPose = hits[0].pose;
                        if (lastSelectedObject != null && lastSelectedObject.IsSelected)
                        {
                            Debug.Log("DRAG");
                            lastSelectedObject.transform.position = hitPose.position;
                            lastSelectedObject.transform.rotation = hitPose.rotation;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    //ClearSelection();
                    break;
            }


        }

    }

    void ChangeSelectedObject(PlacementObject selected)
    {
        foreach (PlacementObject cur in placedObjects)
        {
            MeshRenderer meshRenderer = cur.GetComponent<MeshRenderer>();
            if (selected != cur)
            {
                cur.IsSelected = false;
                meshRenderer.material.color = inactiveColor;
            }
            else
            {
                lastSelectedObject = cur;
                cur.IsSelected = true;
                meshRenderer.material.color = activeColor;
            }
        }
    }

    void ClearSelection()
    {
        if (lastSelectedObject != null) lastSelectedObject.IsSelected = false;
        lastSelectedObject = null;
    }

    public void ChangePrefabTo(string prefabName)
    {
        placePrefab = Resources.Load<GameObject>($"Prefabs/{prefabName}");
        if (placePrefab == null) Debug.Log("Invalid prefabName");
    }
}
