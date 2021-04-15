using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRotationFollower : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform;
    RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.rotation = Quaternion.Inverse(targetTransform.rotation);
    }
}
