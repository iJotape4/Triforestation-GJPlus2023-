using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriangularGrid : DropView
{
    private float edgeLength = 2.962f; // Lenght of the edge of the Triangular Token
    private float sqrt3 = Mathf.Sqrt(3);

    [SerializeField] private GameObject token;
    public GameObject gridTile;
    public Vector3Int initialPosition;
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    public static List<(float x, float y, float z)> occupiedPositions = new List<(float x, float y, float z)>();

    public Vector3 TriCenter(int a, int b, int c)
    {
        // Calculate the center using the provided formula
        float x = (0.5f * a - 0.5f * c) * edgeLength;
        float y = (-sqrt3 / 6 * a + sqrt3 / 3 * b - sqrt3 / 6 * c) * edgeLength;

        return new Vector3(x, 0,y);
    }

    public Vector3 TriCenter(Vector3Int tri)
    {
        return TriCenter(tri.x, tri.y, tri.z);
    }

    // Function to check if a triangle is pointing upwards
    public bool PointsUp(int a, int b, int c)
    {
        return a + b + c == 2;
    }

    public bool PointsUp(Vector3Int tri)
    {
        return PointsUp(tri.x, tri.y, tri.z);
    }

    // Function to find the triangle containing a given Cartesian coordinate point
    // Returns null if the triangle is already occupied
    public Vector3Int? PickTri(float x, float y)
    {
        // Using dot product, measures which row and diagonals a given point occupies.
        // Or equivalently, multiply by the inverse matrix to tri_center
        // Note we have to break symmetry, using floor(...)+1 instead of ceil, in order
        // to deal with corner vertices like (0, 0) correctly.

        int a = Mathf.CeilToInt((1 * x - sqrt3 / 3 * y) / edgeLength);
        int b = Mathf.FloorToInt((sqrt3 * 2 / 3 * y) / edgeLength) + 1;
        int c = Mathf.CeilToInt((-1 * x - sqrt3 / 3 * y) / edgeLength);

        if(occupiedPositions!=null && occupiedPositions.Contains((a, b, c)))        
            return null;        
        else
        {
            occupiedPositions.Add((a, b, c));
            return new Vector3Int(a, b, c);
        }
    }

    // Function to get triangles that share an edge with the given triangle
    public Vector3Int[] TriNeighbours(int a, int b, int c)
    {
        if (PointsUp(a, b, c))
        {
            return new Vector3Int[]
            {
                new Vector3Int(a - 1, b, c),
                new Vector3Int(a, b - 1, c),
                new Vector3Int(a, b, c - 1)
            };
        }
        else
        {
            return new Vector3Int[]
            {
                new Vector3Int(a + 1, b, c),
                new Vector3Int(a, b + 1, c),
                new Vector3Int(a, b, c + 1)
            };
        }
    }

    // Function to check if a cell is free
    public bool IsCellFree(Vector3Int cell)
    {
        return !occupiedCells.Contains(cell);
    }

    // Function to occupy a cell
    public void OccupyCell(Vector3Int cell)
    {
        occupiedCells.Add(cell);
    }

    // Function to free a cell
    public void FreeCell(Vector3Int cell)
    {
        occupiedCells.Remove(cell);
    }

    public Vector3Int[] TriNeighbours(Vector3Int tri)
    {
        return TriNeighbours(tri.x, tri.y, tri.z);
    }

    //TODO : Test purposes only, just delete!
    private void Start()
    {
        Vector3 center = TriCenter(initialPosition);
        GameObject token1 = Instantiate(token, center, transform.rotation);
        token1.GetComponentInChildren<MeshCollider>().enabled = false;
        occupiedPositions.Add(Vector3ToTuple(center));
        GenerateNeighBors(initialPosition);

        //occupiedPositions.Add((1,0, 1));
        //occupiedPositions.Add((1, 1, 0));
        //occupiedPositions.Add((0, 2, 0));

        //Vector3 initialPos2 = new Vector3(center2.x, transform.position.y, center2.y);
        //Vector3 initialPos3 = new Vector3(center3.x, transform.position.y, center3.y);
        //Vector3 initialPos4 = new Vector3(center4.x, transform.position.y, center4.y);

        //GameObject token2 =Instantiate(token, initialPos2, transform.rotation);
        //token2.GetComponentInChildren<MeshCollider>().enabled = false;
        //GameObject token3= Instantiate(token, initialPos3, transform.rotation);
        //token3.GetComponentInChildren<MeshCollider>().enabled = false;
        //GameObject token4 =Instantiate(token, initialPos4, transform.rotation);
        //token4.GetComponentInChildren<MeshCollider>().enabled = false;

    }

    public void GenerateNeighBors(Vector3Int position)
    {
        foreach (var neighbor in TriNeighbours(position))
        {
            Vector3 center = TriCenter(neighbor);
            if (occupiedPositions.Contains(Vector3ToTuple(center)))
                continue;

            occupiedPositions.Add((center.x, center.y, center.z));
            Quaternion rotation;

            if (PointsUp(position))
            {
                var tile = Instantiate(gridTile, center, transform.rotation * Quaternion.Euler(0, 180, 0));
                rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                DropTile dropTile = tile.GetComponent<DropTile>();
                dropTile.isUpwards = false;
                dropTile.intCenter = neighbor;
            }
            else
            {
                var tile = Instantiate(gridTile, center, Quaternion.Euler(Vector3.zero));
                DropTile dropTile = tile.GetComponent<DropTile>();
                tile.GetComponent<DropTile>().isUpwards = true;
                dropTile.intCenter = neighbor;
            }

            Debug.Log($"Neighbor {center}");
            //  Instantiate(gridTile, TriCenter(neighbor), transform.rotation);
        }
    }

    public (float x, float y, float z) Vector3ToTuple(Vector3 center)=> ((float x, float y, float z))(center.x, center.y, center.z);

    public override void OnDrop(PointerEventData eventData) { }
}