using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    private CheckPoint checkPoint;

    private void Start()
    {
        checkPoint = GameObject.FindGameObjectWithTag("CheckPoint").GetComponent<CheckPoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            checkPoint.lastCheckPointPos = transform.position;
            print("Hello");
        }
    }
}
