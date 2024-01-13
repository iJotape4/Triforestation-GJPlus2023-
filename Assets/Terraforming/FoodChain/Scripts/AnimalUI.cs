using Events;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimalUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Camera mainCamera;

    [Header("Animal Data")]
    [SerializeField] public Animal animal;
    [SerializeField] Image UiImage;

    [Header("CurrentPrefab Data")]
    private bool isDragging = false;
    public GameObject spawnedPrefab; // The instance of the spawned prefab
    private bool canDrop;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void SetAnimal(Animal _animal)
    {
        animal = _animal;
        SetAnimalValues();
    }

    private void SetAnimalValues()
    {
        UiImage.sprite = animal.sprite;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Spawn the prefab at the cursor position
        spawnedPrefab = Instantiate(animal.Get3DPrefab(), GetMouseWorldPosition(eventData), Quaternion.identity);
        EventManager.Dispatch(ENUM_AnimalEvent.animalPrefabCreated);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Update the position of the spawned prefab based on the cursor movement
        if (isDragging)
        {
            spawnedPrefab.transform.position = GetMouseWorldPosition(eventData);
        }
    }
    /// <summary>
    /// Clean the spawned prefab if the drop is not valid
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        if (!canDrop)    
            InvalidDrop();       
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            DominoPole pole = hit.transform.GetComponent<DominoPole>();
            if (pole)
            {
                canDrop = true;
                if(animal.chainLevel != ENUM_FoodChainLevel.Bug)
                {
                    pole.CheckBiome(animal.biome);
                }
                else
                {
                      pole.CheckBiome(0);
                }
            }
            else           
                canDrop = false;
            
            return hit.point;
        }

        Debug.LogWarning("No hit", spawnedPrefab);
        return Vector3.zero; // Default if no hit
    }

    public void InvalidDrop()
    {
        Destroy(spawnedPrefab);
        spawnedPrefab = null;
        EventManager.Dispatch(ENUM_AnimalEvent.animalPrefabDestroyed);
    }
}