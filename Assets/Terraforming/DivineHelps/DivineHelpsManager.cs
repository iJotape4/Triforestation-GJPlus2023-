using Events;
using UnityEngine;

public class DivineHelpsManager : SinglentonParent<DivineHelpsManager>
{
    [SerializeField] GameObject divineHelpsPanel;
    private Transform grid;
    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(ENUM_DominoeEvent.selectDoneEvent, EnableDivineHelpsPanel);
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
        grid = FindObjectOfType<GridChangePhase>().transform;
    }

    public void SpawnBonusMoor(GameObject moorPrefab)
    {
        GameObject[] cellGrids = GameObject.FindGameObjectsWithTag("GridCell");


        bool punishApplied = false;
        while (!punishApplied)
        {
            int randomIndex = Random.Range(0, cellGrids.Length);
            GameObject randomCellGrid = cellGrids[randomIndex];

            if (randomCellGrid.activeInHierarchy)
            {
                // Instantiate the replacement prefab at the position of the selected Cell Grid.
                Instantiate(moorPrefab, randomCellGrid.transform.position, randomCellGrid.transform.rotation, grid);

                // Deactivate (set inactive) the selected Cell Grid.
                randomCellGrid.SetActive(false);


                punishApplied = true;
            }
        }
    }
}