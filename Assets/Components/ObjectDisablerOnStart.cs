using UnityEngine;

public class ObjectDisablerOnStart : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }
}
