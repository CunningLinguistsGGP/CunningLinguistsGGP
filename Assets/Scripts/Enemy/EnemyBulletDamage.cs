using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletDamage : MonoBehaviour
{
    public float damage;
    
    private GameObject player;
    private PlayerScript playerHealth;
    private bool playerInRange;
    
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            Attack();
            Destroy(gameObject);    
        }
    }
    
    private void Attack()
    {
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.currentHealth -= damage;
        }
    }
}
