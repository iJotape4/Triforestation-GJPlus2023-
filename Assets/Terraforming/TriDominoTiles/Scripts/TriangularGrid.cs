using Events;
using System.Collections.Generic;
using Terraforming.Dominoes;
using UnityEngine;
using UnityEngine.EventSystems;

public class TriangularGrid : DropView
{
    protected float edgeLength = 2.962f; // Lenght of the edge of the Triangular Token
    protected float sqrt3 = Mathf.Sqrt(3);

    [SerializeField] protected GameObject token;
    public GameObject gridTile;
    public Vector3Int initialPosition;
    protected HashSet<Vector3Int> generatedCells = new HashSet<Vector3Int>();
    private HashSet<Vector3Int> occupiedCells = new HashSet<Vector3Int>();

    private ((int min, int max) x , (int min, int max) y , (int min,  int max) z) currentRange;
    

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
        //Debug.Log($"Occupied {cell}");
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

    public void CalculateMaxRange(Vector3Int cell)
    {
        if(currentRange.y.max < cell.y)       
            currentRange.y.max = cell.y;        
        else if(currentRange.y.min > cell.y)
            currentRange.y.min = cell.y;
        
        if(currentRange.x.max < cell.x)     
               currentRange.x.max = cell.x;
        
        else if(currentRange.x.min > cell.x)        
            currentRange.x.min = cell.x;
        
        if(currentRange.z.max < cell.z)       
            currentRange.z.max = cell.z;
        
        else if(currentRange.z.min > cell.z)
            currentRange.z.min = cell.z;
    }

    protected virtual void Start()
    {
        SpawnInitialDominoe();
    }

    // TODO : Add a method to set the initialPosition coords from level selector
    private void SpawnInitialDominoe()
    {
        Vector3 center = TriCenter(initialPosition);
        GameObject token1 = Instantiate(token, center, transform.rotation, this.gameObject.transform);
        token1.GetComponentInChildren<MeshCollider>().enabled = false;
        OccupyCell(initialPosition);
        generatedCells.Add(initialPosition);
        GenerateNeighBors(initialPosition);
        TokenData tokenData = token1.GetComponent<DominoToken>().tokenData = new TokenData(); ;
        tokenData.biomes = new ENUM_Biome[] { (ENUM_Biome)(-1), (ENUM_Biome)(-1), (ENUM_Biome)(-1) };
    }

    public virtual void GenerateNeighBors(Vector3Int position)
    {
        //We assume that when I want to generate neighbors is because I occupied the cell in any way
        OccupyCell(position);

        foreach (var neighbor in TriNeighbours(position))
        {
            Vector3 center = TriCenter(neighbor);
            if(!IsCellFree(neighbor))
                continue;

            generatedCells.Add(neighbor);
            Quaternion rotation;

            if (PointsUp(position))
            {
                var tile = Instantiate(gridTile, center, transform.rotation * Quaternion.Euler(0, 180, 0), this.gameObject.transform.transform.root);
                rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                DropTile dropTile = tile.GetComponent<DropTile>();
                dropTile.isUpwards = false;
                dropTile.intCenter = neighbor;
            }
            else
            {
                var tile = Instantiate(gridTile, center, Quaternion.Euler(Vector3.zero), this.gameObject.transform.root);
                DropTile dropTile = tile.GetComponent<DropTile>();
                tile.GetComponent<DropTile>().isUpwards = true;
                dropTile.intCenter = neighbor;
            }

            EventManager.Dispatch(ENUM_DominoeEvent.generatedTileEvent, center);
            //Debug.Log($"Neighbor {center}");
            //  Instantiate(gridTile, TriCenter(neighbor), transform.rotation);
        }
    }

    //Get a random cell from generated cells hash that is not in the occupied cells hash
    public (Vector3? position, Quaternion? rotation, Vector3Int? center) GetRandomFreeCell()
    {
        List<Vector3Int> freeCells = new List<Vector3Int>();
        foreach (Vector3Int cell in generatedCells)       
            if (!occupiedCells.Contains(cell))          
                freeCells.Add(cell);               

        if(freeCells.Count == 0)
        {
            EventManager.Dispatch(ENUM_GameState.firstPhaseFinished);
            return (null, null, null); //TODO: Change this to a custom exception (NoFreeCellsException)
        }

        int randomIndex = Random.Range(0, freeCells.Count);
        Quaternion rotation = PointsUp(freeCells[randomIndex]) ? Quaternion.Euler(Vector3.zero) :  Quaternion.Euler(0, 180, 0);

        return  (TriCenter( freeCells[randomIndex]) , rotation, freeCells[randomIndex]);
    }

    public virtual DropTile FindDropTileByIntCenter(Vector3Int center)
    {
        foreach(var tile in FindObjectsOfType<DropTile>())
        {
            if (tile.intCenter == center)
                return tile;
        }
        return null;
    }

    public static TriangularGrid FindTriangularGrid() => FindObjectOfType<TriangularGrid>().transform.root.GetComponent<TriangularGrid>();
    public override void OnDrop(PointerEventData eventData) { }
}