using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageResizeInBounds : MonoBehaviour
{
    private Image image;
    private RectTransform rectTransform;
    private Vector2 initialSize;

    private bool initialized;

    public void ResizeImageToBounds()
    {
        if (!initialized)
        {
            image = GetComponent<Image>();
            rectTransform = GetComponent<RectTransform>();
            initialSize = GetComponent<RectTransform>().sizeDelta;
            initialized = true;
        }

        if (image.sprite == null)
            return;

        bool maxImageSizeIsX = image.sprite.bounds.size.x >= image.sprite.bounds.size.y;
        float maxImageSize = maxImageSizeIsX ? image.sprite.bounds.size.x : image.sprite.bounds.size.y;
        float minImageSize = maxImageSizeIsX ? image.sprite.bounds.size.y : image.sprite.bounds.size.x;
        float minInitialSize = initialSize.x > initialSize.y ? initialSize.x : initialSize.y;
        float minImageSizeTransformation = minImageSize * minInitialSize / maxImageSize;
        rectTransform.sizeDelta = maxImageSizeIsX ? new Vector2(minInitialSize, minImageSizeTransformation) : new Vector2(minImageSizeTransformation, minInitialSize);
    }
}
