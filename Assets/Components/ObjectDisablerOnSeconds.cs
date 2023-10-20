using System.Collections;
using UnityEngine;

public class ObjectDisablerOnSeconds : MonoBehaviour
{
    [SerializeField] private float disableSeconds;

    private void OnEnable() =>
        StartCoroutine(Timer());

    private IEnumerator Timer()
    {
        yield return new WaitForSecondsRealtime(disableSeconds);
        gameObject.SetActive(false);
    }
}
