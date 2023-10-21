using UnityEngine;

public class ObjectScalerInBounds : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D collider;

    private Vector3 initialScale;

    private void Awake() =>
        initialScale = transform.localScale;

    public void ScaleSpriteToBounds()
    {
        if (spriteRenderer.sprite == null)
            return;
        float minColliderSize = collider.size.x >= collider.size.y ? collider.size.y : collider.size.x;
        Vector3 spriteScaled = Vector3.Scale(spriteRenderer.localBounds.size, initialScale);
        float maxSpriteSize = spriteScaled.x >= spriteScaled.y ? spriteScaled.x : spriteScaled.y;
        float targetScale =  minColliderSize / maxSpriteSize;
        transform.localScale = initialScale * targetScale;
    }
}
