using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float radius;
    public float enemyCooldown;
    public float damage;

    private bool playerInRange;

    private UnityEngine.AI.NavMeshAgent agent;
    private GameObject player;
    
    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    private void Update()
    {
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
        if (other.gameObject.CompareTag("Player"))
        {
            player.gameObject.GetComponent<PlayerScript>().currentHealth -= damage;
            StartCoroutine(AttackCooldown());
            Debug.Log(player.gameObject.GetComponent<PlayerScript>().currentHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            agent.isStopped = false;
            Debug.Log("left");
        }
    }

    IEnumerator AttackCooldown()
    {
        playerInRange = false;
        yield return new WaitForSeconds(enemyCooldown);
        playerInRange = true;
    }
}
