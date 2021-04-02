using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using HomeAR.Events;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementManager : MonoBehaviour
{
    public static PlacementManager Instance { get; private set; }
    [SerializeField]
    GameObject placementIndicator;
    [SerializeField]
    GameObject placePrefab;
    [SerializeField]
    PlacementObject[] placedObjects;
    public PlacementObject lastSelectedObject;
    [SerializeField]
    PlacementObjectEvent placementObjectEvent;
    [SerializeField]
    Color activeColor = Color.red;
    [SerializeField]
    Color inactiveColor = Color.gray;
    [SerializeField]
    Camera arCamera;
    ARRaycastManager aRRaycastManager;
    Vector2 touchPosition = default;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private Pose placementPose;
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
    Touch touch;
    bool manipCanvasOpening = false;

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
        aRRaycastManager = GetComponent<ARRaycastManager>();
        CubeButton.onClick.AddListener(() => ChangePrefabTo("Cube"));
        BallButton.onClick.AddListener(() => ChangePrefabTo("Ball"));
    }

    // Update is called once per frame
    void Update()
    {
        if (UIManager.Instance.isUIactive()) return;
        // Select in editor
#if UNITY_EDITOR
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
#endif

        // Update Placement Indicator
        aRRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        // if we hit an AR plane surface, update the position and rotation
        if (hits.Count > 0)
        {
            placementIndicator.transform.position = hits[0].pose.position;
            placementIndicator.transform.rotation = hits[0].pose.rotation;

            // enable the visual if it's disabled
            if (!placementIndicator.transform.GetChild(0).gameObject.activeInHierarchy) placementIndicator.transform.GetChild(0).gameObject.SetActive(true);
            placementPose = hits[0].pose;
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
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
                    else
                    {
                        // Clear selected object & dismiss Annocation UI
                        ClearSelection();
                        // uIManager.UpdateUIAccordingToSelectedObject();
                    }

                    // Place objects on AR plane
                    if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
                    {
                        var hitPose = hits[0].pose;
                        lastSelectedObject = Instantiate(placePrefab, placementPose.position, placementPose.rotation).GetComponent<PlacementObject>();
                        // placedObjects = FindObjectsOfType<PlacementObject>();
                        //ChangeSelectedObject(lastSelectedObject);
                        // lastSelectedObject.oriScale = lastSelectedObject.transform.localScale;
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
                    if (aRRaycastManager.Raycast(touchPosition, hits, TrackableType.Planes))
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
        } while ((touch.phase == TouchPhase.Stationary || (touch.phase == TouchPhase.Moved && touch.deltaPosition.magnitude < 0.5f)) && i < 5);
        if (i == 5)
        {
            UIManager.Instance.SwitchManipUI();
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
                placementObjectEvent.Raise(cur);
            }
        }
        // uIManager.UpdateUIAccordingToSelectedObject();
    }

    void ClearSelection()
    {
        if (lastSelectedObject != null)
        {
            lastSelectedObject.IsSelected = false;
            lastSelectedObject.GetComponent<Outline>().enabled = false;
        }
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
