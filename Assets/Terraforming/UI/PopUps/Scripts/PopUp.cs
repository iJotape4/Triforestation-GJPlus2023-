using UnityEngine;

public class PopUp : MonoBehaviour
{
    [Header("Animation Params")]
    protected Animator animator;
    protected const string animationParamTrigger = "Enable";

    protected void Start() => animator = GetComponent<Animator>();
    protected void OpenPopUp(bool active) => animator.SetBool(animationParamTrigger, active);
}