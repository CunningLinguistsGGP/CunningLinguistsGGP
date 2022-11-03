using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public float enemyCooldown;

    private float timer;
    private bool playerInRange;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;

    private new Transform camera;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private Transform projectileSpawn;
    
    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] AudioSource audioShot;

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

        if (timer >= enemyCooldown && playerInRange)
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
    
    void Shoot()
    {
        timer = 0f;
        
        if (playerHealth.currentHealth > 0)
        {
            if(mzzlFlash!=null)
                mzzlFlash.Play();
                
            if(audioShot!=null)
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
