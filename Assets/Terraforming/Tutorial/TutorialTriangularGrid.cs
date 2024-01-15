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
        // FIRST TUTORIAL STEP
        tutorialLocationQueue.Enqueue(new Vector3Int(1, -1, 2));
        tutorialLocationQueue.Enqueue(new Vector3Int(1, 0, 1));
        tutorialLocationQueue.Enqueue(new Vector3Int(2, -1, 1));

        // SECOND TUTORIAL STEP
        tutorialLocationQueue.Enqueue(new Vector3Int(1, 0, 0)); // ACID RAIN CELL
        tutorialLocationQueue.Enqueue(new Vector3Int(0, -1, 2)); // SWAPED DOMINO CELL
        // Add more locations as needed.
    }

    public Vector3 GiveNextCenter()
    {
        Vector3Int nextCenter = tutorialLocationQueue.Dequeue();
        int a = nextCenter.x;
        int b = nextCenter.y;
        int c = nextCenter.z;

        // Calculate the center using the provided formula
        float x = (0.5f * a - 0.5f * c) * edgeLength;
        float y = (-sqrt3 / 6 * a + sqrt3 / 3 * b - sqrt3 / 6 * c) * edgeLength;

        return new Vector3(x, 0, y);
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

    public override void GenerateNeighBors(Vector3Int position)
    {
        //We assume that when I want to generate neighbors is because I occupied the cell in any way
        OccupyCell(position);

        foreach (var neighbor in TriNeighbours(position))
        {
            Vector3 center = TriCenter(neighbor);
            if (!IsCellFree(neighbor))
                continue;

            generatedCells.Add(neighbor);
            Quaternion rotation;

            if (PointsUp(position))
            {
                var tile = Instantiate(gridTile, center, transform.rotation * Quaternion.Euler(0, 180, 0), this.gameObject.transform.transform.root);
                rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                DropTileTutorial dropTile = tile.GetComponent<DropTileTutorial>();
                dropTile.isUpwards = false;
                dropTile.intCenter = neighbor;
            }
            else
            {
                var tile = Instantiate(gridTile, center, Quaternion.Euler(Vector3.zero), this.gameObject.transform.root);
                DropTileTutorial dropTile = tile.GetComponent<DropTileTutorial>();
                tile.GetComponent<DropTileTutorial>().isUpwards = true;
                dropTile.intCenter = neighbor;
            }

            EventManager.Dispatch(ENUM_DominoeEvent.generatedTileEvent, center);
            //Debug.Log($"Neighbor {center}");
            //  Instantiate(gridTile, TriCenter(neighbor), transform.rotation);
        }
    }
}
