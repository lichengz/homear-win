using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public PlacementObject lastSelectedObject;
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
    [SerializeField]
    UIManager uIManager;
    Touch touch;
    bool manipCanvasOpening = false;

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
        if (uIManager.isUIactive()) return;
        // Select in editor
        Ray r = arCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit o;
        if (Physics.Raycast(r, out o))
        {
            PlacementObject placementObject = o.transform.GetComponent<PlacementObject>();
            if (placementObject != null)
            {
                ChangeSelectedObject(placementObject);
            }
        }

        // Touch Screen

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = touch.position;
            if (IsOnGUi(touchPosition)) return;
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
                        // placedObjects = FindObjectsOfType<PlacementObject>();
                        ChangeSelectedObject(lastSelectedObject);
                        lastSelectedObject.oriScale = lastSelectedObject.transform.localScale;
                        Debug.Log("Place instantiated");
                    }
                    break;
                case TouchPhase.Stationary:
                    // Open ManipCanvas
                    if (manipCanvasOpening) break;
                    RaycastHit ho;
                    if (Physics.Raycast(arCamera.ScreenPointToRay(touchPosition), out ho))
                    {
                        PlacementObject placementObject = ho.transform.GetComponent<PlacementObject>();
                        if (placementObject != null && placementObject == lastSelectedObject)
                        {
                            StartCoroutine(ManipUICountdown());
                            manipCanvasOpening = true;
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (manipCanvasOpening) break;
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

    bool IsOnGUi(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return false;
        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);
        return results.Count > 0;
    }

    private IEnumerator ManipUICountdown()
    {
        int i = 0;
        do
        {
            yield return new WaitForSeconds(0.1f);
            i++;
            Debug.Log(i);
        } while ((touch.phase == TouchPhase.Stationary || (touch.phase == TouchPhase.Moved && touch.deltaPosition.magnitude < 0.5f)) && i < 5);
        if (i == 5)
        {
            uIManager.SwitchManipUI();
        }
        manipCanvasOpening = false;
    }

    void ChangeSelectedObject(PlacementObject selected)
    {
        placedObjects = FindObjectsOfType<PlacementObject>();
        foreach (PlacementObject cur in placedObjects)
        {
            MeshRenderer meshRenderer = cur.GetComponent<MeshRenderer>();
            if (selected != cur)
            {
                cur.IsSelected = false;
                // meshRenderer.material.color = inactiveColor;
                cur.GetComponent<Outline>().enabled = false;
            }
            else
            {
                lastSelectedObject = cur;
                cur.IsSelected = true;
                // meshRenderer.material.color = activeColor;
                cur.GetComponent<Outline>().enabled = true;
            }
        }
        uIManager.UpdateUIAccordingToSelectedObject();
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

    public void ScaleSelectedObject(int scaling)
    {
        if (lastSelectedObject == null) return;
        lastSelectedObject.curScale = scaling;
        lastSelectedObject.transform.localScale = lastSelectedObject.oriScale * (1 + scaling / 10f);
    }
}
