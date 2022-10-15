using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private new Transform camera;
    private void Start()
    {
        if (Camera.main is not null)
        {
            camera = Camera.main.transform;
        }
    }

    private void Update()
    {
        transform.LookAt(camera);
    }
}
