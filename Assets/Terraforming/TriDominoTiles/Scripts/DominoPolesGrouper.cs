using Events;
using System;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;

public class DominoPolesGrouper : MonoBehaviour
{
    DominoPole[] polesList;
    List<DominoPole> agroupedPoles = new List<DominoPole>();

    List<Group> groupsList = new List<Group>();
    int droppedAnimals=0;

    private void Awake()
    {
        EventManager.AddListener(ENUM_GameState.secondPhaseFinished, SecondPhaseFinised);
        EventManager.AddListener(ENUM_GameState.poolAnimals, OnPoolAnimalsStarted);
        EventManager.AddListener(ENUM_AnimalEvent.animalDroped, CheckLevelEnd);
    }

    private void OnDestroy()
    {
        EventManager.RemoveListener(ENUM_GameState.secondPhaseFinished, SecondPhaseFinised);
        EventManager.RemoveListener(ENUM_GameState.poolAnimals, OnPoolAnimalsStarted);
        EventManager.RemoveListener(ENUM_AnimalEvent.animalDroped, CheckLevelEnd);
    }
    private void CheckLevelEnd()
    {
        droppedAnimals++;
        if(droppedAnimals >= polesList.Length)
        {
            EventManager.Dispatch(ENUM_GameState.secondPhaseFinished);
        }
    }

    private void OnPoolAnimalsStarted()
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
        pole.group = group;

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
    
    void SecondPhaseFinised()
    {
        int totalScore=0;
        foreach (Group group in groupsList)
        {
            totalScore += group.GetGroupScore();
        }

        Debug.Log("total score" + totalScore);
    }
}


public class Group
{
    public Group(int ID, ENUM_Biome biome)
    {
        this.ID = ID;
        this.biome = biome;
        poles = new List<DominoPole>();
       // animals = new List<Animal>();
    }

   public int ID;
   public ENUM_Biome biome;
   public List<DominoPole> poles;
    //public List<Animal> animals;
    private int alphas=0;
    private int predators=0;
    private int preys = 0;

    public bool AddAnimal(ENUM_FoodChainLevel chainLevel)
    {
        switch (chainLevel)
        {
            case ENUM_FoodChainLevel.Prey:
                this.preys++;

                return true;
            case ENUM_FoodChainLevel.Predator:
                 if(predators+1 <= preys / 2 && preys>0)
                 {
                    predators++;
                    return true;
                 }
                else return false;
            case ENUM_FoodChainLevel.AnimalKing:
                if(alphas+1 <= predators / 2 && predators>0)
                {
                    this.alphas++;
                    return true;
                }       
                else return false;
            default:
                return false;
        }
    }
    
    public int GetGroupScore()
    {
        //Suggested Score
        //int score = 0;
        //score += alphas * 3;
        //score += primaryConsomers * 2;
        //score += secondaryConsomers;

        //Current score
        int score = (predators+preys+alphas)*2;        
        return score;
    }
}