using Events;
using MyBox;
using Terraforming.Dominoes;
using UnityEngine;

public class PunishToken : DominoToken
{
    protected override void Awake()
    {
        base.Awake();
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, CheckIfSavable);
        EventManager.AddListener(ENUM_GameState.poolDescomposers, EnableDrop);
        EventManager.AddListener(ENUM_GameState.poolAnimals, EnableDrop);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, CheckIfSavable);
        EventManager.RemoveListener(ENUM_GameState.poolDescomposers, EnableDrop);
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, EnableDrop);
    }

    [Header("Hazard Script specific")]
    [SerializeField] LayerMask dominoPoleLayerMask;
    [ReadOnly, SerializeField] public bool savable { get; private set; }

    private void CheckIfSavable()
    {
        int validPoles = 0;
        foreach(DominoPole pole in poles)
        {
            for (int direction = 0; direction < 2; direction++)
            {
                float angleOffset = direction == 0 ? 60f : -60f;
                Quaternion rotation = Quaternion.Euler(0, pole.pivot.eulerAngles.y + angleOffset, 0); // Rotate in the Y-axis
                Vector3 directionVector = rotation * Vector3.forward; // Use Vector3.forward for the X-Z plane

                RaycastHit hit;

                if (Physics.Raycast(pole.pivot.position, directionVector, out hit, 1.2f, dominoPoleLayerMask))
                {
                    validPoles++;
                    Debug.DrawLine(pole.pivot.position, hit.point,Color.green,10f);
                }
            }
        }

        if(validPoles>=6)
            savable = true;
        else
        {
            savable = false;
            GetComponent<DominoPole>().MarkPunishTokenPoleAsOccupied();
        }
    }

    private void EnableDrop()
    {
        dominoCollider.enabled = true;
    }

    public void RecoverEcosystem(ENUM_Biome biome, GameObject animalPrefab)
    {
        savable = false;
        //TODO : Add a super fancy animation here
        GetComponent<MeshRenderer>().enabled = false;
        foreach (DominoPole pole in poles)
        {
            pole.AssignBiome(biome);
            pole.meshRenderer.enabled = true;
            pole.TurnColliderOn();
        }
        Destroy(animalPrefab);

        EventManager.Dispatch(ENUM_GameState.recoveredEcosystem);
    }
}