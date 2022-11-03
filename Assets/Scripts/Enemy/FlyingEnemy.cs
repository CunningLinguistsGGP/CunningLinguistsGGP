using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public Rigidbody projectile;
    public float range = 50.0f;
    public float bulletImpulse = 20.0f;

    public float radius;
    public float enemyCooldown;
    public float damage;

    private float timer;
    private bool playerInRange;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;

    private new Transform camera;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();

        if (Camera.main is not null)
        {
            camera = Camera.main.transform;
        }
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= enemyCooldown && playerInRange )
        {
            Shoot();
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

        transform.LookAt(camera);
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

    private void Shoot()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            Rigidbody bullet = (Rigidbody)Instantiate(projectile, transform.position + transform.forward, transform.rotation);
            bullet.AddForce(transform.forward * bulletImpulse, ForceMode.Impulse);
            playerHealth.currentHealth -= damage;
            Destroy(bullet.gameObject, 2);
        }
    }
}
