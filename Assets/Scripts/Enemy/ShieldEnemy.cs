using UnityEngine;
using UnityEngine.AI;

public class ShieldEnemy : MonoBehaviour
{
    public float enemyCooldown;

    private float timer;
    private bool playerInRange;

    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;
    private MeshRenderer glow;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private Transform projectileSpawn;
    
    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] AudioSource audioShot;

    //Grapple
    private bool stunned;
    private float stunTime = 2f;
    private Target enemy;

    private void Start()
    {
        glow = GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();
        enemy = GetComponent<Target>();
    }
    
    private void Update()
    {
        timer += Time.deltaTime;

        if (stunned)
        {
            agent.isStopped = true;

            enemy.SetDoubleDamage(true);

            stunTime -= Time.deltaTime;

            if (stunTime <= 0f)
            {
                stunned = false;
                agent.isStopped = false;
                enemy.SetDoubleDamage(false);
                stunTime = 2f;
            }
        }
        else
        {
            if (timer >= Random.Range(enemyCooldown, enemyCooldown + 2.0f) && playerInRange)
            {
                Shoot();
            }
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
    
    void Shoot()
    {
        timer = 0.0f;
        
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

    public bool SetStunned(bool stun)
    {
        return stunned = stun;
    }
}
