using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;
public class TutorialTriangularGrid : TriangularGrid
{
    private Queue<Vector3Int> tutorialLocationQueue = new Queue<Vector3Int>();

    protected override void Start()
    {
        Vector3 center = TriCenter(initialPosition);
        GameObject token1 = Instantiate(token, center, Quaternion.Euler(0, 60, 0), this.gameObject.transform);
        token1.GetComponentInChildren<MeshCollider>().enabled = false;
        OccupyCell(initialPosition);
        generatedCells.Add(initialPosition);
        GenerateNeighBors(initialPosition);
        TokenData tokenData = token1.GetComponent<DominoToken>().tokenData = new TokenData(); ;
        tokenData.biomes = new ENUM_Biome[] { (ENUM_Biome)(-1), (ENUM_Biome)(-1), (ENUM_Biome)(-1) };
        StartTutorial();
    }

    private void StartTutorial()
    {
        // Add tutorial locations to the queue.
        EnqueueTutorialLocations();
    }

    private void EnqueueTutorialLocations()
    {
        tutorialLocationQueue.Enqueue(new Vector3Int(0, 0, 0));
        tutorialLocationQueue.Enqueue(new Vector3Int(1, 0, -1));
        tutorialLocationQueue.Enqueue(new Vector3Int(2, 0, 0));
        // Add more locations as needed.
    }

    public bool CheckCardLocation(Vector3Int location)
    {
        // Check if the queue is not empty
        if (tutorialLocationQueue.Count > 0)
        {
            // Peek at the front of the queue to get the current card's location
            Vector3Int currentCardLocation = tutorialLocationQueue.Peek();

            // Check if the given location matches the current card's location
            if (location == currentCardLocation)
            {
                return true;
            }
        }

        // Return false if the location does not match the current card's location or the queue is empty
        return false;
    }

    public void NextTutorialCard()
    {
        tutorialLocationQueue.Dequeue();
    }

}
