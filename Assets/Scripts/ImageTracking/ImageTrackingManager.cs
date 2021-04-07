using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTrackingManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] placeablePrefabs;
    Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
    ARTrackedImageManager aRTrackedImageManager;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        aRTrackedImageManager = GetComponent<ARTrackedImageManager>();
        // foreach (GameObject prefab in placeablePrefabs)
        // {
        //     GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        //     newPrefab.name = prefab.name;
        //     spawnedPrefabs.Add(prefab.name, newPrefab);
        //     prefab.SetActive(false);
        // }
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        aRTrackedImageManager.trackedImagesChanged += ImageChanged;
    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        aRTrackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    void ImageChanged(ARTrackedImagesChangedEventArgs evetnArgs)
    {
        foreach (ARTrackedImage trackedImage in evetnArgs.added)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in evetnArgs.updated)
        {
            UpdateImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in evetnArgs.removed)
        {
            spawnedPrefabs[trackedImage.name].SetActive(false);
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        Vector3 position = trackedImage.transform.position;

        GameObject prefab = spawnedPrefabs[name];
        prefab.transform.position = position;
        prefab.SetActive(true);

        foreach (GameObject go in spawnedPrefabs.Values)
        {
            if(go.name != name) {
                go.SetActive(false);
            }
        }
    }
}
