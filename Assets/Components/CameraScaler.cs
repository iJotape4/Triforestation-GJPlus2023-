using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    [SerializeField] private Collider2D logicalScene;

    private float targetScreenRatio;
    private float currentScreenRatio;

    private void Awake() =>
        targetScreenRatio = logicalScene.bounds.size.x / logicalScene.bounds.size.y;

    private void Update()
    {
        float screenRatio = ((float)Screen.width) / Screen.height;
        if (currentScreenRatio == screenRatio)
            return;
        currentScreenRatio = screenRatio;
        ScaleCameraToRatio();
    }

    private void ScaleCameraToRatio()
    {
        if (currentScreenRatio >= targetScreenRatio)
            Camera.main.orthographicSize = logicalScene.bounds.size.y * 0.5f;
        else
            Camera.main.orthographicSize = logicalScene.bounds.size.y * 0.5f * targetScreenRatio / currentScreenRatio;
    }
}
