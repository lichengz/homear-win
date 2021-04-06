using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Vector2 pos = Vector2.zero - new Vector2(0, Screen.height * 0.1f);
    float percentThreshold = 0.2f;
    public void OnDrag(PointerEventData data)
    {
        float diff = data.pressPosition.y - data.position.y;
        if (pos.y - diff <= 0) transform.GetComponent<RectTransform>().anchoredPosition = pos - new Vector2(0, diff);
    }
    public void OnEndDrag(PointerEventData data)
    {
        float percent = (data.pressPosition.y - data.position.y) / Screen.height;
        if (percent >= percentThreshold)
        {
            UISystem.Instance.DismissObjectPanel();
        }
        else
        {
            // transform.GetComponent<RectTransform>().anchoredPosition = pos;
            transform.GetComponent<RectTransform>().DOAnchorPos(pos, 0.1f);
        }
    }
}
