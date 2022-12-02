using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRoom : MonoBehaviour
{
    [SerializeField] GameObject room;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Quaternion rotationNeeded = Quaternion.identity;
        if (other.tag == "Player")
        {

            Transform pos = other.transform;

            rotationNeeded.SetFromToRotation(Vector3.up, GameObject.Find("facing").transform.position - room.transform.position);

            Vector3 axis;
            float angle;
            rotationNeeded.ToAngleAxis(out angle,out axis);

            pos.Rotate(axis, angle);

            room.transform.rotation *= rotationNeeded;

            other.transform.position = pos.position;
        }
    }
}
