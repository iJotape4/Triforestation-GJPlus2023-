using UnityEngine;

public class SortingLayerModifier : MonoBehaviour
{
    private void Awake() =>
        GetComponent<SpriteRenderer>().sortingOrder = transform.parent.GetSiblingIndex();
}
