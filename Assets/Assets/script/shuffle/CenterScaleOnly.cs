using UnityEngine;
using UnityEngine.UI;

public class CenterScaleOnly : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;

    public float minScale = 0.8f;
    public float maxScale = 1.4f;
    public float scaleSpeed = 10f;

    void Update()
    {
        RectTransform viewport = scrollRect.viewport;

        Vector3 worldCenter = viewport.TransformPoint(
            new Vector3(viewport.rect.width / 2f, viewport.rect.height / 2f, 0));

        float closestDistance = float.MaxValue;
        RectTransform closestItem = null;

        foreach (RectTransform child in content)
        {
            float distance = Mathf.Abs(child.position.x - worldCenter.x);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestItem = child;
            }
        }

        foreach (RectTransform child in content)
        {
            float targetScale = (child == closestItem) ? maxScale : minScale;

            child.localScale = Vector3.Lerp(
                child.localScale,
                Vector3.one * targetScale,
                Time.deltaTime * scaleSpeed
            );
        }
    }
}