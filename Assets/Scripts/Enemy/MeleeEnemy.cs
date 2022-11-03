using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public float radius;
    public float enemyCooldown;
    public float damage;

    private float timer;
    private bool playerInRange;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= enemyCooldown && playerInRange )
        {
            Attack();
            Debug.Log(playerHealth.currentHealth);
        }

        if(playerHealth.currentHealth <= 0)
        {
            Debug.Log("Dead");
        }
        
        if (player != null)
        {
            agent.destination = player.transform.position;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInRange = false;
        }
    }

    private void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            playerHealth.currentHealth -= damage;
            playerHealth.SetSliderHealth(playerHealth.currentHealth);
        }
    }
}
