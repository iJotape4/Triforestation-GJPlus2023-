using Events;
using MyBox;
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
        GameObject token1 = Instantiate(token, center, transform.rotation, this.gameObject.transform);
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
        tutorialLocationQueue.Enqueue(new Vector3Int(-1, 1, 1));
        tutorialLocationQueue.Enqueue(new Vector3Int(0, 1, 0));
        tutorialLocationQueue.Enqueue(new Vector3Int(0, 0, 1));
        // Add more locations as needed.
    }

    // To check if the domino is placed where is supposed to
    public bool CheckCardLocation(Transform position)
    {
        float x = position.position.x;
        float y = position.position.z; 

        int a = Mathf.CeilToInt((1 * x - sqrt3 / 3 * y) / edgeLength);
        int b = Mathf.FloorToInt((sqrt3 * 2 / 3 * y) / edgeLength) + 1;
        int c = Mathf.CeilToInt((-1 * x - sqrt3 / 3 * y) / edgeLength);
        Vector3Int location = new Vector3Int(a, b, c);
        // Check if the queue is not empty
        if (tutorialLocationQueue.Count > 0)
        {
            // Peek at the front of the queue to get the current card's location
            Vector3Int currentCardLocation = tutorialLocationQueue.Peek();
            print("current vector3 location = " + currentCardLocation + ".... Location : " + location );
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
