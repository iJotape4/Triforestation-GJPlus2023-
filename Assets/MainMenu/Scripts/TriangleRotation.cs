using DG.Tweening;
using UnityEngine;

public class TriangleRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DORotate(transform.rotation.eulerAngles - new Vector3(0, 0, 180), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

}