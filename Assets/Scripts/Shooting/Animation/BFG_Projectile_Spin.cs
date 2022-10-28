using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFG_Projectile_Spin : MonoBehaviour
{
    [SerializeField] private float startSpeed = 0.0f, endSpeed = 90.0f,spinUpTime = 5.0f;
    [SerializeField] GameObject[] frames;

    private float currentSpeed;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSpeed<endSpeed)
        {
            currentSpeed += Time.deltaTime * (endSpeed - startSpeed) / spinUpTime;
        }

        for (int i = 0; i < frames.Length; i++)
        {
            frames[i].transform.Rotate(Time.deltaTime * currentSpeed, Time.deltaTime * currentSpeed, Time.deltaTime * currentSpeed);
        }

    }
}
