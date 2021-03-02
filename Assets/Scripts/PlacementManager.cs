using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlacementManager : MonoBehaviour
{
    [SerializeField]
    GameObject placePrefab;
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

    ARRaycastManager aRRaycastManager;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPos)
    {
        if (Input.touchCount > 0)
        {
            Debug.Log("Found Touch");
            touchPos = Input.GetTouch(0).position;
            return true;
        }
        touchPos = default;
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPos))
        {
            return;
        }
        if (aRRaycastManager.Raycast(touchPos, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;
            Instantiate(placePrefab, hitPose.position, hitPose.rotation);
            Debug.Log("Place instantiated");
        }
    }

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
}
