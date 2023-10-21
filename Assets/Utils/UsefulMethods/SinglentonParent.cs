using UnityEngine;

public abstract class SinglentonParent<T> : MonoBehaviour where T : SinglentonParent<T>
{
    public static T Instance { private set; get; }

    protected virtual void Awake()
    {
        if (Instance == null)      
            Instance = (T)this;    
        else       
            if (Instance != this)          
               Destroy(gameObject);                  
    }
}