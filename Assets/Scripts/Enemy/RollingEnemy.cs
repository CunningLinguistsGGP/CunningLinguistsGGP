using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RollingEnemy : MonoBehaviour
{
    public float enemyCooldown;
    public float damage;
    public float offMeshLinkSpeed;
    
    private float timer;
    private bool playerInRange;
    private float originalSpeed;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
        originalSpeed = agent.speed;
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (agent.isOnOffMeshLink)
        {
            agent.speed = offMeshLinkSpeed;
        }
        else if (!agent.isOnOffMeshLink)
        {
            agent.speed = originalSpeed;
        }
        
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
            agent.SetDestination(player.transform.position);
        }
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
