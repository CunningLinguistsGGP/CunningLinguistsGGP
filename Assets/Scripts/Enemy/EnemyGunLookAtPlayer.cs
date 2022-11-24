using UnityEngine;

public class EnemyGunLookAtPlayer : MonoBehaviour
{
    private new Transform camera;
    
    void Start()
    {
        if (Camera.main is not null)
        {
            camera = Camera.main.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camera);
    }
}
