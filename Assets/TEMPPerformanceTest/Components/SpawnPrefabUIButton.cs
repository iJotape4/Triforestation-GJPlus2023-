using TMPro;
using UnityEngine;

public class SpawnPrefabUIButton : UIButton
{
    public GameObject[] prefabsList;
    public Camera mainCamera;
    [SerializeField] TextMeshProUGUI text;
    int counter;
    void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Assuming the main camera is tagged as "MainCamera"
        }    
    }

    protected override void ClickButtonMethod()
    {
        SpawnObjectInRandomPosition();
    }

    void SpawnObjectInRandomPosition()
    {
        int random = Random.Range(0, prefabsList.Length);

        // Define a random position within the camera's viewport
        Vector3 randomViewportPosition = new Vector3(Random.value, Random.value, 0);
        Vector3 randomWorldPosition = mainCamera.ViewportToWorldPoint(randomViewportPosition);
        randomWorldPosition.y = 0;
        // Instantiate the object at the random position
        Instantiate(prefabsList[random], randomWorldPosition, Quaternion.identity);
        counter++;
        text.text = "spawned objects: "+counter.ToString();
    }
}