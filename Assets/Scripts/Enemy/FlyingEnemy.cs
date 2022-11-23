using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public float enemyCooldown;

    private float timer;
    private bool playerInRange;
    public float offMeshLinkSpeed;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;
    private new Transform camera;
    private float originalSpeed;
    private MeshRenderer glow;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private Transform projectileSpawn;
    
    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] AudioSource audioShot;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        glow = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
        originalSpeed = agent.speed;
        
        if (Camera.main is not null)
        {
            camera = Camera.main.transform;
        }
        
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
        
        if (timer >= enemyCooldown && playerInRange)
        {
            Shoot();
        }

        if (player != null)
        {
            agent.destination = player.transform.position;
        }

        transform.LookAt(camera);
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
    
    void Shoot()
    {
        timer = Random.Range(0f, 1.5f);
        
        if (playerHealth.currentHealth > 0)
        {
            if (mzzlFlash != null)
            {
                mzzlFlash.Play();
            }
            
            if(audioShot != null)
            {
                audioShot.Play();
                audioShot.SetScheduledEndTime(AudioSettings.dspTime + enemyCooldown);
            }
                
            GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - projectileSpawn.position).normalized * shotSpeed;
            Destroy(newProjectile, 2.0f);
        }
    }
}
