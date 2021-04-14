using UnityEngine;
using UnityEngine.UI;

public class WorldPositionButton : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform;
    RectTransform rectTransform;
    Image image;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (GetComponent<Image>()) image = GetComponent<Image>();
    }
    void Update()
    {
        var screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
        rectTransform.position = screenPoint;

        var viewportPoint = Camera.main.WorldToViewportPoint(targetTransform.position);
        var distanceFromCenter = Vector2.Distance(viewportPoint, Vector2.one * 0.5f);

        var show = distanceFromCenter < 0.5f;

        if (image) image.enabled = show;
    }
}
