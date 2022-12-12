using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float shotCooldown;

    private float timer;

    private GameObject player;
    private PlayerScript playerHealth;
    private Vector3 lastPlayerPos = Vector3.zero;
    private Vector3 playerPredVel = Vector3.zero;

    public float healthRatio = 1.0f;

    private new Transform camera;


    [SerializeField] private GameObject[] shields;

    [SerializeField] private GameObject projectile;
    [SerializeField] private float shotSpeed = 10.0f;
    [SerializeField] private Transform projectileSpawn;
    [SerializeField] private GameObject projectileDirector;

    private int difficulty = 1;//1 = easy,2=mid,3=hard
    private int phase = 0;
    private int numPhases = 1;


    private Level_Gen levelGen;

    [SerializeField] private ParticleSystem mzzlFlash;

    void Start()
    {

        levelGen = GameObject.Find("Level_Gen").GetComponent<Level_Gen>();
        difficulty = levelGen.GetDifficulty();

        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerScript>();


        if (Camera.main is not null)
        {
            camera = Camera.main.transform;
        }

    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        playerPredVel = (player.transform.position - lastPlayerPos) / Time.deltaTime;



        switch (difficulty)
        {
            //easy
            case 1:
                break;
            //mid
            case 2:
                if (healthRatio < 0.5f)
                    phase++;
                break;
            //hard
            case 3:
                if(phase==0)
                {
                    if (healthRatio < 0.667f)
                        phase = 1;
                }
                if(phase == 1)
                {
                    if (healthRatio < 0.334f)
                        phase = 1;
                }
                break;
            //default to easy
            default:
                break;
        }

        if (timer >= shotCooldown)
        {
            timer = 0.0f;
            Shoot();
        }

        projectileDirector.transform.LookAt(player.transform);
        lastPlayerPos = player.transform.position;
    }


    void Shoot()
    {
        switch (phase)
        {
            case 0:
                ShootDumb();
                break;
            case 1:
                ShootPredict();
                break;
            case 2:
                break;
            default: ShootDumb();
                break;
        }
    }

    void ShootPredict()
    {
        if (playerHealth.currentHealth > 0)
        {
            if (mzzlFlash != null)
            {
                mzzlFlash.Play();
            }

            GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - projectileSpawn.position).normalized * shotSpeed;
            Destroy(newProjectile, 2.0f);
        }

    }

    void ShootDumb()
    {
        if (playerHealth.currentHealth > 0)
        {
            if (mzzlFlash != null)
            {
                mzzlFlash.Play();
            }

            GameObject newProjectile = Instantiate(projectile, projectileSpawn.position, projectileSpawn.rotation);
            newProjectile.GetComponent<Rigidbody>().velocity = (player.transform.position - projectileSpawn.position).normalized * shotSpeed;
            Destroy(newProjectile, 2.0f);
        }
    }

}
