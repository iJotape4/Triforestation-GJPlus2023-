using UnityEngine;

public class ObjectDontDestroyOnLoad : MonoBehaviour
{
    [HideInInspector]
    public string  objectID;

    void Awake()
    {
        objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
    }

    private void Start()
    {
        ObjectDontDestroyOnLoad[]  array = FindObjectsOfType<ObjectDontDestroyOnLoad>();

        for(int i=0; i< array.Length; i++)
        {
            if (array[i] != this)
            {
                if (array[i].objectID == objectID)
                    Destroy(gameObject);
            }
        }
         
        DontDestroyOnLoad(gameObject);
    }
}
