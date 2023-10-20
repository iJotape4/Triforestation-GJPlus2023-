using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FadeLoader : MonoBehaviour
{
    Image image;
    [SerializeField] Color targetColor = new Color(1f, 1f, 1f, 0f);
    [SerializeField,Range(0.1f, 10f)] float fadeDuration = 5f;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.DOColor(targetColor, fadeDuration).SetEase(Ease.InExpo);
    } 
}