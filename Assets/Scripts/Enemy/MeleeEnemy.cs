using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MonoBehaviour
{
    public float enemyCooldown;
    public float damage;
    public float offMeshLinkSpeed;
    public new AudioSource audio;
    public ParticleSystem hit;
    
    private float timer;
    private bool playerInRange;
    private float originalSpeed;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;
    private MeshRenderer glow;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        glow = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
        originalSpeed = agent.speed;

        agent.avoidancePriority = Random.Range(0, 99);
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
        }
        
        if (player != null && agent.enabled)
        {
            agent.destination = player.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            glow.material.EnableKeyword("_EMISSION");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            glow.material.DisableKeyword("_EMISSION");
            playerInRange = false;
        }
    }

    private void Attack()
    {
        timer = 0f;

        if (playerHealth.currentHealth > 0)
        {
            if (hit != null)
            {
                hit.Play();
            }
            
            if(audio != null)
            {
                audio.Play();
                audio.SetScheduledEndTime(AudioSettings.dspTime + enemyCooldown);
            }

            StartCoroutine(Stop(1.0f));
            playerHealth.currentHealth -= damage;
            playerHealth.SetSliderHealth(playerHealth.currentHealth);
        }
    }
    
    IEnumerator Stop(float time)
    {
        agent.isStopped = true;
        yield return new WaitForSeconds(time);
        agent.isStopped = false;
    }
}
