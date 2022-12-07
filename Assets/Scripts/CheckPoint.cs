using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private static CheckPoint instance;
    public Vector3 lastCheckPointPos;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
