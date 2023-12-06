using Events;
using UnityEngine;

public class PunishesManager : MonoBehaviour
{
    [SerializeField] private GameObject acidRainPrefab;
    private Transform grid;

    private void Awake()
    {
        EventManager.AddListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
    }


    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_DominoeEvent.punishEvent, TriggerRandomPunish);
    }
    private void TriggerRandomPunish()
    {
        AcidRainPunish();
        EventManager.Dispatch(ENUM_DominoeEvent.selectDoneEvent);
    }
    private void Start()
    {
        grid = FindObjectOfType<GridChangePhase>().transform;
    }
    private void AcidRainPunish()
    {
        // Get all objects with the "CellGrid" tag in the scene.
        GameObject[] cellGrids = GameObject.FindGameObjectsWithTag("GridCell");
  

        // Select a random active Cell Grid.
        GameObject selectedCellGrid = null;
        bool punishApplied = false;
        while (!punishApplied)
        {
            int randomIndex = Random.Range(0, cellGrids.Length);
            GameObject randomCellGrid = cellGrids[randomIndex];

            if (randomCellGrid.activeInHierarchy)
            {
                selectedCellGrid = randomCellGrid;

                // Instantiate the replacement prefab at the position of the selected Cell Grid.
                Instantiate(acidRainPrefab, selectedCellGrid.transform.position, selectedCellGrid.transform.rotation, grid);

                // Deactivate (set inactive) the selected Cell Grid.
                selectedCellGrid.SetActive(false);

                // Trigger the selectDone event or perform any other desired actions.
                EventManager.Dispatch(ENUM_DominoeEvent.spawnedAcidRainEvent);
                punishApplied = true;
            }
        }
    }
}