using System.Collections.Generic;
using Terraforming.Dominoes;
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

        if(occupiedCells!=null && IsCellFree(new Vector3Int(a,b,c)))
            return null;        
        else
        {
            OccupyCell(new Vector3Int(a, b, c));
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

    private void Start()
    {
        SpawnInitialDominoe();
    }

    // TODO : Add a method to set the initialPosition coords from level selector
    private void SpawnInitialDominoe()
    {
        Vector3 center = TriCenter(initialPosition);
        GameObject token1 = Instantiate(token, center, transform.rotation);
        token1.GetComponentInChildren<MeshCollider>().enabled = false;
        OccupyCell(initialPosition);
        GenerateNeighBors(initialPosition);
        TokenData tokenData = token1.GetComponent<DominoToken>().tokenData = new TokenData(); ;
        tokenData.biomes = new ENUM_Biome[] { (ENUM_Biome)(-1), (ENUM_Biome)(-1), (ENUM_Biome)(-1) };
    }

    public void GenerateNeighBors(Vector3Int position)
    {
        foreach (var neighbor in TriNeighbours(position))
        {
            Vector3 center = TriCenter(neighbor);
            if(!IsCellFree(neighbor))
                continue;
            
            OccupyCell(neighbor);
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

            //Debug.Log($"Neighbor {center}");
            //  Instantiate(gridTile, TriCenter(neighbor), transform.rotation);
        }
    }
    public override void OnDrop(PointerEventData eventData) { }
}