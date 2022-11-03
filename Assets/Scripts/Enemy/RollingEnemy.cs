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
    private RaycastHit hit;
    
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

        if (timer >= enemyCooldown && playerInRange )
        {
            Attack();
            Debug.Log(playerHealth.currentHealth);
        }
        
        if(playerHealth.currentHealth <= 0)
        {
            Debug.Log("Dead");
        }
        
        if(player != null)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * Vector3.Magnitude(agent.velocity) * 360f / (2f * Mathf.PI * 10));
            rb.MovePosition(agent.nextPosition + agent.velocity * Time.fixedDeltaTime);
            agent.SetDestination(player.transform.position);
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
            //agent.updatePosition = false;
            //rb.isKinematic = false;
            StartCoroutine(ChargeAttack());
            playerHealth.currentHealth -= damage;
        }
    }

    IEnumerator ChargeAttack()
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(enemyCooldown);
        agent.isStopped = false;
    }
    
}
