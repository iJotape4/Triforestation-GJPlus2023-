using UnityEngine;
public class MobileUIManager : MonoBehaviour
{
#if !UNITY_ANDROID
    private void Start()
    {
        Destroy(gameObject);   
    }
#endif
}