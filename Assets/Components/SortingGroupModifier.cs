using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class SortingGroupModifier : MonoBehaviour
{
    private void Awake() =>
        GetComponent<SortingGroup>().sortingOrder = transform.parent.GetSiblingIndex();
}
