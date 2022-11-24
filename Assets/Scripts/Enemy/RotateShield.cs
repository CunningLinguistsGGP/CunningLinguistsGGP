using UnityEngine;

public class RotateShield : MonoBehaviour
{
    public float speed;
    public Transform rotatePoint;

    private void Update()
    {
        transform.RotateAround(rotatePoint.position, Vector3.up, speed * Time.deltaTime);
    }
}
