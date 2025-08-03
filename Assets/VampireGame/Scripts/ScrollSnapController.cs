using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScrollSnapController : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform content;
    [SerializeField] private float itemSpacing = 0f;
    [SerializeField] private float animationDuration = 0.3f;

    private int currentIndex = 0;
    private float itemWidth;
    private int totalItems;

    private void Start()
    {
        UpdateItemData();
    }

    private void UpdateItemData()
    {
        totalItems = content.childCount;

        if (totalItems > 0)
        {
            RectTransform firstItem = content.GetChild(0).GetComponent<RectTransform>();
            itemWidth = firstItem.rect.width + itemSpacing;
        }
    }

    public void ScrollNext()
    {
        if (currentIndex < totalItems - 1)
        {
            currentIndex++;
            ScrollToIndex(currentIndex);
        }
    }

    public void ScrollPrevious()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            ScrollToIndex(currentIndex);
        }
    }

    private void ScrollToIndex(int index)
    {
        float targetX = index * itemWidth;
        float maxScrollX = content.rect.width - scrollRect.viewport.rect.width;

        if (maxScrollX <= 0f)
        {
            scrollRect.horizontalNormalizedPosition = 0f;
            return;
        }

        float normalizedX = Mathf.Clamp01(targetX / maxScrollX);

        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
                   x => scrollRect.horizontalNormalizedPosition = x,
                   normalizedX,
                   animationDuration).SetEase(Ease.InOutSine);
    }

    public void Refresh()
    {
        UpdateItemData();
        currentIndex = Mathf.Clamp(currentIndex, 0, totalItems - 1);
        ScrollToIndex(currentIndex);
    }
}
