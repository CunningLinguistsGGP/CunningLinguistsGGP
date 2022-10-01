using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private UnityEngine.AI.NavMeshAgent agent;

    [SerializeField]
    private GameObject player;
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");

    }
    
    void Update()
    {
        agent.destination = player.transform.position;
    }
}
