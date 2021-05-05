using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    // [SerializeField]
    // public Transform targetTransform;
    [SerializeField]
    Text countText;
    [SerializeField]
    bool left;
    float animationDur = 0.25f;

    // Update is called once per frame
    void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(CalculateCenterPosition());

        int count = GetNumOfObjectOutsideVision();
        countText.text = count.ToString();

        if (count == 0)
        {
            DismissBubble();
        }
        else
        {
            DisplayBubble();
        }
        transform.position = new Vector3(transform.position.x, screenPoint.y, transform.position.z);
    }
    public void DisplayBubble()
    {

        transform.DOScale(Vector3.one, animationDur);
    }

    public void DismissBubble()
    {
        transform.DOScale(Vector3.zero, animationDur);
    }

    private int GetNumOfObjectOutsideVision()
    {
        int count = 0;
        foreach (PlacementObject po in PlacementManager.Instance.placedObjects)
        {
            if (!Within(po.gameObject.transform))
            {
                var viewportPoint = Camera.main.WorldToViewportPoint(po.gameObject.transform.position);
                if ((left && viewportPoint.x < 0) || (!left && viewportPoint.x > 1)) count++;
            }
        }
        return count;
    }

    private bool Within(Transform targetTransform)
    {
        var viewportPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
        var distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);
        var within = distanceFromCenter < 0.5f;
        return within;
    }

    private Vector3 CalculateCenterPosition()
    {
        int n = PlacementManager.Instance.placedObjects.Length;
        Vector3 sumPos = Vector3.zero;
        foreach (PlacementObject po in PlacementManager.Instance.placedObjects)
        {
            sumPos += po.gameObject.transform.position;
        }
        return sumPos / n;
    }
}
