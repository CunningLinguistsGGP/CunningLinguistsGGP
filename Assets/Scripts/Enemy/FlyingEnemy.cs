using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    //public Rigidbody projectile;
    //public float range = 50.0f;
    //public float bulletImpulse = 20.0f;

    public float radius;
    public float enemyCooldown;
    public float damage;

    private float timer;
    private bool playerInRange;
    
    private NavMeshAgent agent;
    private GameObject player;
    private PlayerScript playerHealth;

    private new Transform camera;

    [SerializeField] private GameObject projectile;
    [SerializeField] private bool infiniteAmmo = true;
    [SerializeField] private float shootCooldown = 1.0f, shotSpeed = 10.0f, targetRange = 50.0f;
    [SerializeField] private Transform projectileSpawn;
    
    [SerializeField] private ParticleSystem mzzlFlash;
    [SerializeField] AudioSource audioShot;

    private bool canShoot = true;
    private float lastShotTime = 0.0f;


  
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

        if (lastShotTime + shootCooldown < Time.time)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
        if (canShoot)
        {
            Shoot();
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
    
    void Shoot()
    {
        Vector3 forward = transform.forward;
        Vector3 rayDir = forward;

        Ray ray = new Ray(transform.position, rayDir);
        RaycastHit hit;

        if(mzzlFlash!=null)
            mzzlFlash.Play();
        if(audioShot!=null)
        {
            audioShot.Play();
            audioShot.SetScheduledEndTime(AudioSettings.dspTime + shootCooldown);
        }
        
        Vector3 targetPos = ray.GetPoint(targetRange);
        if (Physics.Raycast(transform.position, rayDir, out hit))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                if (playerHealth.currentHealth > 0)
                {
                    targetPos = hit.point;
                    playerHealth.currentHealth -= damage;
                }
            }
        }
        GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().velocity = rayDir.normalized * shotSpeed;
        Destroy(newProjectile, 2.0f);
        
        lastShotTime = Time.time;
    }
}
