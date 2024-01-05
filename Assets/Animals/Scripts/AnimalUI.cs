using UnityEngine;
using UnityEngine.EventSystems;

public class AnimalUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public GameObject prefabToSpawn; // Reference to the prefab
    private GameObject spawnedPrefab; // The instance of the spawned prefab

    private bool isDragging = false;
    private Camera mainCamera;

    //TODO: Add fields for animal Data, and load the reference prefab from the animal data
    //TODO: Add the drop function taking in account the raycasting
    void Start()
    {
        mainCamera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Spawn the prefab at the cursor position
        spawnedPrefab = Instantiate(prefabToSpawn, GetMouseWorldPosition(eventData), Quaternion.identity);

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

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition(PointerEventData eventData)
    {
        Ray ray = mainCamera.ScreenPointToRay(eventData.position);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        Debug.LogWarning("No hit", spawnedPrefab);
        return Vector3.zero; // Default if no hit
    }
}