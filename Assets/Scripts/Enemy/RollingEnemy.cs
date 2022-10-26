using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RollingEnemy : MonoBehaviour
{
    public float radius;
    public float enemyCooldown;
    public float damage;

    private float timer;
    private bool playerInRange;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;
    private Rigidbody rb;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        playerHealth = player.GetComponent<PlayerScript>();
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if(playerHealth.currentHealth <= 0)
        {
            Debug.Log("Dead");
        }
        
        if (player != null)
        {
            agent.destination = player.transform.position;
            rb.AddForce(agent.destination * Time.fixedDeltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (timer >= enemyCooldown && playerInRange)
        {
            Attack();
            Debug.Log(playerHealth.currentHealth);
        }
    }


    private void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.currentHealth -= damage;
        }
    }
}
