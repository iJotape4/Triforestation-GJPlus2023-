using Events;
using UnityEngine;

public class PunishesManager : MonoBehaviour
{
    [SerializeField] private GameObject acidRainPrefab;
    private TriangularGrid grid;

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
        // TODO: Add more random punishes
        AcidRainPunish();
        EventManager.Dispatch(ENUM_DominoeEvent.selectDoneEvent);
    }
    private void Start()
    {
        grid = FindObjectOfType<TriangularGrid>();
    }
    private void AcidRainPunish()
    {
        (Vector3? position, Quaternion? rotation, Vector3Int? center) selectedGridCell = grid.GetRandomFreeCell();
        if (selectedGridCell.position == null || selectedGridCell.rotation == null || selectedGridCell.center == null)
            return;

        // Instantiate the replacement prefab at the position of the selected Cell Grid.
        Instantiate(acidRainPrefab, (Vector3)selectedGridCell.position, (Quaternion)selectedGridCell.rotation, grid.transform);
        // Trigger the selectDone event or perform any other desired actions.
        grid.OccupyCell((Vector3Int)selectedGridCell.center);
        EventManager.Dispatch(ENUM_DominoeEvent.spawnedAcidRainEvent); 
    }
}