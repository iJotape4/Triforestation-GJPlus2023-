using Events;
using UnityEngine;

public class DivineHelpsManager : SinglentonParent<DivineHelpsManager>
{
    [SerializeField] GameObject divineHelpsPanel;
    private TriangularGrid grid;
    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(ENUM_DominoeEvent. selectDoneEvent, EnableDivineHelpsPanel);
        EventManager.AddListener<GameObject>(ENUM_DominoeEvent.startBonusMoorEvent, SpawnBonusMoor);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_DominoeEvent.selectDoneEvent, EnableDivineHelpsPanel);      
    }

    private void EnableDivineHelpsPanel()
    {
        divineHelpsPanel.SetActive(true);
    }   
    public void DisableDivineHelpsPanel()
    {
        divineHelpsPanel.SetActive(false);
    }

    private void Start()
    {
        grid = FindObjectOfType<TriangularGrid>();
    }

    public void SpawnBonusMoor(GameObject moorPrefab)
    {
        (Vector3? position, Quaternion? rotation, Vector3Int? center) selectedGridCell = grid.GetRandomFreeCell();
        if (selectedGridCell.position == null || selectedGridCell.rotation == null || selectedGridCell.center == null)
            return;
        // Instantiate the replacement prefab at the position of the selected Cell Grid.
        Instantiate(moorPrefab, (Vector3)selectedGridCell.position, (Quaternion)selectedGridCell.rotation, grid.transform);
        // Trigger the selectDone event or perform any other desired actions.
        grid.GenerateNeighBors((Vector3Int)selectedGridCell.center);
        EventManager.Dispatch(ENUM_DominoeEvent.spawnedMoorEvent);
    }
}