using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class DominoPolesGrouper : MonoBehaviour
{
    DominoPole[] polesList;
    List<DominoPole> agroupedPoles = new List<DominoPole>();

    List<Group> groupsList = new List<Group>();


    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, OnSecondPhaseFinished);
        EventManager.AddListener(ENUM_GameState.firstPhaseFinished, OnSecondPhaseFinished);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, OnSecondPhaseFinished);
        EventManager.RemoveListener(ENUM_GameState.firstPhaseFinished, OnSecondPhaseFinished);
    }

    private void OnSecondPhaseFinished()
    {
        polesList = GetComponentsInChildren<DominoPole>();  

        foreach (var pole in polesList)
        {
            if ((int)pole.biome == -1)
                continue;

            if (!agroupedPoles.Contains(pole))
            {
                Group newGroup = new Group(groupsList.Count, pole.biome);
                groupsList.Add(newGroup);
                CheckPoleConnections(pole, newGroup);
            }          
        }

        CountGroupsList();
    }

    void CheckPoleConnections(DominoPole pole , Group group)
    {
        Debug.Log("Checking " + pole.biome + " pole connections", pole.gameObject);
        agroupedPoles.Add(pole);
        group.poles.Add(pole);

        bool[] connected = new bool[4];

        for (int direction = 0; direction < 4; direction++)
        {
            float angleOffset = direction switch
            {
                0 => 60f, // "left-Out" connection
                1 => -60f, // "Right-Out" connection
                2 => 120f, // "left - In" connection
                3 => -120f, // "Right - In" connection
                _ => 60f, // Handle other cases or error condition.
            };

            Quaternion rotation = Quaternion.Euler(0, pole.pivot.eulerAngles.y + angleOffset, 0); // Rotate in the Y-axis
            Vector3 directionVector = rotation * Vector3.forward; // Use Vector3.forward for the X-Z plane

            int dominoPoleLayerMask = LayerMask.GetMask("DominoPole");

            RaycastHit hit;
            if (Physics.Raycast(pole.pivot.position, directionVector, out hit, 1.5f, dominoPoleLayerMask))
            {
                DominoPole hitPole = hit.collider.GetComponent<DominoPole>();
                if (hitPole != null && hitPole.biome == pole.biome )
                {
                    connected[direction] = true;
                    if (!agroupedPoles.Contains(hitPole))
                    {
                        CheckPoleConnections(hitPole, group);
                    }
                }

            }
        }
        //Debug.Log(pole.biome + " pole has connections: " + connected[0] + connected[1] + connected[2] + connected[3], pole.gameObject);           
    }

    void CountGroupsList()
    {
        foreach (Group group in groupsList)
        {
            Debug.Log(" Group " + group.ID + " of " + group.biome + " has " + group.poles.Count + " poles");

        }
    }        
}


public struct Group
{
    public Group(int ID, ENUM_Biome biome)
    {
        this.ID = ID;
        this.biome = biome;
        poles = new List<DominoPole>();
        animals = new List<Animal>();
    }

   public int ID;
   public ENUM_Biome biome;
   public List<DominoPole> poles;
   public List<Animal> animals;

    public void CalculateAnimalsAmount()
    {
        int alphas=0, primaryConsomers=0, secondaryConsomers=0;


        foreach (DominoPole pole in poles)       
            if (pole.animalData)            
               animals.Add(pole.animalData);       
        
        foreach (Animal animal in animals)
        {
            switch (animal.chainLevel)
            {
                case ENUM_FoodChainLevel.AnimalKing:
                    alphas++;
                    break;
                case ENUM_FoodChainLevel.Predator:
                    primaryConsomers++;
                    break;
                case ENUM_FoodChainLevel.Prey:
                    secondaryConsomers++;
                    break;
                default:
                    break;
            }
        }
    }
}