using Events;
using System.Collections;
using TMPro;
using UnityEngine;

public class AnimalBehaviour : MonoBehaviour
{
    [SerializeField] public Animal animalData;
    [SerializeField] TextMeshProUGUI animalText;
    public void OnAnimalDroped()
    {
       EventManager.Dispatch(ENUM_SFXEvent.animalSound,animalData.animalSFX);
       animalText.gameObject.SetActive(true);
        StartCoroutine(DisableTMP());
    }

    private void Start()
    {
        string text = animalData.chainLevel  switch
        {
            ENUM_FoodChainLevel.AnimalKing => "+3",
            ENUM_FoodChainLevel.Predator => "+2",
            ENUM_FoodChainLevel.Prey => "+1",
            _=> "+1" 
        };
        animalText.text = text;
    }

    public IEnumerator DisableTMP()
    {
        yield return new WaitForSeconds(1f);
        animalText.gameObject.SetActive(false);
    }
}