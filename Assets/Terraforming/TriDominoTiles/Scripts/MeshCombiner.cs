using MyBox;
using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    MeshCollider collider;
    private void Awake()
    {
        collider = GetComponent<MeshCollider>();
    }
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            //combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            //meshFilters[i].gameObject.SetActive(false);

            i++;
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combine);
        this.GetOrAddComponent<MeshFilter>().sharedMesh = mesh;
        //gameObject.SetActive(true);
        collider.enabled = false;
        collider.sharedMesh = mesh;
        collider.enabled = true;
    }
}