using Events;
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;

public class Phase2PopUp : MonoBehaviour
{
    [SerializeField] LocalizedString descomponsersLocalizedString;
    [SerializeField] LocalizedString nothingToRevocerLocalizedString;
    [SerializeField] LocalizedString animalsLocalizedString;

    [SerializeField] LocalizeStringEvent popUpText;

    [Header("Animation Params")]
    Animator animator;
    const string animationParamTrigger = "Enable";

    bool descomposersTriggered;
    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.poolDescomposers, SetDescomposersText);
        EventManager.AddListener(ENUM_GameState.poolAnimals, SetAnimalsText);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.poolDescomposers, SetDescomposersText);
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, SetAnimalsText);
    }

    private IEnumerator AnimalsCoroutine()
    {
        yield return new WaitForSeconds(1f);

    }

    private IEnumerator DescomposersCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        
    }

    private void SetAnimalsText()
    {

        if (!descomposersTriggered)
            popUpText.StringReference = nothingToRevocerLocalizedString;
        else
            popUpText.StringReference = animalsLocalizedString;

        animator.SetBool(animationParamTrigger, true);
    }

    private void SetDescomposersText()
    {
        popUpText.StringReference = descomponsersLocalizedString;
        descomposersTriggered = true;
        animator.SetBool(animationParamTrigger, true);
    }
}
